using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SurvivorBasicState : MonoBehaviour
{
    private int ID;
    public float speed = 2.3f;
    public Vector3 direction;
    List<Vector3> wayPointsList;

    public ISurvivor currentState;
    public CollectState collectState;
    public BuildState buildState;
    public NourrishState nourrishState;
    public FightState fightState;
    public RepairState repairState;
    public HomeState homeState;
    public SleepState sleepState;

    public int survivorFood;
    public int survivorWater;
    public int survivorBandage;
    public int survivorScrap;

    public GameObject home;
    public bool homeSet;

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
        //fightState = new FightState();
        repairState = new RepairState(this);
        sleepState = new SleepState(this);
        homeState = new HomeState(this);

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

    public void checkBuildingHit(Vector3 destination, GameObject destBuilding, bool moving, bool ignoringDestination)
    {
        LayerMask layerM = 1 << 8;
        layerM = ~layerM;
        RaycastHit hit;

        if (Physics.Linecast(new Vector3(this.transform.position.x, this.transform.position.y + 0.15f, this.transform.position.z),
                    new Vector3(destination.x, destination.y + 0.15f, destination.z), out hit, layerM))
        {
            if (ignoringDestination)
            {
                if (destBuilding != null && hit.collider.gameObject != destBuilding)
                {
                    getAroundBuilding(this.transform.position, hit.collider.gameObject, destination, destBuilding);
                }

                wayPointsList.Insert(wayPointsList.Count, destination);
            }
            else
            {
                float indiceMaxX = hit.collider.bounds.max.x + 0.05f;
                float indiceMinX = hit.collider.bounds.min.x - 0.05f;
                float indiceMaxZ = hit.collider.bounds.max.z + 0.05f;
                float indiceMinZ = hit.collider.bounds.min.z - 0.05f;

                if ((destination.x > indiceMaxX || destination.x < indiceMinX) || (destination.z > indiceMaxZ || destination.z < indiceMinZ))
                {
                    getAroundBuilding(this.transform.position, hit.collider.gameObject, destination, destBuilding);
                    wayPointsList.Insert(wayPointsList.Count, destination);
                }
            }
        }

        else
            wayPointsList.Insert(wayPointsList.Count, destination);
    }

    void getAroundBuilding(Vector3 actualPosition, GameObject building, Vector3 destination, GameObject destBuilding)
    {
        List<Vector3> buildingNodeList = new List<Vector3>();

        foreach (Transform child in building.transform)
        {
            if (child.tag == "Node")
            {
                buildingNodeList.Add(child.transform.position);
            }
        }

        if (buildingNodeList.Count > 0)
        {
            Vector3 survivorPos = new Vector3(actualPosition.x, actualPosition.y + 0.1f, actualPosition.z);
            Vector3 destPos = new Vector3(destination.x, destination.y + 0.1f, destination.z);
            determinePath(buildingNodeList, destPos, survivorPos, false, destBuilding);
        }

        /*Debug.Log("#### LIST ###");
        foreach (Vector3 v in wayPointsList)
            Debug.Log(v);*/
    }

    void determinePath(List<Vector3> nodeList, Vector3 destination, Vector3 survivorPos, bool exitReached, GameObject destBuilding)
    {
        if (!exitReached)
        {
            List<Vector3> reachableNodes = new List<Vector3>();
            LayerMask layerM = 1 << 8;
            layerM = ~layerM;
            RaycastHit h;


            foreach (Vector3 n in nodeList)
            {
                Vector3 nPos = new Vector3(n.x, n.y + 0.05f, n.z);

                if (Physics.Linecast(survivorPos, nPos, out h, layerM))
                {
                    if (h.collider.gameObject.tag == "Node")
                        reachableNodes.Add(n);
                }
            }
            /*
            Debug.Log("### NODES REACHABLES ###");
            Debug.Log(reachableNodes.Count);
            */
            if (reachableNodes.Count > 0)
            {
                Vector3 bestOption = reachableNodes[0];

                if (reachableNodes.Count > 1)
                {
                    foreach (Vector3 v in reachableNodes)
                    {
                        if (Vector3.Distance(bestOption, destination) > Vector3.Distance(v, destination))
                            bestOption = v;
                    }
                }

                wayPointsList.Insert(wayPointsList.Count, new Vector3(bestOption.x, this.transform.position.y, bestOption.z));

                foreach (Vector3 n in reachableNodes)
                    nodeList.Remove(n);

                if (Physics.Linecast(new Vector3(bestOption.x, bestOption.y + 0.1f, bestOption.z), destination, out h, layerM))
                {
                    if (destBuilding != null && h.collider.gameObject == destBuilding)
                        exitReached = true;
                    else
                    {
                        if (nodeList.Count <= 1)
                        {
                            if (h.collider.gameObject.tag != "Node")
                            {
                                getAroundBuilding(new Vector3(bestOption.x, transform.position.y, bestOption.z), h.collider.gameObject, destination, destBuilding);
                            }
                        }
                    }
                }

                else
                {
                    exitReached = true;
                }

                determinePath(nodeList, destination, new Vector3(bestOption.x, bestOption.y + 0.1f, bestOption.z), exitReached, destBuilding);
            }
            // else
            //   moving = false;
        }
    }

}
