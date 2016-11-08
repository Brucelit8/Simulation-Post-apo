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
        items.Add("Wood", 0);
        items.Add("Stone", 0);
        items.Add("Steel", 0);
        items.Add("Electronics", 0);
        items.Add("Cloth", 0);
        items.Add("Alcohol", 0);
        items.Add("Bandage", 0);
        items.Add("Match", 0);
        items.Add("Campfire", 0);
        items.Add("Radio", 0);
        items.Add("Bed", 0);
    }
}
