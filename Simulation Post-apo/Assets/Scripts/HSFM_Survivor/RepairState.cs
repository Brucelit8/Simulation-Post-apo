using UnityEngine;
using System.Collections;
using System;

public class RepairState : ISurvivor
{
    private readonly SurvivorBasicState survivor;
    private GameObject Home;

    float repairingTime = 0.0f;
    bool isRepairing = false;
    bool endRepairing = false;

    // Use this for initialization
    void Start () {
	
	}

    public RepairState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }


    public void UpdateState ()
    {
	    if(Home == null && survivor.homeSet)
        {
            Home = survivor.home;
        }

        if(!isRepairing)
        {
            isRepairing = true;
            Home.GetComponent<House>().setScrap(Home.GetComponent<House>().getScrap() - 1);
            repairingTime = Time.fixedTime;

        }
        else
        {
            if(Time.fixedTime - repairingTime >= 5.0f)
            {
                Home.GetComponent<House>().setSafety(Home.GetComponent<House>().getSafety() + 1);
                endRepairing = true;
            }
        }

        if(endRepairing)
        {
            isRepairing = false;
            endRepairing = false;
            survivor.home.GetComponent<House>().setSign(0);
            ToHomeState();
        }
    }

    void repair()
    {

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
        survivor.currentState = survivor.homeState;
    }

    public void ToRepairState()
    {

    }

    public void ToSleepState()
    {
        survivor.currentState = survivor.sleepState;
    }

    public void ToHealState()
    {

    }
}
