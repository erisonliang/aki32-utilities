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
        var xmin = points.Min(p => p.X) - offset;
        var xmax = points.Max(p => p.X) + offset;
        var ymin = points.Min(p => p.Y) - offset;
        var ymax = points.Max(p => p.Y) + offset;
        var additionalPoints = new List<Point2D>
        {
            new Point2D(xmin, ymin),
            new Point2D(xmax, ymin),
            new Point2D(xmax, ymax),
            new Point2D(xmin, ymax),
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

            ReMesh(currentPolyLine, addingPoint); // [f4]
        }

        // 最初に追加した4点の関連要素を全て削除して返す。
        return currentTriangles
            .Where(t => !t.Points.Any(p => additionalPoints.Contains(p)))
            .ToArray();

    }

    /// <summary>
    /// Re-mesh
    /// </summary>
    /// <param name="targetTriangle"></param>
    /// <param name="addingPoint"></param>
    private void ReMesh(PolyLine2D? targetTriangle, Point2D addingPoint)
    {
        if (targetTriangle is null)
            return;

        var polyoints = targetTriangle.Vertices.ToArray();
        for (int i = 0; i < polyoints.Length; i++)
        {
            currentTriangles.Add(new Triangle2D(
                addingPoint,
                polyoints[i],
                polyoints[(i + 1) % polyoints.Length]
                ));

        }
    }


    // ★★★★★★★★★★★★★★★ tests

    public static Point2D[] TestModel1 => new Point2D[]
    {
        new Point2D(1,2),
        new Point2D(3,4),
        new Point2D(4,1),
        new Point2D(5,3),
        new Point2D(6,2),
    };


    // ★★★★★★★★★★★★★★★ 

}
