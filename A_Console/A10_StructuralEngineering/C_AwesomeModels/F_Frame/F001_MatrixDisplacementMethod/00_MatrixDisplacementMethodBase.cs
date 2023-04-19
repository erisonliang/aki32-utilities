

using MathNet.Spatial.Euclidean;

namespace Aki32Utilities.ConsoleAppUtilities.StructuralEngineering;
public partial class MatrixDisplacementMethod
{

    // ★★★★★★★★★★★★★★★ tests

    public static Structure TestModel1
    {
        get
        {
            var material = MaterialType.Steel;
            var section = new CrossSection(material, 1000, 250000, 1000);

            var loads = new List<Load>
            {
                Load.CreateConcentratedLoad(5,0),
                Load.CreateConcentratedLoad(10,0),
            };

            var nodes = new List<Node>
            {
                new Node(new Point2D(0,0),new Fix(true,true,true)),
                new Node(new Point2D(10,0),new Fix(true,true,true)),
                new Node(new Point2D(20,0),new Fix(true,true,true)),

                new Node(new Point2D(0,5)){ Loads = new List<Load>{loads[0]} },
                new Node(new Point2D(10,5)),
                new Node(new Point2D(20,5)),

                new Node(new Point2D(0,10)){ Loads = new List<Load>{loads[1]} },
                new Node(new Point2D(10,10)),
                new Node(new Point2D(20,10)),
            };

            var members = new List<Member>
            {
                new Member(section,0,3,nodes),
                new Member(section,1,4,nodes),
                new Member(section,2,5,nodes),

                new Member(section,3,4,nodes),
                new Member(section,4,5,nodes),

                new Member(section,3,6,nodes),
                new Member(section,4,7,nodes),
                new Member(section,5,8,nodes),

                new Member(section,6,7,nodes),
                new Member(section,7,8,nodes),
            };

            var structure = new Structure(nodes, members);

            return structure;
        }
    }


    // ★★★★★★★★★★★★★★★ tests

}
