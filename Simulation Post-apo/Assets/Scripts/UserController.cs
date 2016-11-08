using UnityEngine;
using System.Collections;

public class UserController : MonoBehaviour {

    GameObject Selected;
    private bool firstSelect = false;
    private string typeSelect;
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
                if(firstSelect)
                {
                    (Selected.GetComponent(typeSelect) as Building).changed = true;
                    (Selected.GetComponent(typeSelect) as Building).selected = false;
                }

                if (hit.collider.tag == "House")
                {

                    hit.transform.gameObject.GetComponent<House>().Details();
                    Selected = hit.transform.gameObject;
                    firstSelect = true;
                    Selected.GetComponent<House>().selected = true;
                    Selected.GetComponent<House>().changed = true;
                    typeSelect = "House";

                }
                else if (hit.collider.tag == "Hospital")
                {
                    hit.transform.gameObject.GetComponent<Hospital>().Details();
                    Selected = hit.transform.gameObject;
                    firstSelect = true;
                    Selected.GetComponent<Hospital>().selected = true;
                    Selected.GetComponent<Hospital>().changed = true;
                    typeSelect = "Hospital";

                }
                else if (hit.collider.tag == "Supermarket")
                {
                    hit.transform.gameObject.GetComponent<Supermarket>().Details();
                    Selected = hit.transform.gameObject;
                    firstSelect = true;
                    Selected.GetComponent<Supermarket>().selected = true;
                    Selected.GetComponent<Supermarket>().changed = true;
                    typeSelect = "Supermarket";

                }
                else if (hit.collider.tag == "Remain")
                {
                    hit.transform.gameObject.GetComponent<Remains>().Details();
                    Selected = hit.transform.gameObject;
                    firstSelect = true;
                    Selected.GetComponent<Remains>().selected = true;
                    Selected.GetComponent<Remains>().changed = true;
                    typeSelect = "Remains";

                }

            }
        }
    }
}
