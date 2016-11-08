using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Supermarket : Building {

	// Use this for initialization
	void Start () {
        safety = (int)Random.Range(0f, 10f);
        bed = (int)Random.Range(2f, 10f);
        water = (int)Random.Range(0f, 10f);
        food = (int)Random.Range(0f, 10f);

        t_type = GameObject.Find("Type").GetComponent<Text>();
        t_safety = GameObject.Find("Safety").GetComponent<Text>();
        t_bed = GameObject.Find("Bed").GetComponent<Text>();
        t_water = GameObject.Find("Water").GetComponent<Text>();
        t_food = GameObject.Find("Food").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (changed)
        {
            if (selected)
            {
                string s = "Materials/House_1_s";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            else
            {
                string s = "Materials/House_1";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            changed = false;
        }
    }

    public override void Details()
    {
        t_type.text = "Type : Supermarket";
        t_safety.text = "Safety : " + safety;
        t_bed.text = "Bed : " + bed;
        t_water.text = "Water : " + water;
        t_food.text = "Food : " + food;
    }
}
