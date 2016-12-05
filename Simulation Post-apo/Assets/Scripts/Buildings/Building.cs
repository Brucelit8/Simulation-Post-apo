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
    protected int scrap;

    protected float coefR = 1.0f;

    protected int X, Y;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

	}

    public Building()
    {

    }

    public virtual void Details() { }

    public int getSafety() { return safety; }
    public void setSafety(int s)
    {
        safety = s;
    }

    public int getFood() { return food; }
    public void setFood(int f)
    {
        food = f;
    }

    public int getWater() { return water; }
    public void setWater(int w)
    {
        water = w;
    }

    public int getBandage() { return bandage; }
    public void setBandage(int b)
    {
        bandage = b;
    }

    public int getScrap() { return scrap; }
    public void setScrap(int s)
    {
        scrap = s;
    }

    public void setXY(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int getX()
    {
        return X;
    }
    public int getY()
    {
        return Y;
    }

    public virtual void DeselectAllOthers()
    {
 
    }
}
