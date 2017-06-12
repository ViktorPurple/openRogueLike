using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public int x = 0;
    public int y = 0;
    public int h = 0;
    public int g = 0;
    public int f = 0;
    public Node parent;

    public Node(int x, int y, int h, int g, Node parent)
    {
        this.x = x;
        this.y = y;
        this.h = h;
        this.g = g;
        this.f = g + h;
        this.parent = parent;
    }
    //**** This is more officient way to use setters getters. maybe later.
    //public int getF()
    //{
    //    return f;
    //}

    public bool CompareTo(Node nodeToCompare)
    {
        if (this.x == nodeToCompare.x && this.y == nodeToCompare.y)
        {
            if (this.f > nodeToCompare.f) { return false; }
            if (this.f == nodeToCompare.f)
            {
                if (this.g == nodeToCompare.g && this.h == nodeToCompare.h) { return false; }
                if (this.g > nodeToCompare.g) { return false; }
            }
            return true;
        }
        return false;
    }
    //print info about node. debuging purpose
    public string toString()
    {
        return x + " " + y + " " + h + " " + g + " " + f;
    }





}
