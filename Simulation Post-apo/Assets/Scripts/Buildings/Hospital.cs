using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Hospital : Building {

	// Use this for initialization
	void Start ()
    {
        if (GameObject.Find("UserValues"))
        {
            coefR = (float)UserValues.nbR / 500.0f;
        }
        else
        {
            coefR = 1.0f;
        }

        safety = (int)Random.Range(0f, 10f);
        bed = (int)Random.Range(10f, 100f);
        water = (int)(Random.Range(0f, 10f) * coefR);
        food = (int)(Random.Range(0f, 3f) * coefR);
        bandage = (int)(Random.Range(8f, 15f) * coefR);
        scrap = (int)(Random.Range(1f, 5f) * coefR);
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
        VariablesUI.t_scrap.text = "Scrap : " + scrap;

    }

    public override void DeselectAllOthers()
    {
        GameObject M = GameObject.Find("MapManager");

        foreach (Transform child in M.transform)
        {
            GameObject G = child.gameObject;
            if (G.tag == "House" || G.tag == "Supermarket" || G.tag == "Hospital" || G.tag == "Remains")
            {
                string s = G.tag;
                string here = this.gameObject.tag;
                int x1 = (GetComponent(here) as Building).getX();
                int x2 = (G.GetComponent(s) as Building).getX();
                int y1 = (GetComponent(here) as Building).getY();
                int y2 = (G.GetComponent(s) as Building).getY();

                if (x1 != x2 || y1 != y2)
                {
                    (G.GetComponent(s) as Building).changed = true;
                    (G.GetComponent(s) as Building).selected = false;
                }
            }
        }
    }
}
