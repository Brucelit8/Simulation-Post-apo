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
                Debug.Log("Coucou la selection");

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

        //health -= 0.05f;

        if (this.GetComponent<SurvivorBasicState>().getSurvivorHunger() - 0.02f > 0)
        {
            this.GetComponent<SurvivorBasicState>().setSurvivorHunger(this.GetComponent<SurvivorBasicState>().getSurvivorHunger() - 0.02f);
        }
        else
            this.GetComponent<SurvivorBasicState>().setSurvivorHunger(0);

        if (this.GetComponent<SurvivorBasicState>().getSurvivorThirst() - 0.02f > 0)
            this.GetComponent<SurvivorBasicState>().setSurvivorThirst(this.GetComponent<SurvivorBasicState>().getSurvivorThirst() - 0.02f);
        else
            this.GetComponent<SurvivorBasicState>().setSurvivorThirst(0);

        if (this.GetComponent<SurvivorBasicState>().getSurvivorHunger() == 0.0f)
        {
            this.GetComponent<SurvivorBasicState>().setSurvivorHealth(this.GetComponent<SurvivorBasicState>().getSurvivorHealth() - 0.2f);
        }

        if (this.GetComponent<SurvivorBasicState>().getSurvivorThirst() == 0.0f)
        {
            this.GetComponent<SurvivorBasicState>().setSurvivorHealth(this.GetComponent<SurvivorBasicState>().getSurvivorHealth() - 0.5f);
        }

        if (this.GetComponent<SurvivorBasicState>().getSurvivorHealth() <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}