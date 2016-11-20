using UnityEngine;
using System.Collections;

public class SurvivorBasicState : MonoBehaviour {

    public float speed = 2.0f;
    public Vector3 direction;

    public Vector3 destination;
    public Transform target;
    public ISurvivor currentState;
    public CollectState collectState;
    public BuildState buildState;
    public NourrishState nourrishState;
    public FightState fightState;

    void Awake()
    {
        collectState = new CollectState(this);
        //buildState = new BuildState();
        //nourrishState = new NourrishState();
        //fightState = new FightState();
    }

	// Use this for initialization
	void Start () {
        currentState = collectState;
	}
	
	// Update is called once per frame
	void Update () {
        currentState.UpdateState();
	}

    void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}
