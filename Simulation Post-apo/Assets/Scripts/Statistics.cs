using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Statistics : MonoBehaviour
{

    // Use this for initialization

    public float hunger;
    public float thirst;
    public float health;
    public float tiredness;

    public bool selected = false;
    public bool changed = false;

    public GameObject A;

    void Start()
    {
        health = this.GetComponent<SurvivorBasicState>().getSurvivorHealth();
        thirst = this.GetComponent<SurvivorBasicState>().getSurvivorThirst();
        tiredness = this.GetComponent<SurvivorBasicState>().getSurvivorTiredness();
        hunger = this.GetComponent<SurvivorBasicState>().getSurvivorHunger();


    }

    // Update is called once per frame
    void Update()
    {
        if (changed)
        {
            if (selected)
            {

                string s = "Materials/Cha_Knight-Face_s";

                Component[] R = GetComponentsInChildren(typeof(Renderer));
                foreach (Renderer r in R)
                {
                    if (r.name == "Knight")
                    {
                        Material[] mats = r.GetComponent<Renderer>().materials;
                        mats[0] = Resources.Load(s, typeof(Material)) as Material;
                        r.GetComponent<SkinnedMeshRenderer>().materials = mats;
                    }
                }

                if(GetComponent<SurvivorBasicState>().homeSet)
                {
                    if(!GetComponent<SurvivorBasicState>().home.GetComponent<House>().selected)
                    {
                        string n = "Materials/House_3_s2";
                        Material[] mats = GetComponent<SurvivorBasicState>().home.GetComponent<Renderer>().materials;
                        mats[0] = Resources.Load(n, typeof(Material)) as Material;
                        GetComponent<SurvivorBasicState>().home.GetComponent<MeshRenderer>().materials = mats;
                    }
                }
            }
            else
            {
                string s = "Materials/Cha_Knight-Face";

                Component[] R = GetComponentsInChildren(typeof(Renderer));
                foreach (Renderer r in R)
                {
                    if (r.name == "Knight")
                    {
                        Material[] mats = r.GetComponent<Renderer>().materials;
                        mats[0] = Resources.Load(s, typeof(Material)) as Material;
                        r.GetComponent<SkinnedMeshRenderer>().materials = mats;
                    }
                }

                if (GetComponent<SurvivorBasicState>().homeSet)
                {
                    if (!GetComponent<SurvivorBasicState>().home.GetComponent<House>().selected)
                    {
                        string n = "Materials/House_3";
                        Material[] mats = GetComponent<SurvivorBasicState>().home.GetComponent<Renderer>().materials;
                        mats[0] = Resources.Load(n, typeof(Material)) as Material;
                        GetComponent<SurvivorBasicState>().home.GetComponent<MeshRenderer>().materials = mats;
                    }
                }
            }
            changed = false;
        }

        if (selected)
        {
            VariablesUI.a_tiredness = this.GetComponent<SurvivorBasicState>().getSurvivorTiredness();
            VariablesUI.a_thirst = this.GetComponent<SurvivorBasicState>().getSurvivorThirst();
            VariablesUI.a_hunger = this.GetComponent<SurvivorBasicState>().getSurvivorHunger();
            VariablesUI.a_health = this.GetComponent<SurvivorBasicState>().getSurvivorHealth();
        }


        if (this.GetComponent<SurvivorBasicState>().getSurvivorHunger() - 0.02f > 0)
        {
            this.GetComponent<SurvivorBasicState>().setSurvivorHunger(this.GetComponent<SurvivorBasicState>().getSurvivorHunger() - 0.02f * Time.timeScale);
        }
        else
            this.GetComponent<SurvivorBasicState>().setSurvivorHunger(0);

        if (this.GetComponent<SurvivorBasicState>().getSurvivorThirst() - 0.03f > 0)
            this.GetComponent<SurvivorBasicState>().setSurvivorThirst(this.GetComponent<SurvivorBasicState>().getSurvivorThirst() - 0.03f * Time.timeScale);
        else
            this.GetComponent<SurvivorBasicState>().setSurvivorThirst(0);

        if (this.GetComponent<SurvivorBasicState>().getSurvivorTiredness() - 0.01f > 0)
            this.GetComponent<SurvivorBasicState>().setSurvivorTiredness(this.GetComponent<SurvivorBasicState>().getSurvivorTiredness() - 0.01f * Time.timeScale);
        else
            this.GetComponent<SurvivorBasicState>().setSurvivorTiredness(0);

        if (this.GetComponent<SurvivorBasicState>().getSurvivorHunger() == 0.0f)
        {
            this.GetComponent<SurvivorBasicState>().setSurvivorHealth(this.GetComponent<SurvivorBasicState>().getSurvivorHealth() - 0.2f * Time.timeScale);
        }

        if (this.GetComponent<SurvivorBasicState>().getSurvivorThirst() == 0.0f)
        {
            this.GetComponent<SurvivorBasicState>().setSurvivorHealth(this.GetComponent<SurvivorBasicState>().getSurvivorHealth() - 0.5f * Time.timeScale);
        }

        if (this.GetComponent<SurvivorBasicState>().getSurvivorHealth() <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void AgentVisible(bool b)
    {
        Component[] R = GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer r in R)
        {
            if (r.name == "Knight")
            {
                if (b)
                {
                    r.enabled = true;
                }
                else
                {
                    r.enabled = false;
                }
            }
        }
    }

    public void DeselectAllOthers()
    {
       GameObject M = GameObject.Find("AgentManager");

        foreach ( Transform child in M.transform)
        {
            GameObject G = child.gameObject;
            if(GetComponent<SurvivorBasicState>().getID() != G.GetComponent<SurvivorBasicState>().getID())
            {
                G.GetComponent<Statistics>().changed = true;
                G.GetComponent<Statistics>().selected = false;
            }
        }
    }
}