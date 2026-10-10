using System.ComponentModel;
using UnityEngine;
using UnityEngine.Splines;

public class AdjustSplineKnotheights : MonoBehaviour
{
    [SerializeField] SplineContainer splineContainer;
    [SerializeField] Terrain terrain;
    [ContextMenu("Adjust Knot Heights")]
    [ContextMenu("Adjust Knot Heights")]
void AdjustSplineKnotheight()
{
    Spline spline = splineContainer.Spline;
    int count = spline.Count;
    float[] heights = new float[count];

    // Step 1: read the terrain height at each knot
    for (int i = 0; i < count; i++)
    {
        Vector3 pos = splineContainer.transform.TransformPoint((Vector3)spline[i].Position);
        pos.x = 0;
        heights[i] = terrain.SampleHeight(pos) + terrain.transform.position.y;
    }

    // Step 2: smooth the heights by averaging each with its neighbours (repeat for more smoothing)
    for (int pass = 0; pass < 3; pass++)
    {
        float[] smoothed = (float[])heights.Clone();
        for (int i = 1; i < count - 1; i++)
            smoothed[i] = (heights[i - 1] + heights[i] + heights[i + 1]) / 3f;
        heights = smoothed;
    }

    // Step 3: write the smoothed heights back onto the knots
    for (int i = 0; i < count; i++)
    {
        BezierKnot knot = spline[i];
        Vector3 pos = splineContainer.transform.TransformPoint((Vector3)knot.Position);
        pos.x = 0;
        pos.y = heights[i];
        knot.Position = splineContainer.transform.InverseTransformPoint(pos);
        spline[i] = knot;
    }
}
}
