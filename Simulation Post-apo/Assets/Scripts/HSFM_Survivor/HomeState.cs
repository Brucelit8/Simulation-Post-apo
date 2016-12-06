using UnityEngine;
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
        survivor.GetComponent<Statistics>().AgentVisible(false);

        if (survivor.getSurvivorHunger() < 15 || survivor.getSurvivorThirst() < 15)
        {
            Debug.Log("MANGER");
            ToNourrishState();
        }

        else if( survivor.getSurvivorHealth() <= 50)
        {
            Debug.Log("SOIN");
            ToHealState();
        }

        else if (survivor.getSurvivorTiredness() < 15)
        {
            Debug.Log("DODO");
            survivor.home.GetComponent<House>().setSign(1);
            ToSleepState();
        }

        else if (survivor.collectState.getNoResources() && (!survivor.home.GetComponent<House>().haveFarm 
            || !survivor.home.GetComponent<House>().haveWell) && survivor.home.GetComponent<House>().LowResources())
        {
            survivor.home.GetComponent<House>().setSign(0);
            survivor.GetComponent<Statistics>().AgentVisible(true);
            survivor.sword.enabled = true;
            Debug.Log("A LA BATAILLE!");
            ToFightState();
        }

        else if ((!survivor.home.GetComponent<House>().haveWell || !survivor.home.GetComponent<House>().haveFarm) && survivor.home.GetComponent<House>().getScrap() >= 2)
        {
            survivor.home.GetComponent<House>().setSign(3);
            Debug.Log("BUILDING");
            ToBuildState();
        }

        else if (survivor.home.GetComponent<House>().getSafety() < 10 && survivor.home.GetComponent<House>().getScrap() >= 1)
        {
            survivor.home.GetComponent<House>().setSign(2);
            Debug.Log("REPARO!");
            ToRepairState();
        }

        else
        {
            survivor.home.GetComponent<House>().setSign(0);
            survivor.GetComponent<Statistics>().AgentVisible(true);
            ToCollectState();
        }
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToBuildState()
    {
        survivor.currentState = survivor.buildState;
    }

    public void ToCollectState()
    {
        survivor.currentState = survivor.collectState;
    }

    public void ToFightState()
    {
        survivor.currentState = survivor.fightState;
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

    public void ToHealState()
    {
        survivor.currentState = survivor.healState;
    }
}
