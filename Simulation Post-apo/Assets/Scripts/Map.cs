using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {

    public int size = 10;   //Length of the square edge of the map
    public float ratio = 1f;    //Modify the distance between prefabs

    public int nbHospitals = 1;
    public int nbSupermarkets = 2;
    public int nbHouses = 10;
    public int nbRemains = 50;
    public int nbSurvivors = 20;

    private int[,] map;     //Double Array of int to represent the map as a matrix
    /* 
        0 for the ground
        1 for the borders
        2 for the remains
        3 for the houses
        4 for hospitals
        5 for supermarket
    */

    public int[,] getMap() { return map; }

    public Transform mapmanager;
    public GameObject wall;
    public GameObject ground;
    public GameObject remain;
    public GameObject hospital;
    public GameObject house;
    public GameObject supermarket;
    public GameObject AgentManager;

    void Start()
    {
        GenerateMap();
        Load();
        AgentManager.GetComponent<AgentsSpawn>().spawningSurvivors();
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

                    // building layer
                    go.layer = 9;

                    createBuildingNodes(i, j, go);
                }
                else if (map[i, j] == 4)
                {
                    GameObject go = (GameObject)Instantiate(hospital, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);

                    // building layer
                    go.layer = 9;

                    createBuildingNodes(i, j, go);
                }
                else if (map[i, j] == 5)
                {
                    GameObject go = (GameObject)Instantiate(supermarket, new Vector3((float)i * ratio, 0, (float)j * ratio), Quaternion.identity);
                    go.transform.SetParent(mapmanager);

                    // building layer
                    go.layer = 9;

                    createBuildingNodes(i, j, go);
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

            if (map[x, y] == 0 && !AnyNeighbors(x, y, 4, size/5) && x > size/4 && y > size/4 && x < size - size/4 && y < size - size/4)
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

            if (map[x, y] == 0 && !AnyNeighbors(x, y, 5, size/5) && x > size /4 && y > size /4 && x < size - size /4 && y < size - size /4)
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

    void createBuildingNodes(int i, int j, GameObject p)
    {
        if (i - 1 != 0)
        {
            if (i + 1 != size - 1)
            {
                if (j - 1 != 0)
                {
                    //Case of a building not on an edge
                    if (j + 1 != size - 1)
                    {
                        GameObject n1 = new GameObject();
                        createNode(n1, p, "N1", 1, 1);

                        GameObject n2 = new GameObject();
                        createNode(n2, p, "N2", 1, -1);

                        GameObject n3 = new GameObject();
                        createNode(n3, p, "N3", -1, 1);

                        GameObject n4 = new GameObject();
                        createNode(n4, p, "N4", -1, -1);
                    }
                    //Case of a building on the bottom edge of the map but not in a corner
                    else
                    {
                        GameObject n1 = new GameObject();
                        createNode(n1, p, "N1", 1, -1);

                        GameObject n2 = new GameObject();
                        createNode(n2, p, "N2", -1, -1);
                    }
                }

                //Case of a building on the upper edge of the map but not in a corner
                else
                {
                    GameObject n1 = new GameObject();
                    createNode(n1, p, "N1", 1, 1);

                    GameObject n2 = new GameObject();
                    createNode(n2, p, "N2", -1, 1);
                }
            }

            //Case of a building on the right edge of the map
            else
            {
                //Case of a building in the upper right corner
                if (j - 1 == 0)
                {
                    GameObject n1 = new GameObject();
                    createNode(n1, p, "N1", -1, 1);
                }

                //Case of a building on the right edge, not in a corner
                else if (j + 1 != size - 1)
                {
                    GameObject n1 = new GameObject();
                    createNode(n1, p, "N1", -1, 1);

                    GameObject n2 = new GameObject();
                    createNode(n2, p, "N2", -1, -1);
                }

                //Case of a building in the bottom right corner
                else
                {
                    GameObject n1 = new GameObject();
                    createNode(n1, p, "N1", -1, -1);
                }
            }
        }

        else
        {
            //Case of a building in the upper left corner
            if (j - 1 == 0)
            {
                GameObject n1 = new GameObject();
                createNode(n1, p, "N1", 1, 1);
            }

            //Case of a building on the left edge, not in a corner
            else if (j + 1 != size - 1)
            {
                GameObject n1 = new GameObject();
                createNode(n1, p, "N1", 1, 1);

                GameObject n2 = new GameObject();
                createNode(n1, p, "N2", 1, -1);
            }

            //Case of a building in the bottom left corner
            else
            {
                GameObject n1 = new GameObject();
                createNode(n1, p, "N1", 1, -1);
            }
        }
    }

    void createNode(GameObject n, GameObject parent, string name, int xParity, int zParity)
    {
        n.name = name;
        //n.gameObject.transform.parent = parent.gameObject.transform;
        n.gameObject.transform.SetParent(parent.transform);
        n.transform.position = new Vector3(parent.gameObject.transform.localPosition.x + xParity, parent.gameObject.transform.localPosition.y,
           parent.gameObject.transform.localPosition.z + zParity);
        n.AddComponent<BoxCollider>();
        n.GetComponent<BoxCollider>().isTrigger = true;
        n.GetComponent<BoxCollider>().size *= 0.2f;

        // Node layer
        n.layer = 10;

        n.tag = "Node";
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
