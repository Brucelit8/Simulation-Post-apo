using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Remains : Building
{

    // Use this for initialization
    void Start()
    {
        FeedInventory();
        safety = 0;
        bed = 0;
        water = (int)I.items["Water"];
        food = (int)I.items["Food"];

        t_type = GameObject.Find("Type").GetComponent<Text>();
        t_safety = GameObject.Find("Safety").GetComponent<Text>();
        t_bed = GameObject.Find("Bed").GetComponent<Text>();
        t_water = GameObject.Find("Water").GetComponent<Text>();
        t_food = GameObject.Find("Food").GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (changed)
        {
            Debug.Log("Changement");
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

    public void Details()
    {
        t_type.text = "Type : Remains";
        t_safety.text = "Safety : " + safety;
        t_bed.text = "Bed : " + bed;
        t_water.text = "Water : " + water;
        t_food.text = "Food : " + food;
    }

    public void FeedInventory()
    {
        I.items["Water"] = (int)Random.Range(0f, 3f);
        I.items["Food"] = (int)Random.Range(0f, 3f);
        I.items["Wood"] = (int)Random.Range(2f, 15f);
        I.items["Stone"] = (int)Random.Range(2f, 15f);
        I.items["Steel"] = (int)Random.Range(2f, 15f);
        I.items["Electronics"] = (int)Random.Range(2f, 15f);
    }
}
