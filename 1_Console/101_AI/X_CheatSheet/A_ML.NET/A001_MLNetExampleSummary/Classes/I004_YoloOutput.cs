using System.Data;
using System.Drawing;

namespace Aki32Utilities.ConsoleAppUtilities.AI.CheatSheet;
public class I004_YoloOutput
{
    public float[] PredictedLabels;

    public class Parser
    {

        // ★★★★★★★★★★★★★★★ props

        public int RowCount = 13;
        public int ColCount = 13;
        public int ChannelCount = 125; //125
        public int BoxesPerCell = 5;
        public int BoxInfoFeatureCount = 5;
        public int ClassCount = 20; //20
        public float CellWidth = 32;
        public float CellHight = 32;

        private int ChannelStride => RowCount * ColCount;


        // ★★★★★★★★★★★★★★★ list props

        public float[] Anchors = new float[]
        {
            1.08F, 1.19F, 3.42F, 4.41F, 6.63F, 11.38F, 9.42F, 5.11F, 16.62F, 10.52F
        };

        //public static string[] Labels = Enumerable.Range(0, CLASS_COUNT).Select(x => "object").ToArray();

        //public static string[] Labels = new string[] { "person", "bicycle", "car", "motorcycle", "airplane", "bus", "train", "truck", "boat", "traffic light", "fire hydrant", "stop sign", "parking meter", "bench", "bird", "cat", "dog", "horse", "sheep", "cow", "elephant", "bear", "zebra", "giraffe", "backpack", "umbrella", "handbag", "tie", "suitcase", "frisbee", "skis", "snowboard", "sports ball", "kite", "baseball bat", "baseball glove", "skateboard", "surfboard", "tennis racket", "bottle", "wine glass", "cup", "fork", "knife", "spoon", "bowl", "banana", "apple", "sandwich", "orange", "broccoli", "carrot", "hot dog", "pizza", "donut", "cake", "chair", "couch", "potted plant", "bed", "dining table", "toilet", "tv", "laptop", "mouse", "remote", "keyboard", "cell phone", "microwave", "oven", "toaster", "sink", "refrigerator", "book", "clock", "vase", "scissors", "teddy bear", "hair drier", "toothbrush" };

        public string[] Labels = new string[]
        {
            "aeroplane", "bicycle", "bird", "boat", "bottle",
            "bus", "car", "cat", "chair", "cow",
            "diningtable", "dog", "horse", "motorbike", "person",
            "pottedplant", "sheep", "sofa", "train", "tvmonitor"
        };


        //public static Color[] ClassColors = Enumerable.Range(0, CLASS_COUNT).Select(x => Color.Red).ToArray();
        public Color[] ClassColors = new Color[]
        {
            Color.Khaki,
            Color.Fuchsia,
            Color.Silver,
            Color.RoyalBlue,
            Color.Green,
            Color.DarkOrange,
            Color.Purple,
            Color.Gold,
            Color.Red,
            Color.Aquamarine,
            Color.Lime,
            Color.AliceBlue,
            Color.Sienna,
            Color.Orchid,
            Color.Tan,
            Color.LightPink,
            Color.Yellow,
            Color.HotPink,
            Color.OliveDrab,
            Color.SandyBrown,
            Color.DarkTurquoise
        };


        // ★★★★★★★★★★★★★★★ main methods


        public IList<I004_YoloBoundingBox> ParseOutputs(float[] yoloModelOutputs, float threshold = .3F)
        {
            var boxes = new List<I004_YoloBoundingBox>();

            for (int row = 0; row < RowCount; row++)
            {
                for (int column = 0; column < ColCount; column++)
                {
                    for (int box = 0; box < BoxesPerCell; box++)
                    {
                        var channel = (box * (ClassCount + BoxInfoFeatureCount));

                        RectangleF boundingBoxDimensions = ExtractBoundingBoxDimensions(yoloModelOutputs, row, column, channel);

                        float confidence = GetConfidence(yoloModelOutputs, row, column, channel);

                        RectangleF mappedBoundingBox = MapBoundingBoxToCell(row, column, box, boundingBoxDimensions);

                        if (confidence < threshold)
                            continue;

                        float[] predictedClasses = ExtractClasses(yoloModelOutputs, row, column, channel);

                        var (topResultIndex, topResultScore) = GetTopResult(predictedClasses);
                        var topScore = topResultScore * confidence;

                        if (topScore < threshold)
                            continue;

                        boxes.Add(new I004_YoloBoundingBox()
                        {
                            Rect = new RectangleF
                            {
                                X = (mappedBoundingBox.X - mappedBoundingBox.Width / 2),
                                Y = (mappedBoundingBox.Y - mappedBoundingBox.Height / 2),
                                Width = mappedBoundingBox.Width,
                                Height = mappedBoundingBox.Height,
                            },
                            Confidence = topScore,
                            Label = Labels[topResultIndex],
                            BoxColor = ClassColors[topResultIndex]
                        });
                    }
                }
            }
            return boxes;
        }

