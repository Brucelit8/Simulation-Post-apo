using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class House : Building {

    public GameObject Sign;
    int availableBeds;
    int maxBeds;

    // Use this for initialization
    void Start () {

        safety = (int)Random.Range(0f, 5.0f);
        bed = (int)Random.Range(2f, 10f);
        water = (int)Random.Range(5f, 12f);
        food = (int)Random.Range(5f, 12f);
        bandage = (int)Random.Range(1f, 3f);
        scrap = (int)Random.Range(1f, 5f);

        availableBeds = bed;
        maxBeds = availableBeds;
    }

    // Update is called once per frame
    void Update()
    {

        if (changed)
        {
            if (selected)
            {
                string s = "Materials/House_3_s";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            else
            {
                string s = "Materials/House_3";
                Material[] mats = GetComponent<Renderer>().materials;
                mats[0] = Resources.Load(s, typeof(Material)) as Material;
                GetComponent<MeshRenderer>().materials = mats;
            }
            changed = false;
        }
    }

    public override void Details()
    {
        VariablesUI.t_type.text = "Type : House";
        VariablesUI.t_safety.text = "Safety : " + safety;
        VariablesUI.t_bed.text = "Bed : " + bed;
        VariablesUI.t_water.text = "Water : " + water;
        VariablesUI.t_food.text = "Food : " + food;
        VariablesUI.t_bandage.text = "Bandage : " + bandage;
        VariablesUI.t_scrap.text = "Scrap : " + scrap;

    }

    public void setRemainingBeds(int b)
    {
        availableBeds--;
        Details();
    }

    public int getRemainingBeds() { return availableBeds; }
    public int getMaxBeds() { return maxBeds; }

    public void setSign(int n)
    {
        switch(n)
        {
            case 0:
                Sign.GetComponent<SpriteRenderer>().enabled = false;
                Sign.transform.rotation = new Quaternion(0.0f, 0.0f, 0.0f, 0.0f);
                break;
            case 1:
                Sign.GetComponent<SpriteRenderer>().enabled = true;
                Sign.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/sleep");
                break;
            case 2:
                Sign.GetComponent<SpriteRenderer>().enabled = true;
                Sign.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/repair");
                break;
            case 3:
                Sign.GetComponent<SpriteRenderer>().enabled = true;
                Sign.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/build");
                break;

        }
    }

}
