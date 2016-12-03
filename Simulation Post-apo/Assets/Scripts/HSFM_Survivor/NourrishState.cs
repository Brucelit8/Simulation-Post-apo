using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NourrishState : ISurvivor
{
    private readonly SurvivorBasicState survivor;

    bool wayPointsListInstantiated = false;
    bool roadHomeSet = false;

    float detectionRange;
    float minThirst = 40.0f;
    float minHunger = 40.0f;

    public NourrishState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

    // Use this for initialization
    void Start()
    {

    }

    public void UpdateState()
    {
        if (survivor.getSurvivorHunger() > minHunger && survivor.getSurvivorThirst() > minThirst)
            ToCollectState();

        if (survivor.survivorFood != 0 && survivor.getSurvivorHunger() < minHunger)
        {
            survivor.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            survivor.survivorFood -= 1;
            survivor.setSurvivorHunger(survivor.getSurvivorHunger() + 50);
        }

        if (survivor.survivorWater != 0 && survivor.getSurvivorThirst() < minThirst)
        {
            survivor.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            survivor.survivorWater -= 1;
            survivor.setSurvivorThirst(survivor.getSurvivorThirst() + 50);
        }

        else if ((survivor.survivorFood == 0 && survivor.getSurvivorHunger() < minHunger) || (survivor.survivorWater == 0 && survivor.getSurvivorThirst() < minThirst))
            goBackHome();
    }

    void goBackHome()
    {
        if (!roadHomeSet)
        {
            survivor.getWayPointsList().Clear();
            survivor.checkBuildingHit(survivor.home.transform.position, true);
            survivor.getWayPointsList().Add(survivor.home.transform.position);
            roadHomeSet = true;
        }

        if (survivor.getWayPointsList().Count > 1)
            detectionRange = 0.3f;
        else
            detectionRange = 1.3f;

        Debug.Log("Detection: " + detectionRange + ", Dest : " + survivor.getWayPointsList()[0]);
        survivor.transform.position = Vector3.MoveTowards(survivor.transform.position, survivor.getWayPointsList()[0], survivor.speed * Time.deltaTime);

        if (Vector3.Distance(survivor.transform.position, survivor.getWayPointsList()[0]) < detectionRange)
        {
            if (survivor.getWayPointsList().Count == 1)
            {
                survivor.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;

                if (survivor.getSurvivorHunger() < minHunger && survivor.home.GetComponent<Building>().getFood() > 0)
                {
                    survivor.home.GetComponent<Building>().setFood(survivor.home.GetComponent<Building>().getFood() - 1);
                    survivor.setSurvivorHunger(survivor.getSurvivorHunger() + 50);
                }

                if (survivor.getSurvivorThirst() < minThirst && survivor.home.GetComponent<Building>().getWater() > 0)
                {
                    survivor.home.GetComponent<Building>().setWater(survivor.home.GetComponent<Building>().getWater() - 1);
                    survivor.setSurvivorThirst(survivor.getSurvivorThirst() + 50);
                }

            }
            else
            {
                survivor.getWayPointsList().RemoveAt(0);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToBuildState()
    {

    }

    public void ToCollectState()
    {
        survivor.currentState = survivor.collectState;
    }

    public void ToFightState()
    {

    }

    public void ToNourrishState()
    {

    }

    public void ToHomeState()
    {

    }

    public void ToRepairState()
    {

    }

    public void ToSleepState()
    {
        survivor.currentState = survivor.sleepState;
    }
}
