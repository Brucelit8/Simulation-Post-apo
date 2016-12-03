using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Remains : Building
{
    public GameObject ground;
    // Use this for initialization
    void Start()
    {
        safety = 0;
        bed = 0;
        water = (int)Random.Range(0f, 3f);
        food = (int)Random.Range(0f, 3f);
        bandage = (int)Random.Range(0f, 1.5f);
        scrap = (int)Random.Range(0f, 5f);
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
        Destroy(gameObject);
    }
}
