using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ chainable

    /// <summary>
    /// Check if point is in circle 
    /// 円の中に点が入ってるかどうか。
    /// <param name="targetPoint"></param>
    /// <param name="circle"></param>
    /// <returns></returns>
    public static bool IsPointInCircle(this Circle2D circle, Point2D targetPoint)
    {
        var distance = targetPoint.DistanceTo(circle.Center);
        return distance < circle.Radius;
    }

    /// <summary>
    /// Merge polyline and triangle to make bigger polyline
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static PolyLine2D ConnectTriangle(this PolyLine2D? polygon, Triangle2D triangle)
    {
        if (polygon is null)
            return new PolyLine2D(triangle.Points);

        var polyPoints = polygon.Vertices.ToList();
        var triPoints = triangle.Vertices.ToList();

        var adjacentPolyIndex0 = polyPoints.FindIndex(p => triPoints.Contains(p));
        var adjacentPolyIndex1 = polyPoints.FindLastIndex(p => triPoints.Contains(p));

        if (adjacentPolyIndex0 == -1 || adjacentPolyIndex0 == adjacentPolyIndex1)
            throw new InvalidDataException("Polygon and triangle must share their 2 points");

        var triIndex0 = triPoints.FindIndex(p => p == polyPoints[adjacentPolyIndex0]);
        var triIndex1 = triPoints.FindIndex(p => p == polyPoints[adjacentPolyIndex1]);
        var triIndex = new int[] { 0, 1, 2 }.Except(new int[] { triIndex0, triIndex1 }).First();
        var addingPoint = triPoints[triIndex];

        // If indexes next to each other OR first and last
        if (adjacentPolyIndex1 - adjacentPolyIndex0 == 1)
            polyPoints.Insert(adjacentPolyIndex1, addingPoint);
        else
            polyPoints.Add(addingPoint);

        return new PolyLine2D(polyPoints);
    }


    // ★★★★★★★★★★★★★★★ 

}
