﻿using UnityEngine;
using System.Collections;

public class HomeState : ISurvivor
{
    private readonly SurvivorBasicState survivor;

    public HomeState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void UpdateState ()
    {
        

        if (survivor.getSurvivorHunger() < 15 || survivor.getSurvivorThirst() < 15)
        {
            Debug.Log("MANGER");
            ToNourrishState();
        }

        else if (survivor.getSurvivorTiredness() < 15)
        {
            Debug.Log("DODO");
            ToSleepState();
        }

        else if (survivor.home.GetComponent<House>().getSafety() < 10 && survivor.home.GetComponent<House>().getScrap() >= 3)
        {
            Debug.Log("CONSTRUIRE");
            ToRepairState();
        }

        else
        {
            ToCollectState();
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
        survivor.currentState = survivor.nourrishState;
    }

    public void ToHomeState()
    {

    }

    public void ToRepairState()
    {
        survivor.currentState = survivor.repairState;
    }

    public void ToSleepState()
    {
        survivor.currentState = survivor.sleepState;
    }
}
