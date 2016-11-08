using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hospital : Building {

	// Use this for initialization
	void Start ()
    {
        safety = (int)Random.Range(0f, 10f);
        bed = (int)Random.Range(10f, 100f);
        water = (int)Random.Range(0f, 10f);
        food = (int)Random.Range(0f, 10f);
        bandage = (int)Random.Range(8f, 15f);
    }

    // Update is called once per frame
    void Update ()
    {
        if (changed)
        {
            if (selected)
            {
                string s = "Materials/House_4_s";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            else
            {
                string s = "Materials/House_4";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            changed = false;
        }
    }

    public override void Details()
    {
        VariablesUI.t_type.text = "Type : Hospital";
        VariablesUI.t_safety.text = "Safety : " + safety;
        VariablesUI.t_bed.text = "Bed : " + bed;
        VariablesUI.t_water.text = "Water : " + water;
        VariablesUI.t_food.text = "Food : " + food;
        VariablesUI.t_bandage.text = "Bandage : " + bandage;

    }

}
