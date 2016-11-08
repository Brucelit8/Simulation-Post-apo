using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {

    public Hashtable items;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Inventory()
    {
        items = new Hashtable();
        items.Add("Water", 0);
        items.Add("Food", 0);
        items.Add("Bandage", 0);
    }
}
