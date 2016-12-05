using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public float speed;
    public float zoom;

    public bool followCurrent = false;
    public Transform T = null;

    private float offsetX = 0.0f, offsetY = 5.0f, offsetZ = -1.80f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(followCurrent)
        {
            GameObject M = GameObject.Find("AgentManager");

            foreach (Transform child in M.transform)
            {
                GameObject G = child.gameObject;
                if (G.GetComponent<Statistics>().selected == true)
                {
                    T = G.transform;
                }
            }
            transform.position = T.position + new Vector3(offsetX,offsetY,offsetZ);
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0.0f)
            {
                MoveCamera(0);
            }
            if (Input.GetAxis("Horizontal") > 0.0f)
            {
                MoveCamera(1);
            }
            if (Input.GetAxis("Vertical") < 0.0f)
            {
                MoveCamera(2);
            }
            if (Input.GetAxis("Vertical") > 0.0f)
            {
                MoveCamera(3);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            {
                MoveCamera(4);
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            {
                MoveCamera(5);
            }
        }

    }

    void MoveCamera(int c)
    {
        switch (c)  
        {
            case 0 :
                transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
                break;
            case 1:
                transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                break;
            case 2:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);
                break;
            case 3:
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);
                break;
            case 4:
                transform.position = new Vector3(transform.position.x, transform.position.y + zoom, transform.position.z);
                break;
            case 5:
                transform.position = new Vector3(transform.position.x, transform.position.y - zoom, transform.position.z);
                break;

        }
    }
}
