using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FollowButton : MonoBehaviour {

    public Text t;
    private bool disable = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}   

    public void Following()
    {
        if(disable)
        {
            t.text = "On";
            GameObject.Find("Main Camera").GetComponent<CameraController>().followCurrent = true;
            disable = false;
        }
        else
        {
            t.text = "Off";
            GameObject.Find("Main Camera").GetComponent<CameraController>().followCurrent = false;
            disable = true;
        }
    }
}