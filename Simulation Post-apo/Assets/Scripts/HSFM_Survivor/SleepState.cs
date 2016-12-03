using UnityEngine;
using System.Collections;

public class SleepState : ISurvivor
{
    private readonly SurvivorBasicState survivor;
    bool isSleeping = false;
    float currentTime;

    public SleepState(SurvivorBasicState survivorState)
    {
        survivor = survivorState;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	public void UpdateState ()
    {
        if(!isSleeping)
        {
            isSleeping = true;
            currentTime = Time.fixedTime;
        }
        else
        {
            if (survivor.getSurvivorTiredness() < 90)
            {
                if(Time.fixedTime - currentTime >= 1.0f)
                {
                    survivor.setSurvivorTiredness(survivor.getSurvivorTiredness() + 10);
                    currentTime = Time.fixedTime;
                }
            }
            else
            {
                isSleeping = false;
                ToHomeState();
            }
        }
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

    public void OnTriggerEnter(Collider other)
    {

    }
}
