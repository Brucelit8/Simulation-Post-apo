using UnityEngine;
using System.Collections;

public class GroundPositions : MonoBehaviour {

    private int X, Y;

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

}