        public IList<I004_YoloBoundingBox> FilterBoundingBoxes(IList<I004_YoloBoundingBox> boxes, int limit, float threshold)
        {
            var activeCount = boxes.Count;
            var isActiveBoxes = new bool[boxes.Count];

            for (int i = 0; i < isActiveBoxes.Length; i++)
                isActiveBoxes[i] = true;

            var sortedBoxes = boxes.Select((b, i) => new { Box = b, Index = i })
                                .OrderByDescending(b => b.Box.Confidence)
                                .ToList();

            var results = new List<I004_YoloBoundingBox>();

            for (int i = 0; i < boxes.Count; i++)
            {
                if (isActiveBoxes[i])
                {
                    var boxA = sortedBoxes[i].Box;
                    results.Add(boxA);

                    if (results.Count >= limit)
                        break;

                    for (var j = i + 1; j < boxes.Count; j++)
                    {
                        if (isActiveBoxes[j])
                        {
                            var boxB = sortedBoxes[j].Box;

                            if (IntersectionOverUnion(boxA.Rect, boxB.Rect) > threshold)
                            {
                                isActiveBoxes[j] = false;
                                activeCount--;

                                if (activeCount <= 0)
                                    break;
                            }
                        }
                    }

                    if (activeCount <= 0)
                        break;
                }
            }
            return results;
        }

        private float IntersectionOverUnion(RectangleF boundingBoxA, RectangleF boundingBoxB)
        {
            var areaA = boundingBoxA.Width * boundingBoxA.Height;

            if (areaA <= 0)
                return 0;

            var areaB = boundingBoxB.Width * boundingBoxB.Height;

            if (areaB <= 0)
                return 0;

            var minX = Math.Max(boundingBoxA.Left, boundingBoxB.Left);
            var minY = Math.Max(boundingBoxA.Top, boundingBoxB.Top);
            var maxX = Math.Min(boundingBoxA.Right, boundingBoxB.Right);
            var maxY = Math.Min(boundingBoxA.Bottom, boundingBoxB.Bottom);

            var intersectionArea = Math.Max(maxY - minY, 0) * Math.Max(maxX - minX, 0);

            return intersectionArea / (areaA + areaB - intersectionArea);
        }


        // ★★★★★★★★★★★★★★★ sub methods

        private RectangleF ExtractBoundingBoxDimensions(float[] modelOutput, int x, int y, int channel)
        {
            return new RectangleF
            {
                X = modelOutput[GetOffset(x, y, channel)],
                Y = modelOutput[GetOffset(x, y, channel + 1)],
                Width = modelOutput[GetOffset(x, y, channel + 2)],
                Height = modelOutput[GetOffset(x, y, channel + 3)]
            };
        }

        private float GetConfidence(float[] modelOutput, int x, int y, int channel)
        {
            return Sigmoid(modelOutput[GetOffset(x, y, channel + 4)]);
        }

        private RectangleF MapBoundingBoxToCell(int x, int y, int box, RectangleF boxDimensions)
        {
            return new RectangleF
            {
                X = (x + Sigmoid(boxDimensions.X)) * CellWidth,
                Y = (y + Sigmoid(boxDimensions.Y)) * CellHight,
                Width = (float)Math.Exp(boxDimensions.Width) * CellWidth * Anchors[box * 2],
                Height = (float)Math.Exp(boxDimensions.Height) * CellHight * Anchors[box * 2 + 1],
            };
        }

        private int GetOffset(int x, int y, int channel)
        {
            // YOLO outputs a tensor that has a shape of 125x13x13, which 
            // WinML flattens into a 1D array.  To access a specific channel 
            // for a given (x,y) cell position, we need to calculate an offset
            // into the array
            return (channel * ChannelStride) + (y * ColCount) + x;
        }

        public float[] ExtractClasses(float[] modelOutput, int x, int y, int channel)
        {
            float[] predictedClasses = new float[ClassCount];
            int predictedClassOffset = channel + BoxInfoFeatureCount;
            for (int predictedClass = 0; predictedClass < ClassCount; predictedClass++)
            {
                predictedClasses[predictedClass] = modelOutput[GetOffset(x, y, predictedClass + predictedClassOffset)];
            }
            return Softmax(predictedClasses);
        }

        private ValueTuple<int, float> GetTopResult(float[] predictedClasses)
        {
            return predictedClasses
                .Select((predictedClass, index) => (Index: index, Value: predictedClass))
                .OrderByDescending(result => result.Value)
                .First();
        }


        // ★★★★★★★★★★★★★★★ sub sub methods

        private static float Sigmoid(float value)
        {
            var k = (float)Math.Exp(value);
            return k / (1.0f + k);
        }

        private static float[] Softmax(float[] values)
        {
            var maxVal = values.Max();
            var exp = values.Select(v => Math.Exp(v - maxVal));
            var sumExp = exp.Sum();

            return exp.Select(v => (float)(v / sumExp)).ToArray();
        }


        // ★★★★★★★★★★★★★★★ 

    }

}
