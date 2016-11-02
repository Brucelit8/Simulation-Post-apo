using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "House")
                {
                    hit.transform.gameObject.GetComponent<House>().Details();
                }
                else if (hit.collider.tag == "Hospital")
                {
                    hit.transform.gameObject.GetComponent<Hospital>().Details();
                }
                else if (hit.collider.tag == "Supermarket")
                {
                    hit.transform.gameObject.GetComponent<Supermarket>().Details();
                }
                else if (hit.collider.tag == "Remain")
                {
                    hit.transform.gameObject.GetComponent<Remains>().Details();
                }
            }
        }
    }
}
