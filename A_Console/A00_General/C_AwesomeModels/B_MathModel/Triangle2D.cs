using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public class Triangle2D : PolyLine2D
{

    // ★★★★★★★★★★★★★★★ inits

    public Triangle2D(Point2D p0, Point2D p1, Point2D p2) : base(new Point2D[] { p0, p1, p2 })
    {
    }


    // ★★★★★★★★★★★★★★★ methods

    public Point2D P0 => Vertices.ElementAt(0);
    public Point2D P1 => Vertices.ElementAt(1);
    public Point2D P2 => Vertices.ElementAt(2);
    public List<Point2D> Points => Vertices.ToList();

    ///<summary>
    /// Find outer circle 
    /// </summary>
    /// <see cref="https://issekinichou.wordpress.com/2021/05/16/3%E7%82%B9%E3%82%92%E9%80%9A%E3%82%8B%E5%86%86%EF%BC%882d%EF%BC%89/"/>
    /// <returns></returns>
    public Circle2D GetOuterCircle()
    {
        var a = new Point3D(P0.X, P1.X, P2.X);
        var b = new Point3D(P0.Y, P1.Y, P2.Y);
        var c = new Point3D(1, 1, 1);
        var d = new Point3D
            (
            -(a.X * a.X) - (b.X * b.X) - (c.X * c.X),
            -(a.Y * a.Y) - (b.Y * b.Y) - (c.Y * c.Y),
            -(a.Z * a.Z) - (b.Z * b.Z) - (c.Z * c.Z)
            );

        static double Det(Point3D a, Point3D b, Point3D c)
            => (0
            + a.X * b.Y * c.Z
            + a.Z * b.X * c.Y
            + a.Y * b.Z * c.X
            - a.Z * b.Y * c.X
            - a.Y * b.X * c.Z
            - a.X * b.Z * c.Y
            );

        var l = Det(d, b, c) / Det(a, b, c);
        var m = Det(a, d, c) / Det(a, b, c);
        var n = Det(a, b, d) / Det(a, b, c);

        var center = new Point2D(-l / 2, -m / 2);
        var radius = Math.Sqrt((l * l + m * m - 4 * n) / 4);

        return new Circle2D(center, radius);
    }

    public override string ToString()
    {
        return $"{P0}, {P1}, {P2}";
    }


    // ★★★★★★★★★★★★★★★

}
