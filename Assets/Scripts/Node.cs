using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public int x = 0;
    public int y = 0;
    public int h = 0;
    public int g = 0;
    public int f = 0;


    public Node(int x, int y, int h, int g)
    {
        this.x = x;
        this.y = y;
        this.h = h;
        this.g = g;
        this.f = g + h;
    }

    public int getF()
    {
        return f; 
    }



    public int CompareTo(Node nodeToCompare)
    {
        if (this.x == nodeToCompare.x && this.y == nodeToCompare.y)
        {
            int compare = f.CompareTo(nodeToCompare.f);

            if (compare == 0)
            {
                compare = h.CompareTo(nodeToCompare.h);
            }
            return compare;
        }
        else
        {
            return 0;
        }
    }

    public string toString()
    {
        return x + " " + y + " " + h + " " + g + " " + f;
    }

    
}
