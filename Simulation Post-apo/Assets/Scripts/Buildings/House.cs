using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class House : Building {

    public GameObject Sign, farm, well;
    int availableBeds;
    int maxBeds;
    float spawnTime = 0.0f;

    public bool haveWell = false;
    public bool haveFarm = false;
    
    // Use this for initialization
    void Start () {

        if (GameObject.Find("UserValues"))
        {
            coefR = (float)UserValues.nbR / 500.0f;
        }
        else
        {
            coefR = 1.0f;
        }

        safety = (int)Random.Range(0f, 5.0f);
        bed = (int)Random.Range(2f, 10f);
        water = (int)(Random.Range(5f, 12f) * coefR);
        food = (int)(Random.Range(5f, 12f) * coefR);
        bandage = (int)(Random.Range(1f, 3f) * coefR);
        scrap = (int)(Random.Range(1f, 5f) * coefR);
        spawnTime = Time.fixedTime;
        availableBeds = bed;
        maxBeds = availableBeds;
    }

    // Update is called once per frame
    void Update()
    {

        if(Time.fixedTime - spawnTime > 4.0f)
        {
            if(haveWell)
            {
                water += 1;
            }
            if(haveFarm)
            {
                food += 1;
            }
            spawnTime = Time.fixedTime;
        }
        
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

    public void build(int v)
    {
        int size = GameObject.Find("Map").GetComponent<Map>().getSize();
        int[,] M = GameObject.Find("Map").GetComponent<Map>().getMap();

        int ind1=0, ind2=0;

        for(int i = getX()-1; i<getX()+2; i++)
        {
            for (int j = getY()-1; j < getY()+2; j++)
            {
                if(i> 0 && i < size - 1 && j > 0 && j < size - 1)
                {
                    if ((i != this.getX() || j != this.getY()) && M[i, j] == 0)
                    {
                        ind1 = i;
                        ind2 = j;
                        break;
                    }
                }

            }
        }

        Component[] Grounds = GameObject.Find("MapManager").GetComponentsInChildren<Component>();
        foreach (Component G in Grounds)
        {
            if (G.tag == "ground" && G.GetComponent<GroundPositions>().getX() == ind1 && G.GetComponent<GroundPositions>().getY() == ind2)
            {
                Transform T = G.transform;
                T.gameObject.SetActive(false);
                if (v == 0)
                {
                    M[ind1, ind2] = 7;
                    GameObject go = (GameObject)Instantiate(well, T.position, T.rotation);
                    go.transform.SetParent(GameObject.Find("MapManager").transform);
                    haveWell = true;
                }
                else
                {
                    M[ind1, ind2] = 6;
                    GameObject go = (GameObject)Instantiate(farm, T.position, T.rotation);
                    go.transform.SetParent(GameObject.Find("MapManager").transform);
                    haveFarm = true;
                }

                Destroy(G.gameObject);
            }
        }


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
