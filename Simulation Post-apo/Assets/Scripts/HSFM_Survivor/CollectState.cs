using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectState : ISurvivor
{
    private readonly SurvivorBasicState survivor;
    bool moving = false;
    float rX, rZ;
    float movementRange = 4f;
    bool wayPointsListInstantiated = false;
    bool lastPointReached = false;

    bool collectingRessources = false;
    bool goingHome = false;
    bool roadHomeSet = false;
    bool stateChanged = false;
    float detectionRange;

    GameObject buildingCollected;

    Vector3 point;
    Vector3 destinationRotation;
    Ray ray;
    RaycastHit hit;

    List<Vector3> wayPointsList;
    Vector3 nextDestination;


    public CollectState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

    // Use this for initialization
    void Start()
    {
        wayPointsList = survivor.getWayPointsList();
    }

    // Update is called once per frame
    public void UpdateState()
    {
        if (survivor.getSurvivorHunger() < 15 || survivor.getSurvivorThirst() < 15)
        {
            stateChanged = true;
            ToNourrishState();
        }

        if (!moving)
        {
            /*
            if (!wayPointsListInstantiated)
            {
                wayPointsList = survivor.getWayPointsList();
                wayPointsListInstantiated = true;
            }
            */
            survivor.getWayPointsList().Clear();

            rX = Random.Range(survivor.transform.position.x - movementRange, survivor.transform.position.x + movementRange);
            rZ = Random.Range(survivor.transform.position.z - movementRange, survivor.transform.position.z + movementRange);

            point = new Vector3(rX, survivor.transform.position.y, rZ);

            // CHECK WALL 1
            if (Physics.Linecast(new Vector3(survivor.transform.position.x, survivor.transform.position.y + 0.2f, survivor.transform.position.z),
                new Vector3(point.x, point.y + 0.2f, point.z), out hit))
            {
                if (hit.collider.gameObject.tag == "Wall")
                {
                    Vector3 destRange = new Vector3(point.x - survivor.transform.position.x, point.y - survivor.transform.position.y, point.z - survivor.transform.position.z);
                    destRange = Quaternion.Euler(0, 180, 0) * destRange;
                    Debug.DrawLine(survivor.transform.position, destRange, Color.red);
                    point.x = survivor.transform.position.x + destRange.x;
                    point.y = survivor.transform.position.y + destRange.y;
                    point.z = survivor.transform.position.z + destRange.z;
                }
            }

            //CHECK WALL 2
            if (point.x <= 0 || point.x >= survivor.currentMap.GetComponent<Map>().size - 1 || point.z <= 0 || point.z >= survivor.currentMap.GetComponent<Map>().size - 1)
                moving = false;
            else
            {
                moving = true;
                lastPointReached = false;
                survivor.checkBuildingHit(point, moving);
                survivor.getWayPointsList().Insert(survivor.getWayPointsList().Count, point);
            }
        }
        else
        {
            /*Debug.Log("#####");
            foreach (Vector3 v in survivor.getWayPointsList())
                Debug.Log(v);*/

            //ROAMING
            if (!collectingRessources && !goingHome)
            {
                if (!lastPointReached)
                {
                    survivor.transform.position = Vector3.MoveTowards(survivor.transform.position, survivor.getWayPointsList()[0], survivor.speed * Time.deltaTime);
                    Debug.DrawLine(survivor.transform.position, point);

                    if (Vector3.Distance(survivor.transform.position, survivor.getWayPointsList()[0]) < 0.3f)
                    {
                        if (survivor.getWayPointsList().Count == 1)
                        {
                            lastPointReached = true;
                            moving = false;
                        }
                        else
                            survivor.getWayPointsList().RemoveAt(0);
                    }
                }

                else
                    moving = false;
            }

            //COLLECTING
            else if (collectingRessources && !goingHome)
            {
                survivor.transform.position = Vector3.MoveTowards(survivor.transform.position, survivor.getWayPointsList()[0], survivor.speed * Time.deltaTime);

                if (Vector3.Distance(survivor.transform.position, survivor.getWayPointsList()[0]) < 1.3f)
                {
                    survivor.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    if (buildingCollected != null)
                    {
                        //COLLECTING FOOD
                        if (survivor.survivorFood + 1 <= 3 && buildingCollected.GetComponent<Building>().getFood() > 0)
                        {
                            survivor.survivorFood += 1;
                            buildingCollected.GetComponent<Building>().setFood(buildingCollected.GetComponent<Building>().getFood() - 1);
                            string s = buildingCollected.tag;
                            if ((buildingCollected.GetComponent(s) as Building).selected)
                            {
                                (buildingCollected.GetComponent(s) as Building).Details();
                            }
                        }

                        //COLLECTING WATER
                        if (survivor.survivorWater + 1 <= 3 && buildingCollected.GetComponent<Building>().getWater() > 0)
                        {
                            survivor.survivorWater += 1;
                            buildingCollected.GetComponent<Building>().setWater(buildingCollected.GetComponent<Building>().getWater() - 1);
                            string s = buildingCollected.tag;
                            if ((buildingCollected.GetComponent(s) as Building).selected)
                            {
                                (buildingCollected.GetComponent(s) as Building).Details();
                            }
                        }

                        //COLLECTING BANDAGE
                        if (survivor.survivorBandage + 1 <= 3 && buildingCollected.GetComponent<Building>().getBandage() > 0)
                        {
                            survivor.survivorBandage += 1;
                            buildingCollected.GetComponent<Building>().setBandage(buildingCollected.GetComponent<Building>().getBandage() - 1);
                            string s = buildingCollected.tag;
                            if ((buildingCollected.GetComponent(s) as Building).selected)
                            {
                                (buildingCollected.GetComponent(s) as Building).Details();
                            }
                        }

                        //COLLECTING SCRAP
                        if (survivor.survivorScrap + 1 <= 3 && buildingCollected.GetComponent<Building>().getScrap() > 0)
                        {
                            survivor.survivorScrap += 1;
                            buildingCollected.GetComponent<Building>().setScrap(buildingCollected.GetComponent<Building>().getScrap() - 1);
                            string s = buildingCollected.tag;
                            if ((buildingCollected.GetComponent(s) as Building).selected)
                            {
                                (buildingCollected.GetComponent(s) as Building).Details();
                            }
                        }
                    }
                    else
                    {
                        collectingRessources = false;
                        moving = false;
                    }
                    if (survivor.survivorFood == 3 || survivor.survivorWater == 3 || survivor.survivorBandage == 3 || survivor.survivorScrap == 3)
                    {
                        if (survivor.homeSet)
                        {
                            collectingRessources = false;
                            goingHome = true;
                        }
                    }

                    else
                    {
                        collectingRessources = false;
                        moving = false;
                    }
                }
            }

            //GOING HOME
            else if (!collectingRessources && goingHome)
            {
                //if (survivor.survivorFood < 3 && survivor.survivorWater < 3 && survivor.survivorBandage < 3)
                //  moving = false;

                if (!roadHomeSet)
                {
                    survivor.getWayPointsList().Clear();
                    survivor.checkBuildingHit(survivor.home.transform.position, moving);
                    survivor.getWayPointsList().Insert(survivor.getWayPointsList().Count, survivor.home.transform.position);
                    roadHomeSet = true;
                }

                if (survivor.getWayPointsList().Count > 1)
                    detectionRange = 0.3f;
                else
                    detectionRange = 1.3f;

                survivor.transform.position = Vector3.MoveTowards(survivor.transform.position, survivor.getWayPointsList()[0], survivor.speed * Time.deltaTime);

                if (Vector3.Distance(survivor.transform.position, survivor.getWayPointsList()[0]) < detectionRange)
                {
                    if (survivor.getWayPointsList().Count == 1)
                    {
                        survivor.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                        survivor.home.GetComponent<House>().setFood(survivor.home.GetComponent<House>().getFood() + survivor.survivorFood);
                        survivor.survivorFood = 0;
                        survivor.home.GetComponent<House>().setWater(survivor.home.GetComponent<House>().getWater() + survivor.survivorWater);
                        survivor.survivorWater = 0;
                        survivor.home.GetComponent<House>().setWater(survivor.home.GetComponent<House>().getBandage() + survivor.survivorBandage);
                        survivor.survivorBandage = 0;
                        survivor.home.GetComponent<House>().setWater(survivor.home.GetComponent<House>().getScrap() + survivor.survivorScrap);
                        survivor.survivorScrap = 0;

                        if(survivor.home.GetComponent<House>().selected)
                        {
                            survivor.home.GetComponent<House>().Details();
                        }

                        goingHome = false;
                        roadHomeSet = false;
                        moving = false;

                        ToHomeState();

                    }
                    else
                    {
                        survivor.getWayPointsList().RemoveAt(0);
                    }
                }
            }
        }

        if(stateChanged)
        {
            goingHome = false;
            collectingRessources = false;
            moving = false;
            stateChanged = false;
        }
    }
    /*
    void checkBuildingHit(Vector3 destination)
    {
        LayerMask layerM = 1 << 8;
        layerM = ~layerM;

        if (Physics.Linecast(survivor.transform.position, destination, out hit))
        {
            if (hit.collider.gameObject.tag == "House" || hit.collider.gameObject.tag == "Supermarket" || hit.collider.gameObject.tag == "Hospital")
            {
                BoxCollider b = hit.collider.gameObject.GetComponent<BoxCollider>();

                if (!b.bounds.Contains(destination))
                    getAroundBuilding(survivor.transform.position, hit.collider.gameObject, destination);
                else if (hit.collider.gameObject != survivor.home)
                    moving = false;
            }
        }
    }
    */
    public void ToBuildState()
    {

    }

    public void ToFightState()
    {

    }

    public void ToNourrishState()
    {
        survivor.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        moving = false;
        survivor.currentState = survivor.nourrishState;
    }

    public void ToCollectState()
    {

    }

    public void ToHomeState()
    {
        survivor.currentState = survivor.homeState;
    }

    public void ToRepairState()
    {

    }

        public void ToSleepState()
    {
        survivor.currentState = survivor.sleepState;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!goingHome && !collectingRessources)
        {
            if (other.gameObject.tag == "House")
            {
                //HOME SET
                if (!survivor.homeSet && other.gameObject.GetComponent<House>().getRemainingBeds() > 0)
                {
                    survivor.setHome(other.gameObject);
                    other.gameObject.GetComponent<House>().setRemainingBeds(other.gameObject.GetComponent<House>().getRemainingBeds() - 1);
                }

                //GETTING RESSOURCES FROM UNHABITED HOME
                else if (survivor.homeSet && other.gameObject.GetComponent<House>().getRemainingBeds() == other.gameObject.GetComponent<House>().getMaxBeds())
                {
                    buildingCollected = other.gameObject;
                    survivor.getWayPointsList().Clear();
                    survivor.checkBuildingHit(buildingCollected.transform.position, moving);
                    survivor.getWayPointsList().Insert(survivor.getWayPointsList().Count, new Vector3(other.gameObject.transform.position.x, survivor.transform.position.y,
                            other.gameObject.transform.position.z));
                    collectingRessources = true;
                }
            }

            //GETTING RESSOURCES FROM OTHER BUILDINGS
            else if (other.gameObject.tag == "Supermarket" || other.gameObject.tag == "Hospital" || other.gameObject.tag == "Remains")
            {
                buildingCollected = other.gameObject;
                collectingRessources = true;
                survivor.getWayPointsList().Clear();
                survivor.checkBuildingHit(buildingCollected.transform.position, moving);
                survivor.getWayPointsList().Insert(survivor.getWayPointsList().Count,new Vector3(other.gameObject.transform.position.x, survivor.transform.position.y,
                        other.gameObject.transform.position.z));
            }
        }
    }

    /*
    void getAroundBuilding(Vector3 actualPosition, GameObject building, Vector3 destination)
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
            Vector3 destPos = new Vector3(point.x, point.y + 0.1f, point.z);
            determinePath(buildingNodeList, destPos, survivorPos, false);
        }
    }

    void determinePath(List<Vector3> nodeList, Vector3 destination, Vector3 survivorPos, bool exitReached)
    {
        if (!exitReached)
        {
            List<Vector3> reachableNodes = new List<Vector3>();
            LayerMask layerM = 1 << 8;
            layerM = ~layerM;
            RaycastHit h;

            foreach (Vector3 n in nodeList)
            {
                Vector3 nPos = new Vector3(n.x, n.y + 0.1f, n.z);

                if (Physics.Linecast(survivorPos, nPos, out h, layerM))
                {
                    if (h.collider.gameObject.tag == "Node")
                        reachableNodes.Add(n);
                }
            }

            //Debug.Log(reachableNodes.Count + " " + exitReached);

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

                wayPointsList.Insert(wayPointsList.Count, new Vector3(bestOption.x, survivor.transform.position.y, bestOption.z));

                foreach (Vector3 n in reachableNodes)
                    nodeList.Remove(n);

                if (!Physics.Linecast(new Vector3(bestOption.x, bestOption.y + 0.1f, bestOption.z), destination, out h, layerM))
                {
                    exitReached = true;
                }

                else
                {
                    if (nodeList.Count <= 1)
                    {
                        if (h.collider.gameObject.tag != "Node" && h.collider.gameObject != survivor.home)
                        {
                            getAroundBuilding(new Vector3(bestOption.x, survivor.transform.position.y, bestOption.z), h.collider.gameObject, destination);
                            exitReached = true;
                        }
                    }
                }

                determinePath(nodeList, destination, new Vector3(bestOption.x, bestOption.y + 0.1f, bestOption.z), exitReached);
            }
            else
                moving = false;
        }
    }
    */
}
