using UnityEngine;
using System.Collections;

public class AgentsSpawn : MonoBehaviour {

    public int remainingSurvivors = 10;
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
        if(remainingSurvivors != 0)
        {
            int x = Random.Range(0, MapGameObject.GetComponent<Map>().size);
            int y = Random.Range(0, MapGameObject.GetComponent<Map>().size);
            int[,] map = MapGameObject.GetComponent<Map>().getMap();

            if (map[x,y] == 0)
            {
                GameObject g = (GameObject)Instantiate(survivor, new Vector3(x, 0.2f, y), Quaternion.identity);
                g.transform.localScale -= new Vector3(scaleChange, scaleChange, scaleChange);

                SurvivorBasicState sb = g.AddComponent(typeof(SurvivorBasicState)) as SurvivorBasicState;
                Rigidbody rb = g.AddComponent<Rigidbody>();
                rb.mass = 90;
                //BoxCollider bc = g.AddComponent<BoxCollider>();
                //bc.center = new Vector3(g.transform.localPosition.x, g.transform.localPosition.y, g.transform.localPosition.z);

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
}
