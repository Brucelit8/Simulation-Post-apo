using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurvivorBasicState : MonoBehaviour
{
    public SpriteRenderer sword;
    public LayerMask layerM;
    public LayerMask layerMBuilding;
    private int ID;
    public float speed = 2.3f;
    public Vector3 startingPosition;
    List<Vector3> wayPointsList;

    public ISurvivor currentState;
    public CollectState collectState;
    public BuildState buildState;
    public NourrishState nourrishState;
    public FightState fightState;
    public RepairState repairState;
    public HomeState homeState; 
    public SleepState sleepState;
    public HealState healState;


    public int loopCounter = 0;
    public int survivorFood;
    public int survivorWater;
    public int survivorBandage;
    public int survivorScrap;

    public GameObject home;
    public bool homeSet;
    public bool destInBuilding = false;

    float survivorHunger;
    float survivorThirst;
    float survivorHealth;
    float survivorTiredness;

    public GameObject currentMap;

    void Awake()
    {
        homeSet = false;
        survivorFood = 0;
        survivorWater = 0;
        survivorBandage = 0;
        survivorScrap = 0;

        survivorHealth = 100;
        survivorHunger = 90;
        survivorThirst = 90;
        survivorTiredness = 90;

        wayPointsList = new List<Vector3>();

        collectState = new CollectState(this);
        buildState = new BuildState(this);
        nourrishState = new NourrishState(this);
        fightState = new FightState(this);
        repairState = new RepairState(this);
        sleepState = new SleepState(this);
        homeState = new HomeState(this);
        healState = new HealState(this);
        currentMap = GameObject.Find("Map");
    }

    // Use this for initialization
    void Start()
    {
        currentState = collectState;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -15)
        {
            //Debug.Log("test y" + " " + transform.position.y);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            currentState = collectState;
            collectState.Initialize();
            transform.position = new Vector3(startingPosition.x, startingPosition.y + 0.2f, startingPosition.z);
        }

        currentState.UpdateState();
    }

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    public int getID() { return ID; }
    public float getSurvivorHealth() { return survivorHealth; }
    public float getSurvivorHunger() { return survivorHunger; }
    public float getSurvivorThirst() { return survivorThirst; }
    public float getSurvivorTiredness() { return survivorTiredness; }

    public void setID(int i) { ID = i; }
    public void setSurvivorHealth(float h) { survivorHealth = h; }
    public void setSurvivorHunger(float h) { survivorHunger = h; }
    public void setSurvivorThirst(float t) { survivorThirst = t; }
    public void setSurvivorTiredness(float t) { survivorTiredness = t; }

    public List<Vector3> getWayPointsList() { return wayPointsList; }

    public void setHome(GameObject g)
    {
        home = g;
        homeSet = true;
    }

    public GameObject getMap() { return currentMap; }

    public void checkBuildingHit(Vector3 destination, GameObject destBuilding, bool moving, bool ignoringDestination)
    {
        RaycastHit hit;
        loopCounter = 0;
        if (Physics.Linecast(this.GetComponent<Collider>().bounds.center, destination, out hit, layerMBuilding))
        {
            if (ignoringDestination)
            {
                if (destBuilding != null && hit.collider.gameObject.transform.position != destBuilding.transform.position)
                {
                    getAroundBuilding(hit.collider.gameObject, destination, destBuilding, transform.gameObject);
                }
                //wayPointsList.Insert(wayPointsList.Count, destination);
            }
            else
            {
                float indiceMaxX = hit.collider.gameObject.GetComponent<Collider>().bounds.max.x + 0.05f;
                float indiceMinX = hit.collider.gameObject.GetComponent<Collider>().bounds.min.x - 0.05f;
                float indiceMaxZ = hit.collider.gameObject.GetComponent<Collider>().bounds.max.z + 0.05f;
                float indiceMinZ = hit.collider.gameObject.GetComponent<Collider>().bounds.min.z - 0.05f;
                if ((destination.x > indiceMaxX || destination.x < indiceMinX) || (destination.z > indiceMaxZ || destination.z < indiceMinZ))
                {
                    getAroundBuilding(hit.collider.gameObject, destination, destBuilding, transform.gameObject);
                    //wayPointsList.Insert(wayPointsList.Count, destination);
                }
                else
                {
                    destInBuilding = true;
                }
            }
        }
        //else
            //wayPointsList.Insert(wayPointsList.Count, destination);
    }
    void getAroundBuilding(GameObject building, Vector3 destination, GameObject destBuilding, GameObject source)
    {
        List<GameObject> buildingNodeList = new List<GameObject>();
        foreach (Transform child in building.transform)
        {
            if (child.tag == "Node")
            {
                buildingNodeList.Add(child.gameObject);
            }
        }
        if (buildingNodeList.Count > 0)
        {
            determinePath(buildingNodeList, destination, source, destBuilding, building);
        }
    }
    void determinePath(List<GameObject> nodeList, Vector3 destination, GameObject source, GameObject destBuilding, GameObject originalHitBuilding)
    {
        List<GameObject> reachableNodes = new List<GameObject>();
        RaycastHit h;
        if (source.tag == "Node")
            source.GetComponent<Collider>().enabled = false;
        else if (source.tag == "Agent")
        {
            foreach (GameObject n in nodeList)
            {
                if (source.GetComponent<Collider>().bounds.Intersects(n.GetComponent<Collider>().bounds))
                {
                    n.GetComponent<Collider>().enabled = false;
                }
            }
        }
        foreach (GameObject n in nodeList)
        {
            if (Physics.Linecast(source.transform.position, n.transform.position, out h, layerM))
            {
                if (h.collider.gameObject.tag == "Node" && h.collider.gameObject == n)
                    reachableNodes.Add(n);
            }
        }
        GameObject bestOption = null;
        if (reachableNodes.Count == 3)
        {
            bestOption = reachableNodes[0];
            foreach (GameObject v in reachableNodes)
            {
                if (Vector3.Distance(bestOption.transform.position, destination) > Vector3.Distance(v.transform.position, destination))
                    bestOption = v;
            }
            wayPointsList.Insert(wayPointsList.Count, bestOption.transform.position);
            //bestOption.GetComponent<Collider>().enabled = false;
        }
        else if (reachableNodes.Count == 2)
        {
            bestOption = reachableNodes[0];
            if (Vector3.Distance(bestOption.transform.position, destination) > Vector3.Distance(reachableNodes[1].transform.position, destination))
                bestOption = reachableNodes[1];
            foreach (GameObject v in reachableNodes)
            {
                nodeList.Remove(v);
            }
            wayPointsList.Insert(wayPointsList.Count, bestOption.transform.position);
            bestOption.GetComponent<Collider>().enabled = false;
            reachableNodes.Clear();

            foreach (GameObject v in nodeList)
            {
                if (Physics.Linecast(bestOption.transform.position, v.transform.position, out h, layerM))
                {
                    if (h.collider.gameObject.tag == "Node" && h.collider.gameObject == v)
                        reachableNodes.Add(v);
                }
                if (reachableNodes.Count > 0)
                {
                    wayPointsList.Insert(wayPointsList.Count, reachableNodes[0].transform.position);
                    bestOption = reachableNodes[0];
                    //reachableNodes[0].GetComponent<Collider>().enabled = false;
                }
            }
        }
        else if (reachableNodes.Count == 1)
        {
            wayPointsList.Insert(wayPointsList.Count, reachableNodes[0].transform.position);
            bestOption = reachableNodes[0];
            //reachableNodes[0].GetComponent<Collider>().enabled = false;
        }
        foreach (Transform child in originalHitBuilding.transform)
        {
            if (child.tag == "Node")
            {
                child.gameObject.GetComponent<Collider>().enabled = true;
            }
        }
        if (Physics.Linecast(bestOption.transform.position, destination, out h, layerMBuilding))
        {
            if (loopCounter > 3)
            {
                if (destBuilding != null && h.collider.gameObject != destBuilding)
                {
                    loopCounter++;
                //    Debug.Log(h.collider.gameObject + " " + h.collider.gameObject.transform.position + " " + destBuilding + " " + destBuilding.transform.position);
                    getAroundBuilding(h.collider.gameObject, destination, destBuilding, bestOption);
                }
                else if (destBuilding == null)
                {
                 //   Debug.Log(h.collider.gameObject + " " + h.collider.gameObject.transform.position + " " + destBuilding + " " + destBuilding.transform.position);
                    getAroundBuilding(h.collider.gameObject, destination, destBuilding, bestOption);
                }
            }
        }

    }

}
