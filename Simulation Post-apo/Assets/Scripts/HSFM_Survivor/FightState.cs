using UnityEngine;
using System.Collections;

public class FightState : ISurvivor {

    private bool moving = false, roadHomeSet = false;
    float rX, rZ;
    float movementRange = 4f;
    float detectionRange;
    Vector3 point;
    RaycastHit hit;

    private GameObject HouseToSteal;

    private readonly SurvivorBasicState survivor;


    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    public void UpdateState()
    {
        if (survivor.getSurvivorHunger() < 15 || survivor.getSurvivorThirst() < 15)
        {
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
                    int indiceX;
                    int indiceZ;

                    if (point.x - (int)point.x <= 0.5f)
                        indiceX = (int)point.x;
                    else
                        indiceX = (int)point.x + 1;

                    if (point.z - (int)point.z <= 0.5f)
                        indiceZ = (int)point.z;
                    else
                        indiceZ = (int)point.z + 1;

                    if (indiceX < 0)
                        indiceX = 0;
                    else if (indiceX > survivor.getMap().GetComponent<Map>().getSize() - 1)
                        indiceX = survivor.getMap().GetComponent<Map>().getSize() - 1;

                    if (indiceZ < 0)
                        indiceZ = 0;
                    else if (indiceZ > survivor.getMap().GetComponent<Map>().getSize() - 1)
                        indiceZ = survivor.getMap().GetComponent<Map>().getSize() - 1;

                    if (survivor.getMap().GetComponent<Map>().getMap()[indiceX, indiceZ] == 1)
                    {
                        moving = false;
                    }
                }
            }

            //CHECK WALL 2
            if (point.x <= 0.7f || point.x >= survivor.getMap().GetComponent<Map>().getSize() - 2 || point.z <= 0.7f || point.z >= survivor.getMap().GetComponent<Map>().getSize() - 2)
                moving = false;
            else
            {
                moving = true;
                survivor.checkBuildingHit(point, null, moving, false);

                if (survivor.destInBuilding)
                {
                    moving = false;
                    survivor.destInBuilding = false;
                }
            }
        }
        else
        {
            /*Debug.Log("#####");
            foreach (Vector3 v in survivor.getWayPointsList())
                Debug.Log(v);*/

            //ROAMING
            if (!HouseToSteal)
            {
                survivor.transform.position = Vector3.MoveTowards(survivor.transform.position, survivor.getWayPointsList()[0], survivor.speed * Time.deltaTime);
                Debug.DrawLine(survivor.transform.position, point);

                if (Vector3.Distance(survivor.transform.position, survivor.getWayPointsList()[0]) < 0.3f)
                {
                    if (survivor.getWayPointsList().Count == 1)
                    {
                        moving = false;
                    }
                    else
                        survivor.getWayPointsList().RemoveAt(0);
                }
            }
            else
            {
                if(!roadHomeSet)
                {
                    survivor.getWayPointsList().Clear();
                    survivor.checkBuildingHit(HouseToSteal.transform.position, HouseToSteal, true, true);
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
                        survivor.GetComponent<Rigidbody>().velocity = Vector3.zero;
                        //JE CASSE TOUT

                        int food = HouseToSteal.GetComponent<House>().getFood();
                        int water = HouseToSteal.GetComponent<House>().getWater();
                        Debug.Log("J'ai tout cassé : " + food + " de bouffe et " + water + " d'eau");

                        if (food >= 3)
                        {
                            HouseToSteal.GetComponent<House>().setFood(food - 3);
                            survivor.survivorFood = 3;
                        }
                        else
                        {
                            HouseToSteal.GetComponent<House>().setFood(0);
                            survivor.survivorFood = food;
                        }

                        if (water >= 3)
                        {
                            HouseToSteal.GetComponent<House>().setWater(water - 3);
                            survivor.survivorWater = 3;
                        }
                        else
                        {
                            HouseToSteal.GetComponent<House>().setWater(0);
                            survivor.survivorFood = water;
                        }

                        survivor.collectState.setGoingHome(true);
                        survivor.collectState.setMoving(true);
                        survivor.sword.enabled = false;

                        ToCollectState();
                    }
                    else
                        survivor.getWayPointsList().RemoveAt(0);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag=="House" && other.transform.position != survivor.home.transform.position &&
            other.transform.gameObject.GetComponent<House>().getRemainingBeds() 
            < other.transform.gameObject.GetComponent<House>().getMaxBeds())
        {
            Debug.Log("J'ai trouvé une maison de coord " + other.transform.position + " , ma maison : " + survivor.home.transform.position);
            HouseToSteal = other.transform.gameObject;
        }
    }

    public void ToBuildState()
    {
    }

    public void ToNourrishState()
    {
    }

    public void ToCollectState()
    {
        survivor.currentState = survivor.collectState;
    }

    public void ToFightState()
    {
    }

    public void ToHomeState()
    {
    }

    public void ToRepairState()
    {
    }

    public FightState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

    public void ToHealState()
    {

    }
}
