using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{

    private Node BIGEST_NODE = new Node(10000, 10000, 10000, 10000, null);
    //tem    
    List<Node> listOfNodes = new List<Node>();
    List<Node> blockNodes = new List<Node>();

    List<Node> cleared = new List<Node>();
    //array of walls
    private GameObject[] walls;


    public PathFinder() { }

    public List<Vector3> path = new List<Vector3>();


    public bool PathFinderMain(Vector3 start, Vector3 end, GameObject pathArea)
    {
        // x, y, h, g,  f, parent

       // path = new List<Vector3>();

        //collect walls
        blockNodes = gameObject.AddComponent<BlockingLayerLoading>().getWalls();  


        Node startNode = new Node((int)start.x, (int)start.y, calculateHCost(start, end), 0, null);


        blockNodes.Add(startNode);
        listOfNodes.Add(startNode);
        cleared.Add(startNode);
        //spare Nodes
        addSpareNodes(end, startNode);

        Node current = nextNode(listOfNodes[0]);

        int countSteps = 0;
        while (current.x != (int)end.x || current.y != (int)end.y)
        //for (int i=0; i < 1000; i++)
        {
            cleared.Add(current);
            addSpareNodes(end, current);
            if (current == nextNode(current) || countSteps > 200) {
                Debug.Log(" path is too long of not found" );
                return false; }
            current = nextNode(current);
            countSteps++;
        }

        drawPath(current);

        //Debug.Log(path.Count);
        //foreach (Node i in listOfNodes)
        //{
        //    Debug.Log(i.toString() + " parent " );
        //}

        //Destroy(GetComponent<BlockingLayerLoading>());
        return true;
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
                    int g = Mathf.Abs(i) != Mathf.Abs(j) ? 10 : 14;
                    //Debug.Log("not hitted");
                    compareAndChange(new Node(current.x + i, current.y + j, calculateHCost(new Vector2(current.x + i, current.y + j), end), current.g + g, current));
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
                //listOfNodes[i].h = current.h;
                if (!isCleared(current))
                {
                    listOfNodes[i].g = current.g;
                    listOfNodes[i].f = listOfNodes[i].h + listOfNodes[i].g;
                    listOfNodes[i].parent = current.parent;
                }

                return;
            }
        }


        foreach (Node node in blockNodes)
        {
            if (current.x == node.x && current.y == node.y)
            {
                return;
            }
        }        
        listOfNodes.Add(current);
    }
    
    

    public Node nextNode(Node current)
    {
        Node minNode = BIGEST_NODE;
        foreach (Node node in listOfNodes)
        {
            if (minNode.f >= node.f)
            {
                if (!isCleared(node))
                {
                    minNode = node; 
                }
            }
        }
        return minNode;
    }

    private bool isCleared(Node current)
    {
        foreach (Node node in cleared)
        {
            if (node.x == current.x && node.y == current.y)
            {
                return true;
            }
        }
        return false;
    }

    private void drawPath(Node current)
    {
        while (current.x != listOfNodes[0].x || current.y != listOfNodes[0].y)
        //for (int i=0; i < 20; i++)
        {
            path.Add(new Vector3(current.x, current.y, 0));
            current = current.parent;
        }
    }
}
