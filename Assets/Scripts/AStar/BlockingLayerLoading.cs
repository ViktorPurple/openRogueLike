﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockingLayerLoading : MonoBehaviour {

    private GameObject[] walls;
    private int count;
    private List<Node> wallsNodes = new List<Node>();
    

    public  List<Node> getWalls()
    {
        count = 0;
        //find all walls with tag
        walls = GameObject.FindGameObjectsWithTag("blockingLayer");
        //create nodes for it
        foreach (GameObject wall in walls)
        {
            count++;
            wallsNodes.Add(new Node((int)wall.transform.position.x, (int)wall.transform.position.y, 0, 0, null));
        }

        Debug.Log(count);
        return wallsNodes;
    }

}
