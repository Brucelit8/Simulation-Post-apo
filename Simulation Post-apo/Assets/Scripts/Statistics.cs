using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Statistics : MonoBehaviour {

    // Use this for initialization

    public float hunger;
    public float thirst;
    public float health;
    public float tiredness;

    public bool selected = false;
    public bool changed = false;

	void Start ()
    {
        health = 100.0f;
        thirst = 80.0f;
        tiredness = 70.0f;
        hunger = 50.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(changed)
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

        if(selected)
        {
            VariablesUI.a_tiredness = tiredness;
            VariablesUI.a_thirst = thirst;
            VariablesUI.a_hunger = hunger;
            VariablesUI.a_health = health;
        }

        health -= 0.05f;

        if(hunger == 0.0f)
        {
            health -= 0.2f;
        }

        if(thirst == 0.0f)
        {
            health -= 0.5f;
        }

	    if(health <= 0.0f)
        {
            Destroy(gameObject);
        }
	}
}