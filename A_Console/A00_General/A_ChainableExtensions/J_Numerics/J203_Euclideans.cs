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

    public static Line2D[] GetLines(this PolyLine2D polyline, bool ordered = false)
    {
        var lines = new List<Line2D>();
        void AddItem(Point2D target0, Point2D target1)
        {
            bool reverse;
            if (ordered)
                reverse = target0.Compare(target1) < 0;
            else
                reverse = false;

            if (reverse)
                lines.Add(new Line2D(target1, target0));
            else
                lines.Add(new Line2D(target0, target1));

        }

        var points = polyline.Vertices.ToArray();
        for (int i = 0; i < points.Length - 1; i++)
            AddItem(points[i], points[i + 1]);
        AddItem(points.Last(), points.First());

        return lines.ToArray();
    }


    // ★★★★★★★★★★★★★★★ chainable (sub)

    public static Line2D[] GetDistinctedLines(this Triangle2D[] triangles)
    {
        var lineEdges = new List<Line2D>();
        foreach (var triangle in triangles)
            lineEdges.AddRange(triangle.GetLines(ordered: true));

        return lineEdges
            .Distinct()
            .ToArray();

    }


    // ★★★★★★★★★★★★★★★ 

}
