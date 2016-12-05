using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Remains : Building
{
    public GameObject ground;
    // Use this for initialization
    void Start()
    {
        if (GameObject.Find("UserValues"))
        {
            coefR = (float)UserValues.nbR / 500.0f;
        }
        else
        {
            coefR = 1.0f;
        }


        safety = 0;
        bed = 0;
        water = (int)(Random.Range(0f, 3f) * coefR);
        food = (int)(Random.Range(0f, 3f) * coefR);
        bandage = (int)(Random.Range(0f, 1.5f) * coefR);
        scrap = (int)(Random.Range(0f, 5f) * coefR);
    }

    // Update is called once per frame
    void Update()
    {

        if(water == 0 && food == 0 && bandage == 0)
        {
            switchToGround();
        }

        if (changed)
        {
            if (selected)
            {
                string s = "Materials/Beton_s";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            else
            {
                string s = "Materials/Beton";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            changed = false;
        }
    }

    public override void Details()
    {
        VariablesUI.t_type.text = "Type : Remains";
        VariablesUI.t_safety.text = "Safety : " + safety;
        VariablesUI.t_bed.text = "Bed : " + bed;
        VariablesUI.t_water.text = "Water : " + water;
        VariablesUI.t_food.text = "Food : " + food;
        VariablesUI.t_bandage.text = "Bandage : " + bandage;
        VariablesUI.t_scrap.text = "Scrap : " + scrap;
    }

    void switchToGround()
    {
        Transform T = transform;
        transform.gameObject.SetActive(false);
        GameObject go = (GameObject)Instantiate(ground, T.position, T.rotation);
        go.transform.SetParent(GameObject.Find("MapManager").transform);
        go.GetComponent<GroundPositions>().setXY(this.GetComponent<Remains>().getX(), this.GetComponent<Remains>().getY());
        GameObject.Find("Map").GetComponent<Map>().setValue(this.GetComponent<Remains>().getX(), this.GetComponent<Remains>().getY(), 0);
        Destroy(gameObject);
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
