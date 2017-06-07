using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour {

    //temp

    private GameObject go;
    List<Node> listOfNodes = new List<Node>();

    public PathFinder() { }

    public void PathFinderMain(Vector3 start, Vector3 end, GameObject pathArea)
    {
        // x, y, h, g,  f, parent


        Node startNode = new Node((int) start.x, (int) start.y, calculateHCost(start, end), 0);

        //spare Nodes
        addSpareNodes(end, startNode);

        Node current = nextNode(listOfNodes[0]);
        go = Instantiate(pathArea, new Vector2(current.x, current.y), Quaternion.identity);

        // (player.position.x - end.x != 0 || player.position.y - end.y != 0)
        //while (current.x - (int) end.x != 0 || current.y - (int)end.y != 0)
        for (int i = 0; i < 20; i ++)
        {
           // Debug.Log(current.x + " " + current.y + " in Process");
            current = nextNode(current);
            addSpareNodes(end, current);
            current = nextNode(current);
           go = Instantiate(pathArea, new Vector2(current.x, current.y), Quaternion.identity);
      }
        //Debug.Log(current.x + " " + current.y + " path found");
          
        
        
    }

    private int calculateHCost(Vector3 start, Vector3 end)
    {
        Vector2 spot = new Vector2(start.x, start.y);
        int h = 0;

        while (spot.x - end.x != 0 || spot.y - end.y != 0)
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
        //Debug.Log(h);
        return h;
    }

    private void addSpareNodes(Vector3 end, Node current)
    {
        Vector3 start = new Vector3(current.x, current.y, 0);
        //first line
        if (!compareAndChange(new Node(current.x - 1, current.y - 1, calculateHCost(start, end), 14)))
        { listOfNodes.Add(new Node(current.x - 1, current.y - 1, calculateHCost(start, end), 14)); }

        if (!compareAndChange(new Node(current.x, current.y - 1, calculateHCost(start, end), 10)))
        { listOfNodes.Add(new Node(current.x, current.y - 1, calculateHCost(start, end), 10)); }

         if (!compareAndChange(new Node(current.x + 1, current.y - 1, calculateHCost(start, end), 14)))
         { listOfNodes.Add(new Node(current.x + 1, current.y - 1, calculateHCost(start, end), 14)); }
        //second line
         if (!compareAndChange(new Node(current.x - 1, current.y, calculateHCost(start, end), 10)))
        { listOfNodes.Add(new Node(current.x - 1, current.y, calculateHCost(start, end), 10)); }       
         if (!compareAndChange(new Node(current.x + 1, current.y, calculateHCost(start, end), 10)))
                { listOfNodes.Add(new Node(current.x + 1, current.y, calculateHCost(start, end), 10)); }
        //3rd line
         if (!compareAndChange(new Node(current.x - 1, current.y + 1, calculateHCost(start, end), 14)))
        { listOfNodes.Add(new Node(current.x - 1, current.y + 1, calculateHCost(start, end), 10)); }
        if (!compareAndChange(new Node(current.x, current.y + 1, calculateHCost(start, end), 10)))
        { listOfNodes.Add(new Node(current.x, current.y + 1, calculateHCost(start, end), 10)); }
        if (!compareAndChange(new Node(current.x + 1, current.y + 1, calculateHCost(start, end), 14)))
        { listOfNodes.Add(new Node(current.x + 1, current.y + 1, calculateHCost(start, end), 10)); }
    }

    private bool compareAndChange(Node current)
    {
        foreach (Node node in listOfNodes)
        {
            if (current.CompareTo(node) < 0)
            {
                node.h = current.h;
                node.g = current.g;
                node.f = current.f;
                return true;
            }
        }
        return false;
    }

    private Node nextNode(Node current)
    {
        Node minNode = current;
        foreach (Node node in listOfNodes)
        {
            if (minNode.f > node.f)
            {
                Debug.Log("node changed from " + minNode.toString() + " to " + node.toString());
                minNode = node;
                //Debug.Log("node changed");
                
            }
            else
            {
                if (minNode.f == node.f)
                {
                    minNode = minNode.h > node.h ? node : minNode;
                }
            }
        }
        return minNode;
    }
}
