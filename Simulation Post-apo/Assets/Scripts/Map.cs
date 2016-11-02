using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public int size = 10;   //Length of the square edge of the map
    public float ratio = 1f;    //Modify the distance between prefabs

    public int nbHospitals = 1;
    public int nbSupermarkets = 2;
    public int nbHouses = 10;
    public int nbRemains = 50;
    
    private int[,] map;     //Double Array of int to represent the map as a matrix
    /* 
        0 for the ground
        1 for the borders
        2 for the remains
        3 for the houses
        4 for hospitals
        5 for supermarket
    */

    public Transform mapmanager;
    public GameObject wall;
    public GameObject ground;
    public GameObject remain;
    public GameObject hospital;
    public GameObject house;
    public GameObject supermarket;

    void Start()
    {
        GenerateMap();
        Load();
    }

    void GenerateMap()
    {
        map = new int[size,size];

        //Initialisation and borders creation
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

        SpawningHospitals(nbHospitals);
        SpawningSupermarkets(nbSupermarkets);
        SpawningHouses(nbHouses);
        SpawningRemains(nbRemains);
    }

    void Load()     // This fonction just load prefabs according to map values
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (map[i, j] == 0)
                {
                    GameObject go = (GameObject)Instantiate(ground, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);
                }
                else if (map[i, j] == 1)
                {
                    GameObject go = (GameObject)Instantiate(wall, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);
                }
                else if (map[i, j] == 2)
                {
                    GameObject go = (GameObject)Instantiate(remain, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);
                }
                else if (map[i, j] == 3)
                {
                    GameObject go = (GameObject)Instantiate(house, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);
                }
                else if (map[i, j] == 4)
                {
                    GameObject go = (GameObject)Instantiate(hospital, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);
                }
                else if (map[i, j] == 5)
                {
                    GameObject go = (GameObject)Instantiate(supermarket, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);
                }
            }
        }
    }

    void SpawningHospitals(int remainingHospitals)
    {
        if (remainingHospitals != 0)
        {
            int n = remainingHospitals;
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);

            if (map[x, y] == 0 && !AnyNeighbors(x, y, 4, 5))
            {
                map[x, y] = 4;
                SpawningHospitals(n - 1);
            }
            else
            {
                SpawningHospitals(n);
            }
        }
    }

    void SpawningSupermarkets(int remainingSupermarkets)
    {
        if (remainingSupermarkets != 0)
        {
            int n = remainingSupermarkets;
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);

            if (map[x, y] == 0 && !AnyNeighbors(x, y, 5, 5))
            {
                map[x, y] = 5;
                SpawningSupermarkets(n - 1);
            }
            else
            {
                SpawningSupermarkets(n);
            }
        }
    }

    void SpawningRemains(int nbR)
    {
        SR1(nbR / 10);
        SR2(nbR * 9 / 10);
    }

    void SR1(int nbR)
    {
        if(nbR!=0)
        {
            int r = nbR;
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);

            if (map[x, y] == 0 && !AnyNeighbors(x, y, 2, size / 10))
            {
                map[x, y] = 2;
                SR1(r - 1);
            }
            else
            {
                SR1(r);
            }
        }
    }

    void SR2(int nbR)
    {
        if (nbR != 0)
        {
            int r = nbR;
            int x = Random.Range(0, size);
            int y = Random.Range(0, size);

            if (map[x, y] == 0 && AnyNeighbors(x, y, 2, 2))
            {
                map[x, y] = 2;
                SR2(r - 1);
            }
            else
            {
                SR2(r);
            }
        }
    }

    void SpawningHouses(int remainingHouses)
    {
        SH1(remainingHouses / 10);
        SH2(remainingHouses * 9 / 10);
    }

    void SH1(int remainingHouses)
    {
            if (remainingHouses != 0)
            {
                int n = remainingHouses;
                int x = Random.Range(0, size);
                int y = Random.Range(0, size);

                if (map[x, y] == 0 && !AnyNeighbors(x, y, 3, size/10))
                {
                    map[x, y] = 3;
                    SH1(n - 1);
                }
                else
                {
                    SH1(n);
                }
            }
    }

    void SH2(int remainingHouses)
    {
            if (remainingHouses != 0)
            {
                int n = remainingHouses;
                int x = Random.Range(0, size);
                int y = Random.Range(0, size);

                if (map[x, y] == 0 && !AnyNeighbors(x, y, 3, 1) && AnyNeighbors(x, y, 3, size/5))
                {
                    map[x, y] = 3;
                    SH2(n - 1);
                }
                else
                {
                    SH2(n);
                }
            }
    }



    bool AnyNeighbors(int x, int y, int neighbor, int depth)
    {
        for(int i = x - depth; i < x + depth + 1; i++)
        {
            for (int j = y - depth; j < y + depth + 1; j++)
            {
                if((x!=i || y!=j) && (i>0 && i<size-1 && j>0 && j<size-1))
                {
                    if((map[i, j] == neighbor))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }
}
