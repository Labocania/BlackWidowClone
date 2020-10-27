using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineDrawer : MonoBehaviour
{
	// The width of the line.
	public float width;

	// Local variables and references.
	LineRenderer lineRenderer;
	public PointList pointList;

	void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = false;
		lineRenderer.startWidth = width;
		lineRenderer.endWidth = width;		
	}

    void Start()
    {
		SetPoints();
	}
    /**
	 * This method sets the points that the line renderer will use.
	 */
    public void SetPoints()
	{
		List<Vector3> drawPoints = new List<Vector3>();
		foreach (Vector3 point in pointList.points)
		{
			drawPoints.Add(point);
		}

        for (int k = pointList.points.Count - 1; k >= 1; k--)
        {
			drawPoints.Add(pointList.points[k]);
		}

		lineRenderer.positionCount = drawPoints.Count;

		int i = 0;
		foreach (Vector3 point in drawPoints)
		{
			lineRenderer.SetPosition(i, point);
			i++;
		}
	}
}
