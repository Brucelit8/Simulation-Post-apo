using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Building : MonoBehaviour{

    public bool selected = false;
    public bool changed = false;

    protected int safety;
    protected int bed;
    protected int water;
    protected int food;
    protected int bandage;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

	}

    public Building() {
    }

    public virtual void Details() { }

    public int getFood() { return food; }
    public void setFood(int f)
    {
        food = f;
        Details();
    }

    public int getWater() { return water; }
    public void setWater(int w)
    {
        water = w;
        Details();
    }

    public int getBandage() { return bandage; }
    public void setBandage(int b)
    {
        bandage = b;
        Details();
    }

}
