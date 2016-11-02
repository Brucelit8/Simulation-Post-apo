using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Building : MonoBehaviour{

    protected Text t_type;
    protected Text t_safety;
    protected Text t_bed;
    protected Text t_water;
    protected Text t_food;
    protected Inventory I;
    protected int safety;
    protected int bed;
    protected int water;
    protected int food;

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
	
	}

    public Building(int s, int b, int w, int f)
    {
        safety = s;
        bed = b;
        water = w;
        food = f;


    }

    public Building() { }


}
