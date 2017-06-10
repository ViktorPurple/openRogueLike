﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    //temp

    private GameObject go;
    List<Node> listOfNodes = new List<Node>();
    List<Node> blockNodes = new List<Node>();

    List<Node> notCleared = new List<Node>();
    List<Node> cleared = new List<Node>();
    //array of walls
    private GameObject[] walls;

    private GameObject p;

    public PathFinder() { }

    public void PathFinderMain(Vector3 start, Vector3 end, GameObject pathArea)
    {
        // x, y, h, g,  f, parent
        p = pathArea;        

        //collect walls
        blockNodes = gameObject.AddComponent<BlockingLayerLoading>().getWalls();  


        Node startNode = new Node((int)start.x, (int)start.y, calculateHCost(start, end), 0, null);
        blockNodes.Add(startNode);
        listOfNodes.Add(startNode);
        //spare Nodes

        addSpareNodes(end, startNode);

        Node current = nextNode(listOfNodes[0]);
        
        while (current.x != (int)end.x || current.y != (int)end.y)
        //for (int i=0; i < 1000; i++)
        {
            cleared.Add(current);
            addSpareNodes(end, current);
            if (current == nextNode(current)) { Debug.Log(current.toString() + " " + nextNode(current).toString()); break; }
            current = nextNode(current);
            
           
        }

        foreach (Node node in listOfNodes)
        {
            Debug.Log(node.toString());
        }
    }


    // Calculating HCost 
    private int calculateHCost(Vector3 start, Vector3 end)
    {
        Vector2 spot = new Vector2(start.x, start.y);
        int h = 0;

        while (spot.x != end.x || spot.y != end.y)
        {
            float x = 0;
            x = spot.x - end.x > 0 ? -1 : 1;
            x = spot.x - end.x == 0 ? 0 : x;

            float y = 0;
            y = spot.y - end.y > 0 ? -1 : 1;
            y = spot.y - end.y == 0 ? 0 : y;

            h += x != 0 && y != 0 ? 14 : 10;
            spot = new Vector2(spot.x + x, spot.y + y);

        }
        return h;
    }


    //add all side Nodes
    private void addSpareNodes(Vector3 end, Node current)
    {
        Vector3 start = new Vector3(current.x, current.y, 0);
        //first line
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (!(i == 0 && j == 0))
                {
                    int g = Mathf.Abs(i) != Mathf.Abs(j) ? 14 : 10;
                    //Debug.Log("not hitted");
                    compareAndChange(new Node(current.x + i, current.y + j, calculateHCost(new Vector2(current.x + i, current.y + j), end), g, current));
                }
            }
        }
    }

    // If same change stats
    private void compareAndChange(Node current)
    {
        int count = listOfNodes.Count;

        for (int i = 0; i < count; i++)
        {  
            
            if (current.CompareTo(listOfNodes[i]))
            {
                listOfNodes[i].h = current.h;
                listOfNodes[i].g = current.g;
                listOfNodes[i].f = current.f;
                listOfNodes[i].parent = current.parent;

                Debug.Log("changed");
                return;
            }
        }


        foreach (Node node in blockNodes)
        {
            if (current.x == node.x && current.y == node.y)
            {
                Debug.Log(" not added " + node.toString());
                return;
            }
        }

        go = Instantiate(p, new Vector2(current.x, current.y), Quaternion.identity);
        listOfNodes.Add(current);
        Debug.Log("added " + current.toString());
    }
    
    

    public Node nextNode(Node current)
    {
        Node minNode = listOfNodes[1];
        foreach (Node node in listOfNodes)
        {
            if (minNode.f > node.f)
            {
                if (!checkIfClear(node))
                {
                    minNode = node; 
                }
            }
        }
        return minNode;
    }

    private bool checkIfClear(Node current)
    {
        foreach (Node node in cleared)
        {
            if (node == current)
            {
                return true;
            }
        }
        return false;
    }
}
