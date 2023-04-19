using Aki32Utilities.ConsoleAppUtilities.General;

using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public class DelaunayTriangulationExecuter
{

    // ★★★★★★★★★★★★★★★ field

    private List<Point2D> currentPoints;
    private List<Triangle2D> currentTriangles;


    // ★★★★★★★★★★★★★★★ methods

    public Triangle2D[] Execute(Point2D[] points)
    {
        currentPoints = points.ToList();

        // 最初に環境を囲うように4点追加。
        var offset = 1;
        var xMin = points.Min(p => p.X) - offset;
        var xMax = points.Max(p => p.X) + offset;
        var yMin = points.Min(p => p.Y) - offset;
        var yMax = points.Max(p => p.Y) + offset;
        var additionalPoints = new List<Point2D>
        {
            new Point2D(xMin, yMin),
            new Point2D(xMax, yMin),
            new Point2D(xMax, yMax),
            new Point2D(xMin, yMax),
        };
        currentPoints.AddRange(additionalPoints);

        // 最初の要素追加
        currentTriangles = new List<Triangle2D>
        {
            new Triangle2D(additionalPoints[0], additionalPoints[1], additionalPoints[2]),
            new Triangle2D(additionalPoints[2], additionalPoints[3], additionalPoints[0]),
        };

        // 最初に追加したやつ以外の全ての点を順番に追加しながら処理。
        for (int i = 0; i < currentPoints.Count - 4; i++)
        {
            var addingPoint = currentPoints[i];

            var mergingTriangles = new List<Triangle2D>();
            for (int j = currentTriangles.Count - 1; j >= 0; j--)
            {
                var tri = currentTriangles[j];
                if (tri.GetOuterCircle().IsPointInCircle(addingPoint))  // [f1] [f2]
                    mergingTriangles.Add(tri);
            }

            PolyLine2D? currentPolyLine = null;
            for (int j = 0; j < mergingTriangles.Count; j++)
            {
                var mergingTriangle = mergingTriangles[j];
                currentTriangles.Remove(mergingTriangle);
                currentPolyLine = currentPolyLine.ConnectTriangle(mergingTriangle); // [f3]
            }

            AddMeshesAroundAPoint(currentPolyLine, addingPoint); // [f4]
        }

        // 最初に追加した4点の関連要素を全て削除して返す。
        return currentTriangles
            .Where(t => !t.Points.Any(p => additionalPoints.Contains(p)))
            .ToArray();

    }

    /// <summary>
    /// Add meshes around "addingPoint"
    /// </summary>
    /// <param name="surroundingPolyline"></param>
    /// <param name="addingPoint"></param>
    private void AddMeshesAroundAPoint(PolyLine2D? surroundingPolyline, Point2D addingPoint)
    {
        if (surroundingPolyline is null)
            return;

        var polyPoints = surroundingPolyline.Vertices.ToArray();
        for (int i = 0; i < polyPoints.Length - 1; i++)
            currentTriangles.Add(new Triangle2D(addingPoint, polyPoints[i], polyPoints[i + 1]));
        currentTriangles.Add(new Triangle2D(addingPoint, polyPoints.Last(), polyPoints.First()));
    
    }


    // ★★★★★★★★★★★★★★★ samples

    public static Point2D[] SampleModel1 => new Point2D[]
    {
        new Point2D(1,2),
        new Point2D(3,4),
        new Point2D(4,1),
        new Point2D(5,3),
        new Point2D(6,2),
    };


    // ★★★★★★★★★★★★★★★ 

}
