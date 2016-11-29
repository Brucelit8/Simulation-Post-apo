using UnityEngine;
using System.Collections;

public class CreateCurves : MonoBehaviour {

    public LineRenderer LR;
    float timeCurrent;
    public float timeLimit = 0.2f;
    public Transform origin;
    public float xScale = 10f;
    public float yScale = 10f;
    int nbAgents;
    bool full = false;
    int[] values;
    int v_size;

	// Use this for initialization
	void Start ()
    {
        nbAgents = GameObject.Find("AgentManager").transform.childCount;
        v_size = 10;
        values = new int[v_size];
        LR.SetVertexCount(v_size);
        timeCurrent = Time.fixedTime;

        for(int i=0; i<v_size;i++)
        {
            values[i] = -1;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Time.fixedTime - timeCurrent >= timeLimit)
        {
            nbAgents = GameObject.Find("AgentManager").transform.childCount;
            draw();
            timeCurrent = Time.fixedTime;
        }

	}

    void draw()
    {
        int i = 0;

        while(i<v_size && values[i] != -1 && !full)
        {
            i++;
        }

        if(i == v_size-1)
        {
            full = true;
        }

        if(full)
        {
            for(int j=1; j<v_size;j++)
            {
                values[j - 1] = values[j];
            }

            values[v_size - 1] = nbAgents;

        }
        else
        {
            values[i] = nbAgents;
        }

        for(int j=0; j<v_size; j++)
        {
            LR.SetPosition(j, new Vector3(origin.position.x + j * xScale, origin.position.y + yScale * values[j], origin.position.z));
        }

    }



}
