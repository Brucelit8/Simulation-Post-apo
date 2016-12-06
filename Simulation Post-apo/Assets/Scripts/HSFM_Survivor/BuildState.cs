using UnityEngine;
using System.Collections;

public class BuildState : ISurvivor
{
    float buildingTime = 0.0f;
    bool isBuilding = false;

    private bool haveWell = false;

    private readonly SurvivorBasicState survivor;

    public BuildState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }


	// Update is called once per frame
	public void UpdateState ()
    {
        if(!isBuilding)
        {
            isBuilding = true;
            buildingTime = Time.fixedTime;
        }
        else
        {
            if (!haveWell)
            {
                if(Time.fixedTime - buildingTime < 10.0f)
                {
                    survivor.home.GetComponent<House>().build(0);
                    buildingTime = Time.fixedTime;
                    isBuilding = false;
                    haveWell = true;
                    ToHomeState();
                }
            }
            else
            {
                if (Time.fixedTime - buildingTime < 10.0f)
                {
                    survivor.home.GetComponent<House>().build(1);
                    isBuilding = false;
                    buildingTime = Time.fixedTime;
                    survivor.home.GetComponent<House>().setSign(0);
                    ToHomeState();
                }
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
        survivor.currentState = survivor.nourrishState;
    }

    public void ToHomeState()
    {
        survivor.currentState = survivor.homeState;
    }

    public void ToRepairState()
    {
        survivor.currentState = survivor.repairState;
    }

    public void ToSleepState()
    {
        survivor.currentState = survivor.sleepState;
    }

    public void ToHealState()
    {

    }
}
