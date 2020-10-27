using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Point List")]
public class PointList : ScriptableObject
{
    public List<Vector3> points = new List<Vector3>();
}
