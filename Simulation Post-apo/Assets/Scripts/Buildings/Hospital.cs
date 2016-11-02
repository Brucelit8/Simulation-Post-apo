using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hospital : Building {

	// Use this for initialization
	void Start () {
        safety = (int)Random.Range(0.0f, 10.0f);
        bed = (int)Random.Range(2.0f, 10.0f);
        water = (int)Random.Range(0.0f, 10.0f);
        food = (int)Random.Range(0.0f, 10.0f);

        t_type = GameObject.Find("Type").GetComponent<Text>();
        t_safety = GameObject.Find("Safety").GetComponent<Text>();
        t_bed = GameObject.Find("Bed").GetComponent<Text>();
        t_water = GameObject.Find("Water").GetComponent<Text>();
        t_food = GameObject.Find("Food").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Hospital(int s, int b, int w, int f) :base(s,b,w,f){ }

    public void Details()
    {
        t_type.text = "Type : Hospital";
        t_safety.text = "Safety : " + safety;
        t_bed.text = "Bed : " + bed;
        t_water.text = "Water : " + water;
        t_food.text = "Food : " + food;
    }
}
