﻿using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }

    static LinkedList<Transform> edibleBugs = new LinkedList<Transform>();
    static System.Random rand = new System.Random();

    public static void AddEdibleBug(Transform transform)
    {
        if (edibleBugs == null)
        {
            edibleBugs.AddFirst(transform);
        }
        else
        {
            edibleBugs.AddLast(transform);
        }
    }

    public static void RemoveEdibleBug(Transform transform)
    {
        edibleBugs.Remove(transform);
    }

    public static Transform SelectBug()
    {
        int scope = 1;
        Transform chosenBug = null;
        LinkedListNode<Transform> currentNode = edibleBugs.First;

        while (currentNode != null)
        {
            if (rand.Next() < 1 / scope)
            {
                chosenBug = currentNode.Value;
            }

            scope++;
            currentNode = currentNode.Next;
        }

        return chosenBug;
    }
}