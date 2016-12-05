using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateCurves : MonoBehaviour {

    public Transform T1, T2;

    public LineRenderer LR1, LR2, LR3;
    float timeCurrent;
    public float timeLimit = 0.2f;
    public Transform origin;
    private float lastYA, lastYW, lastYF;
    public float xScaleA, yScaleA, xScaleF, yScaleF, xScaleW, yScaleW;
    public int nbEA, nbEF, nbEW;
    private float yUp, yDown;
    int nbAgents=0;
    int nbFood=0;
    int nbWater=0;
    bool fullA = false, fullF = false, fullW = false;
    int[] valuesA, valuesF, valuesW;
    int v_size;
    public enum state { Agents, Food, Water, All, None};
    protected state current ;
    public TextMesh a_text, f_text, w_text;
    // Use this for initialization

    void Start ()
    {
        nbAgents = GameObject.Find("AgentManager").transform.childCount;
        v_size = 10;
        valuesA = new int[v_size];
        valuesF = new int[v_size];
        valuesW = new int[v_size];

        LR1.SetVertexCount(0);
        LR2.SetVertexCount(0);
        LR3.SetVertexCount(0);

        LR1.SetColors(Color.green, Color.green);
        LR2.SetColors(Color.red, Color.red);
        LR3.SetColors(Color.blue, Color.blue);

        LR1.enabled = false;
        LR2.enabled = false;
        LR3.enabled = false;

        timeCurrent = Time.fixedTime;

        for(int i=0; i<v_size;i++)
        {
            valuesA[i] = -1;
            valuesF[i] = -1;
            valuesW[i] = -1;
        }

        nbAgents = countAgents();
        nbWater = countWater();
        nbFood = countFood();

        yUp = T1.position.y;
        yDown = T2.position.y;

        current = state.None;
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
                        if(!LR1.enabled)
                        {
                            LR1.enabled = true;
                        }
                        if (LR2.enabled)
                        {
                            LR2.enabled = false;
                        }
                        if (LR3.enabled)
                        {
                            LR3.enabled = false;
                        }
                        nbAgents = countAgents();

                        if(fullA)
                        {
                            if(lastYA > yUp)
                            {
                                yScaleA -= 0.1f;
                            }

                            if (lastYA < yDown)
                            {
                                yScaleA += 0.1f;
                            }
                        }

                        draw(false);
                        updateText();

                        break;
                }
                case state.Food:
                {
                        if (LR1.enabled)
                        {
                            LR1.enabled = false;
                        }
                        if (!LR2.enabled)
                        {
                            LR2.enabled = true;
                        }
                        if (LR3.enabled)
                        {
                            LR3.enabled = false;
                        }

                        nbFood = countFood();

                        if (fullF)
                        {
                            if (lastYF > yUp)
                            {
                                yScaleF -= 0.01f;
                            }

                            if (lastYF < yDown)
                            {
                                yScaleF += 0.01f;
                            }
                        }

                        draw(false);
                        updateText();
                        break;
                }
                case state.Water:
                {
                        if (LR1.enabled)
                        {
                            LR1.enabled = false;
                        }
                        if (LR2.enabled)
                        {
                            LR2.enabled = false;
                        }
                        if (!LR3.enabled)
                        {
                            LR3.enabled = true;
                        }

                        nbWater = countWater();

                        if (fullW)
                        {
                            if (lastYW > yUp)
                            {
                                yScaleW -= 0.01f;
                            }

                            if (lastYW < yDown)
                            {
                                yScaleW += 0.01f;
                            }
                        }

                        draw(false);
                        updateText();
                        break;
                }
                case state.All:
                {
                        if (!LR1.enabled)
                        {
                            LR1.enabled = true;
                        }
                        if (!LR2.enabled)
                        {
                            LR2.enabled = true;
                        }
                        if (!LR3.enabled)
                        {
                            LR3.enabled = true;
                        }

                        if (fullA)
                        {
                            if (lastYA > yUp)
                            {
                                yScaleA -= 0.1f;
                            }

                            if (lastYA < yDown)
                            {
                                yScaleA += 0.1f;
                            }
                        }

                        if (fullF)
                        {
                            if (lastYF > yUp)
                            {
                                yScaleF -= 0.01f;
                            }

                            if (lastYF < yDown)
                            {
                                yScaleF += 0.01f;
                            }
                        }

                        if (fullW)
                        {
                            if (lastYW > yUp)
                            {
                                yScaleW -= 0.01f;
                            }

                            if (lastYW < yDown)
                            {
                                yScaleW += 0.01f;
                            }
                        }

                        nbWater = countWater();
                        nbFood = countFood();
                        nbAgents = countAgents();

                        draw(true);
                        updateText();
                        break;
                }
                case state.None:
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

        while(i<v_size && valuesA[i] != -1 && !fullA)
        {
            i++;
        }

        if(i == v_size)
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
            nbEA = v_size;
            LR1.SetVertexCount(v_size);

        }
        else
        {
            valuesA[i] = nbAgents;
            LR1.SetVertexCount(i);
            nbEA = i;
        }
        ////////////////////////////////////////////////////////2
        i = 0;

        while (i < v_size && valuesF[i] != -1 && !fullF)
        {
            i++;
        }

        if (i == v_size)
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
            LR2.SetVertexCount(v_size);
            nbEF = v_size;

        }
        else
        {
            valuesF[i] = nbFood;
            LR2.SetVertexCount(i);
            nbEF = i;

        }
        /////////////////////////////////////////////////////////3
        i = 0;

        while (i < v_size && valuesW[i] != -1 && !fullW)
        {
            i++;
        }

        if (i == v_size)
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
            LR3.SetVertexCount(v_size);
            nbEW = v_size;

        }
        else
        {
            valuesW[i] = nbWater;
            LR3.SetVertexCount(i);
            nbEW = i;

        }

        switch (current)
            {
                case state.All:
                {
                    for (int j = 0; j < nbEA; j++)
                    {
                        lastYA = origin.position.y + yScaleA * valuesA[j];
                        LR1.SetPosition(j, new Vector3(origin.position.x + j * xScaleA, origin.position.y + yScaleA * valuesA[j], origin.position.z));
                    }
                    for (int j = 0; j < nbEF; j++)
                    {
                        lastYF = origin.position.y + yScaleF * valuesF[j];
                        LR2.SetPosition(j, new Vector3(origin.position.x + j * xScaleF, origin.position.y + yScaleF * valuesF[j], origin.position.z));
                    }
                    for (int j = 0; j < nbEW; j++)
                    {
                        lastYW = origin.position.y + yScaleW * valuesW[j];
                        LR3.SetPosition(j, new Vector3(origin.position.x + j * xScaleW, origin.position.y + yScaleW * valuesW[j], origin.position.z));
                    }
                    break;
                }

                case state.Agents:
                {
                    for (int j = 0; j < nbEA; j++)
                    {
                        lastYA = origin.position.y + yScaleA * valuesA[j];
                        LR1.SetPosition(j, new Vector3(origin.position.x + j * xScaleA, origin.position.y + yScaleA * valuesA[j], origin.position.z));
                    }
                    break;
                }

                case state.Food:
                {
                    for (int j = 0; j < nbEF; j++)
                    {
                        lastYF = origin.position.y + yScaleF * valuesF[j];
                        LR2.SetPosition(j, new Vector3(origin.position.x + j * xScaleF, origin.position.y + yScaleF * valuesF[j], origin.position.z));
                    }
                    break;
                }

             case state.Water:
                {
                    for (int j = 0; j < nbEW; j++)
                    {
                        lastYW = origin.position.y + yScaleW * valuesW[j];
                        LR3.SetPosition(j, new Vector3(origin.position.x + j * xScaleW, origin.position.y + yScaleW * valuesW[j], origin.position.z));
                    }
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
                    break;
            }

            case state.Food:
            {
                    f_text.text = "" + nbFood;
                    break;
            }

            case state.Water:
            {
                    w_text.text = "" + nbWater;
                    break;
            }
            case state.All:
            {
                    a_text.text = "" + nbAgents;
                    f_text.text = "" + nbFood;
                    w_text.text = "" + nbWater;
                    break;
            }
        }
    }
}