using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{

    // ★★★★★★★★★★★★★★★ FileSystemInfo chain process

    /// <summary>
    /// WriteExifInfoToConsole
    /// </summary>
    /// <param name="inputFile"></param>
    /// <returns></returns>
    public static void WriteExifInfoToConsole(this FileInfo inputFile)
    {
        // sugar
        using var bitmap = new Bitmap(inputFile.FullName);

        Console.WriteLine($"meta info of {inputFile.Name}");
        foreach (PropertyItem item in bitmap.PropertyItems)
        {
            Console.Write($"Id: {item.Id}, Len: {item.Len}, Type: {item.Type}, Value: ");

            switch (item.Type)
            {
                case 1: // BYTE: array of bytes
                    for (int i = 0; i < item.Len; i++)
                        Console.Write($"{item.Value[i]:X2} ");
                    break;
                case 2: // ASCII: null-terminated ASCII string
                    Console.Write(Encoding.ASCII.GetString(item.Value));
                    break;
                case 3: // SHORT: array of unsigned short (16bit)
                    for (int i = 0; i < item.Len; i += 2)
                        Console.Write($"{BitConverter.ToUInt16(item.Value, i)} ");
                    break;
                case 4: // LONG: array of unsigned long (32bit)
                    for (int i = 0; i < item.Len; i += 4)
                        Console.Write($"{BitConverter.ToUInt32(item.Value, i)} ");
                    break;
                case 5: // RATIONAL: array of pairs of unsigned long
                    for (int i = 0; i < item.Len; i += 8)
                        Console.Write($"{BitConverter.ToUInt32(item.Value, i)}/{BitConverter.ToUInt32(item.Value, i + 4)} ");
                    break;
                case 7: // UNDEFINED: array of bytes
                    for (int i = 0; i < item.Len; i++)
                        Console.Write($"{item.Value[i]:X2} ");
                    break;
                case 9: // SLONG: array of singed long
                    for (int i = 0; i < item.Len; i += 4)
                        Console.Write($"{BitConverter.ToInt32(item.Value, i)} ");
                    break;
                case 10: // SRATIONAL: array of pairs of singed long
                    for (int i = 0; i < item.Len; i += 8)
                        Console.Write($"{BitConverter.ToInt32(item.Value, i)}/{BitConverter.ToInt32(item.Value, i + 4)} ");
                    break;
                default:
                    break;

            }

            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static void SetProperty(ref PropertyItem prop, int id, short type, object valObj)
    {
        prop.Id = id;
        prop.Type = type;
        switch (type)
        {
            case 1: // BYTE: array of bytes

                break;
            case 2: // ASCII: null-terminated ASCII string
                {
                    var val = (string)valObj;
                    int iLen = val.Length + 1;
                    var bTxt = new byte[iLen];
                    for (int i = 0; i < iLen - 1; i++)
                        bTxt[i] = (byte)val[i];
                    bTxt[iLen - 1] = 0x00;
                    prop.Value = bTxt;
                    prop.Len = iLen;
                }
                break;
            case 3: // SHORT: array of unsigned short (16bit)
                break;
            case 4: // LONG: array of unsigned long (ulong, 32bit, 4byte)
                {
                    var val = (ulong)valObj;
                    prop.Value = val.ToBytes();
                    prop.Len = 4;
                }
                break;
            case 5: // RATIONAL: array of pairs of unsigned long
                break;
            case 7: // UNDEFINED: array of bytes
                break;
            case 9: // SLONG: array of singed long (int, 4byte)
                {
                    var val = (int)valObj;
                    prop.Value = val.ToBytes();
                    prop.Len = 4;
                }
                break;
            case 10: // SRATIONAL: array of pairs of singed long
                break;
            default:
                break;
        }
    }

    //// ★★★★★★★★★★★★★★★ loop sugar


    // ★★★★★★★★★★★★★★★ 

}
