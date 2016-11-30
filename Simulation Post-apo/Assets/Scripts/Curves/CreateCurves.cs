using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateCurves : MonoBehaviour {

    public LineRenderer LR1, LR2, LR3;
    float timeCurrent;
    public float timeLimit = 0.2f;
    public Transform origin;
    public float xScaleA, yScaleA, xScaleF, yScaleF, xScaleW, yScaleW;
    private Vector3 lastA, lastF, lastW;
    int nbAgents;
    int nbFood;
    int nbWater;
    bool fullA = false, fullF = false, fullW = false;
    int[] valuesA, valuesF, valuesW;
    int v_size;
    public enum state { Agents, Food, Water, All };
    public state current;
    public TextMesh a_text, f_text, w_text;
    // Use this for initialization

    void Start ()
    {
        nbAgents = GameObject.Find("AgentManager").transform.childCount;
        v_size = 10;
        valuesA = new int[v_size];
        valuesF = new int[v_size];
        valuesW = new int[v_size];

        LR1.SetVertexCount(v_size);
        LR2.SetVertexCount(v_size);
        LR3.SetVertexCount(v_size);

        LR1.SetColors(Color.green, Color.green);
        LR2.SetColors(Color.red, Color.red);
        LR3.SetColors(Color.blue, Color.blue);

        timeCurrent = Time.fixedTime;

        for(int i=0; i<v_size;i++)
        {
            valuesA[i] = -1;
            valuesF[i] = -1;
            valuesW[i] = -1;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.fixedTime - timeCurrent >= timeLimit)
        {
            switch (current)
            {
                case state.Agents:
                {
                        nbAgents = countAgents();
                        draw(false);
                        updateText();
                        break;
                }
                case state.Food:
                {
                        nbFood = countFood();
                        draw(false);
                        updateText();
                        break;
                }
                case state.Water:
                {
                        nbWater = countWater();
                        draw(false);
                        updateText();
                        break;
                }
                case state.All:
                {
                        nbWater = countWater();
                        draw(true);
                        updateText();
                        break;
                }
                default:
                {
                        break;
                }
            }
            timeCurrent = Time.fixedTime;
        }
	}

    void draw(bool all)
    {
        /////////////////////////////////////////////////////////1
        int i = 0;

        int i1 = v_size-1, i2 = v_size - 1, i3 = v_size - 1;

        while(i<v_size && valuesA[i] != -1 && !fullA)
        {
            i++;
        }

        if(i == v_size-1)
        {
            fullA = true;
        }

        if(fullA)
        {
            for(int j=1; j<v_size;j++)
            {
                valuesA[j - 1] = valuesA[j];
            }

            valuesA[v_size - 1] = nbAgents;

        }
        else
        {
            valuesA[i] = nbAgents;
            i1 = i;
        }
        ////////////////////////////////////////////////////////2
        i = 0;

        while (i < v_size && valuesF[i] != -1 && !fullF)
        {
            i++;
        }

        if (i == v_size - 1)
        {
            fullF = true;
        }

        if (fullF)
        {
            for (int j = 1; j < v_size; j++)
            {
                valuesF[j - 1] = valuesF[j];
            }

            valuesF[v_size - 1] = nbFood;

        }
        else
        {
            valuesF[i] = nbFood;
            i2 = i;
        }
        /////////////////////////////////////////////////////////3
        i = 0;

        while (i < v_size && valuesW[i] != -1 && !fullW)
        {
            i++;
        }

        if (i == v_size - 1)
        {
            fullW = true;
        }

        if (fullW)
        {
            for (int j = 1; j < v_size; j++)
            {
                valuesW[j - 1] = valuesW[j];
            }

            valuesW[v_size - 1] = nbWater;

        }
        else
        {
            valuesW[i] = nbWater;
            i3 = i;

        }

        switch (current)
            {
                case state.All:
                {
                    for (int j = 0; j < v_size; j++)
                    {
                        LR1.SetPosition(j, new Vector3(origin.position.x + j * xScaleA, origin.position.y + yScaleA * valuesA[j], origin.position.z));
                        LR2.SetPosition(j, new Vector3(origin.position.x + j * xScaleF, origin.position.y + yScaleF * valuesF[j], origin.position.z));
                        LR3.SetPosition(j, new Vector3(origin.position.x + j * xScaleW, origin.position.y + yScaleW * valuesW[j], origin.position.z));
                    }
                    lastA = new Vector3(origin.position.x + i1 * xScaleA, origin.position.y + yScaleA * valuesA[i1], origin.position.z);
                    lastF = new Vector3(origin.position.x + i2 * xScaleF, origin.position.y + yScaleF * valuesF[i2], origin.position.z);
                    lastW = new Vector3(origin.position.x + i3 * xScaleW, origin.position.y + yScaleW * valuesW[i3], origin.position.z);
                    break;
                }

                case state.Agents:
                {
                    for (int j = 0; j < v_size; j++)
                    {
                        LR1.SetPosition(j, new Vector3(origin.position.x + j * xScaleA, origin.position.y + yScaleA * valuesA[j], origin.position.z));
                    }
                    lastA = new Vector3(origin.position.x + i1 * xScaleA, origin.position.y + yScaleA * valuesA[i1], origin.position.z);
                    break;
                }

                case state.Food:
                {
                    for (int j = 0; j < v_size; j++)
                    {
                        LR2.SetPosition(j, new Vector3(origin.position.x + j * xScaleF, origin.position.y + yScaleF * valuesF[j], origin.position.z));
                    }
                    lastF = new Vector3(origin.position.x +i2 * xScaleF, origin.position.y + yScaleF * valuesF[i2], origin.position.z);
                    break;
                }

             case state.Water:
                {
                    for (int j = 0; j < v_size; j++)
                    {
                        LR3.SetPosition(j, new Vector3(origin.position.x + j * xScaleW, origin.position.y + yScaleW * valuesW[j], origin.position.z));
                    }
                    lastW = new Vector3(origin.position.x + i3 * xScaleW, origin.position.y + yScaleW * valuesW[i3], origin.position.z);
                    break;
                }

        }
        
    }

    public int countFood()
    {
        int cpt = 0;

        foreach( Transform child in GameObject.Find("MapManager").transform)
        {
            if (child.tag == "Remains")
            {
                cpt += child.GetComponentInChildren<Remains>().getFood();
            }
            else if (child.tag == "House")
            {
                cpt += child.GetComponentInChildren<House>().getFood();
            }
            else if (child.tag == "Hospital")
            {
                cpt += child.GetComponentInChildren<Hospital>().getFood();
            }
            else if (child.tag == "Supermarket")
            {
                cpt += child.GetComponentInChildren<Supermarket>().getFood();
            }
        }

        return cpt;
    }

    public int countWater()
    {
        int cpt = 0;

        foreach (Transform child in GameObject.Find("MapManager").transform)
        {
            if (child.tag == "Remains")
            {
                cpt += child.GetComponentInChildren<Remains>().getWater();
            }
            else if (child.tag == "House")
            {
                cpt += child.GetComponentInChildren<House>().getWater();
            }
            else if (child.tag == "Hospital")
            {
                cpt += child.GetComponentInChildren<Hospital>().getWater();
            }
            else if (child.tag == "Supermarket")
            {
                cpt += child.GetComponentInChildren<Supermarket>().getWater();
            }
        }

        return cpt;
    }

    public int countAgents()
    {
        return GameObject.Find("AgentManager").transform.childCount;
    }

    public void setAgents()
    {
        LR1.enabled = false;
        LR2.enabled = false;
        LR3.enabled = false;

        for (int i =0; i< v_size; i++)
        {
            valuesA[i] = -1;
        }
        fullA = false;
        LR1.enabled = true;
        current = state.Agents;
    }

    public void setFood()
    {
        LR1.enabled = false;
        LR2.enabled = false;
        LR3.enabled = false;
        for (int i = 0; i < v_size; i++)
        {
            valuesF[i] = -1;
        }
        fullF = false;
        LR2.enabled = true;
        current = state.Food;
    }

    public void setWater()
    {
        LR1.enabled = false;
        LR2.enabled = false;
        LR3.enabled = false;
        for (int i = 0; i < v_size; i++)
        {
            valuesW[i] = -1;
        }
        fullW = false;
        LR3.enabled = true;
        current = state.Water;
    }

    public void setAll()
    {
        LR1.enabled = false;
        LR2.enabled = false;
        LR3.enabled = false;
        for (int i = 0; i < v_size; i++)
        {
            valuesA[i] = -1;
            valuesF[i] = -1;
            valuesW[i] = -1;

        }

        fullA = false;
        fullF = false;
        fullW = false;

        LR1.enabled = true;
        LR2.enabled = true;
        LR3.enabled = true;
        current = state.All;
    }

    public void updateText()
    {
        a_text.text = "";
        f_text.text = "";
        w_text.text = "";
        switch (current)
        {
            case state.Agents:
            {
                    a_text.text = ""+nbAgents;
                    a_text.transform.position = lastA;
                    Debug.Log("Vieux point : " + a_text.transform.position);
                    break;
            }

            case state.Food:
            {
                    f_text.text = "" + nbFood;
                    f_text.transform.position = lastF;

                    break;
            }

            case state.Water:
            {
                    w_text.text = "" + nbWater;
                    w_text.transform.position = lastW;

                    break;
            }
            case state.All:
            {
                    a_text.text = "" + nbAgents;
                    f_text.text = "" + nbFood;
                    w_text.text = "" + nbWater;

                    a_text.transform.position = lastA;
                    f_text.transform.position = lastF;
                    w_text.transform.position = lastW;

                    break;
            }
        }
    }
}