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
                    if(!(Selected == null))
                    {
                        if (Selected.GetComponent<Collider>().tag == "Agent")
                        {
                            Selected.GetComponent<Statistics>().changed = true;
                            Selected.GetComponent<Statistics>().selected = false;
                        }
                        else
                        {
                            (Selected.GetComponent(typeSelect) as Building).changed = true;
                            (Selected.GetComponent(typeSelect) as Building).selected = false;
                        }
                    }
                }

                if(hit.collider.tag == "House" || hit.collider.tag == "Hospital" || hit.collider.tag == "Supermarket" || hit.collider.tag == "Remains")
                {
                    string buildingType = hit.collider.tag;
                    (hit.transform.gameObject.GetComponent(buildingType) as Building).Details();
                    Selected = hit.transform.gameObject;
                    firstSelect = true;
                    (Selected.GetComponent(buildingType) as Building).changed = true;
                    (Selected.GetComponent(buildingType) as Building).selected = true;
                    typeSelect = buildingType;
                }
                else if(hit.collider.tag == "Agent")
                {
                    Selected = hit.transform.gameObject;
                    Selected.GetComponent<Statistics>().changed = true;
                    Selected.GetComponent<Statistics>().selected = true;

                }
            }
        }
    }
}
