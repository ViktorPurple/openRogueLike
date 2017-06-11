using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingLayerLoading : MonoBehaviour {

    private GameObject[] walls;
    private List<Node> wallsNodes = new List<Node>();
    

    public  List<Node> getWalls()
    {
        //find all walls with tag
        walls = GameObject.FindGameObjectsWithTag("blockingLayer");
        //create nodes for it
        foreach (GameObject wall in walls)
        {
            wallsNodes.Add(new Node((int)wall.transform.position.x, (int)wall.transform.position.y, 0, 0, null));
        }
        
        return wallsNodes;
    }

}
