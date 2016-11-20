using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CollectState : ISurvivor {

    private readonly SurvivorBasicState survivor;
    bool moving = false;
    float rX, rZ;
    float movementRange = 2f;
    bool wayPointsListInstantiated = false;

    Vector3 point;
    Ray ray;
    RaycastHit hit;

    List<Vector3> wayPointsList;
    Vector3 nextDestination;


    public CollectState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	public void UpdateState ()
    {
        if (!moving)
        {
            if (!wayPointsListInstantiated)
                wayPointsList = new List<Vector3>();

            moving = true;

            rX = Random.Range(survivor.transform.position.x - movementRange, survivor.transform.position.x + movementRange);
            rZ = Random.Range(survivor.transform.position.z - movementRange, survivor.transform.position.z + movementRange);

            point = new Vector3(rX, survivor.transform.position.y, rZ);
            // ray = new Ray(survivor.gameObject.GetComponent<BoxCollider>().bounds.extents, point);

            if (Physics.Linecast(survivor.transform.position, point, out hit))
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

            wayPointsList.Add(point);
            checkBuildingHit();
        }

        else
        {
            if (wayPointsList.Count > 0)
            {
                survivor.transform.position = Vector3.MoveTowards(survivor.transform.position, wayPointsList[0], survivor.speed * Time.deltaTime);
                Debug.DrawLine(survivor.transform.position, point);

                if (Vector3.Distance(survivor.transform.position, point) < 0.1f)
                {
                    wayPointsList.RemoveAt(0);
                }
            }

            else
                moving = false;
        }
    }

    void checkBuildingHit()
    {
        if (Physics.Linecast(survivor.transform.position, point, out hit))
        {
            if (hit.collider.gameObject.tag == "House" || hit.collider.gameObject.tag == "Supermarket" || hit.collider.gameObject.tag == "Hospital")
            {
                BoxCollider b = hit.collider.gameObject.GetComponent<BoxCollider>();

                if (!hit.collider.bounds.Contains(point))
                    getAroundBuilding(hit.collider.gameObject, point);
                else
                    moving = false;
            }
        }
    }

    public void ToBuildState()
    {

    }

    public void ToFightState()
    {

    }

    public void ToNourrishState()
    {

    }

    public void ToCollectState()
    {

    }

    public void OnTriggerEnter(Collider other)
    {

    }

    void getAroundBuilding(GameObject building, Vector3 destination)
    {
        List<Vector3> buildingNodeList = new List<Vector3>();

        foreach(Transform child in building.transform)
        {
            if(child.tag == "Node")
            {
                buildingNodeList.Add(child.transform.position);
            }
        }
      
        if (buildingNodeList.Count > 0)
        {
            Vector3 survivorPos = new Vector3(survivor.transform.position.x, survivor.transform.position.y + 0.2f, survivor.transform.position.z);
            determinePath(buildingNodeList, point, survivorPos, false);
            wayPointsList.Insert(wayPointsList.Count, point);    
        }
       
    }

    void determinePath(List<Vector3> nodeList, Vector3 destination, Vector3 survivorPos, bool exitReached)
    {
        /*
        Debug.Log("####");
        foreach(Vector3 v in wayPointsList)
            Debug.Log(v + " , index : " + wayPointsList.IndexOf(v));
        */

        if(nodeList.Count != 0 || !exitReached)
        {
            List<Vector3> reachableNodes = new List<Vector3>();
            RaycastHit h;

            foreach(Vector3 n in nodeList)
            {
                Vector3 nPos = new Vector3(n.x, n.y + 0.2f, n.z);

                if (Physics.Linecast(survivorPos, nPos, out h))
                {
                    if (h.collider.gameObject.tag == "Node")
                        reachableNodes.Add(n);
                }
            }

            Vector3 bestOption = reachableNodes[0];
            reachableNodes.RemoveAt(0);

            if (reachableNodes.Count > 0)
            {
                foreach (Vector3 rN in reachableNodes)
                {
                    if (Vector3.Distance(rN, destination) < Vector3.Distance(bestOption, destination))
                    {
                        bestOption = rN;
                    }
                }
            }

            wayPointsList.Insert(wayPointsList.Count, new Vector3(bestOption.x, survivorPos.y, bestOption.z));
            nodeList.Remove(bestOption);

            if (Physics.Linecast(bestOption, destination))
                exitReached = true;

            determinePath(nodeList, destination, survivorPos, exitReached);
        }
    }
}
