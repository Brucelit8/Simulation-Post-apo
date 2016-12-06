using UnityEngine;
using System.Collections;
using System;

public class HealState : ISurvivor {

    public bool isHome = false;
    private readonly SurvivorBasicState survivor;

    public HealState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

    public void OnTriggerEnter(Collider other)
    {
    }

    public void ToBuildState()
    {
    }

    public void ToCollectState()
    {
    }

    public void ToFightState()
    {
    }

    public void ToHealState()
    {
    }

    public void ToSleepState()
    {
        survivor.currentState = survivor.sleepState;
    }

    public void ToHomeState()
    {
        survivor.currentState = survivor.homeState;
    }

    public void ToNourrishState()
    {
    }

    public void ToRepairState()
    {
    }

    // Use this for initialization
    void Start()
    {

    }
    public void UpdateState()
    {
        if(survivor.home.GetComponent<House>().getBandage() > 0)
        {
            survivor.setSurvivorHealth(100.0f);
            survivor.GetComponent<House>().setBandage(survivor.GetComponent<House>().getBandage() - 1 );
            ToHomeState();
        }
        else
        {
            ToSleepState();
        }
    }




}
