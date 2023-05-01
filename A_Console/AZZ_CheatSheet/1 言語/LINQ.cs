

namespace Aki32Utilities.ConsoleAppUtilities.CheatSheet;
internal class LINQ
{
    public static void SelectMany()
    {
        var numbers = new List<int> { 1, 2, 3 };
        var letters = new List<string> { "a", "b", "c" };
        var grid = numbers.SelectMany(n => letters, (n, l) => (n.ToString() + l));
        // 1a, 1b, 1c, 2a, 2b, 2c, 3a, 3b, 3c 
    }

    public static void Join()
    {
        var customers = new List<(int Id, string Name)> { (1, "Bob"), (2, "Tom"), (3, "John") };
        var purchases = new List<(int CustomerId, int ProductId)> { (1, 2), (2, 2), (3, 2) };
        var joined = customers.Join     // outer コレクション
        (
            purchases,              // inner コレクション
            c => c.Id,              // outer キー選択
            p => p.CustomerId,      // inner キー選択
            (c, p) =>               // 結果
                $"{c.Name} は {p.ProductId}番の商品を購入しました。"
        );
    }

    public static void Zip()
    {
        var intNums = new int[] { 1, 2, 3 };
        var stringNums = new List<string> { "one", "two", "three", "ignored" };
        var zipped = intNums.Zip(stringNums, (n, w) => $"{n} = {w}");
    }
}