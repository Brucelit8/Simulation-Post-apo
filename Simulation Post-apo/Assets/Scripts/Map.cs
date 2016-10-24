using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public int size = 10;
    public float ratio = 0.1f;
    public float offset = 0 ;
    private int[,] map;
    public GameObject wall;
    public GameObject ground;

    void Start()
    {
        GenerateMap();
        Load();
    }

    void GenerateMap()
    {
        map = new int[size,size];

        //Initialisation
        for (int i = 0; i<size; i++)
        {
            for(int j = 0; j<size; j++)
            {
                if (i == 0 || j == 0 || i == size-1 || j == size-1)
                {
                    map[i,j] = 1;
                }
                else
                {
                    map[i,j] = 0;
                }
            }
        }

    }

    void Load()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (map[i,j] == 0)
                {
                    Instantiate(ground, new Vector3((float)i*ratio + offset, 0, (float)j*ratio + offset), Quaternion.identity);
                }
                else if(map[i,j] == 1)
                {
                    Instantiate(wall, new Vector3((float)i*ratio + offset, 0, (float)j*ratio + offset), Quaternion.identity);
                }
            }
        }
    }

}
