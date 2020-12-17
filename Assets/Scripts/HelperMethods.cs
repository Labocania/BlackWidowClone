using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static LinkedList<Transform> EdibleBugs { get; private set; } = new LinkedList<Transform>();
    public static Transform playerTransform;
    static Dictionary<float, Vector3> rotationAngles = new Dictionary<float, Vector3>();
    static Dictionary<float, WaitForSeconds> waitTimes = new Dictionary<float, WaitForSeconds>();
    public static System.Random rand = new System.Random();   

    public static List<GameObject> GetChildren(this GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }

    public static void AddEdibleBug(Transform transform)
    {
        if (EdibleBugs == null)
        {
            EdibleBugs.AddFirst(transform);
        }
        else
        {
            EdibleBugs.AddLast(transform);          
        }
    }

    public static void RemoveEdibleBug(Transform transform)
    {
        EdibleBugs.Remove(transform);
    }

    public static Transform SelectBug()
    {
        Transform result = null;
        LinkedListNode<Transform> currentNode = EdibleBugs.First;
        for (int n = 2;  currentNode != null; n++)
        {
            if (rand.Next() % n == 0)
            {
                result = currentNode.Value;
            }

            currentNode = currentNode.Next;
        }

        return result;
    }

    public static Vector3 GetRotationAngle(float angle)
    {
        if (rotationAngles.ContainsKey(angle))
        {
            return rotationAngles[angle];
        }
        else
        {
            rotationAngles[angle] = new Vector3(0f, 0f, angle);
            return rotationAngles[angle];
        }
    }

    public static List<Vector3> GetRotationAngles(params float[] angles)
    {
        List<Vector3> selected = new List<Vector3>();
        for (int i = 0; i < angles.Length; i++)
        {
            selected.Add(GetRotationAngle(angles[i]));
        }
        return selected;
    }

    public static Vector3 GetRandomAngle(params float[] angles)
    {
        return GetRotationAngle(angles[rand.Next(angles.Length)]);
    }


    public static WaitForSeconds GetWaitTime(float time)
    {
        if (waitTimes.ContainsKey(time))
        {
            return waitTimes[time];
        }
        else
        {
            waitTimes[time] = new WaitForSeconds(time);
            return waitTimes[time];
        }
    }

    public static List<WaitForSeconds> GetWaitTimes(params float[] time)
    {
        List<WaitForSeconds> selected = new List<WaitForSeconds>();
        for (int i = 0; i < time.Length; i++)
        {
            selected.Add(GetWaitTime(time[i]));
        }
        return selected;
    }

    public static WaitForSeconds GetRandomWaitTime(params float[] times)
    {
        return GetWaitTime(times[rand.Next(times.Length)]);
    }
}