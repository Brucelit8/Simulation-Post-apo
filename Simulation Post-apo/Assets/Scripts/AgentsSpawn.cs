using UnityEngine;
using System.Collections;

public class AgentsSpawn : MonoBehaviour {

    private int remainingSurvivors ;
    private int mapSize;
    private int[,] map;
    private float scaleChange = 0.7f;

    public GameObject survivor;
    public GameObject MapGameObject;

	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void spawningSurvivors()
    {
        if (remainingSurvivors != 0)
        {
            int x = Random.Range(0, MapGameObject.GetComponent<Map>().getSize());
            int y = Random.Range(0, MapGameObject.GetComponent<Map>().getSize());
            int[,] map = MapGameObject.GetComponent<Map>().getMap();

            if (map[x,y] == 0)
            {
                GameObject g = (GameObject)Instantiate(survivor, new Vector3(x, 0.1f, y), Quaternion.identity);
                g.transform.localScale -= new Vector3(scaleChange, scaleChange, scaleChange);

                // Player layer
                g.layer = 8;

                SurvivorBasicState sb = g.AddComponent(typeof(SurvivorBasicState)) as SurvivorBasicState;
                //BoxCollider bc = g.AddComponent<BoxCollider>();
                Rigidbody rb = g.AddComponent<Rigidbody>();
                g.tag = "Agent";

                g.GetComponent<SurvivorBasicState>().setID(remainingSurvivors);

                g.transform.SetParent(gameObject.transform);
                remainingSurvivors--;
                spawningSurvivors();
            }

            else
            {
                spawningSurvivors();
            }
        }

    }

    public void setNumberAgents(int i)
    {
        remainingSurvivors = i;
    }
}
