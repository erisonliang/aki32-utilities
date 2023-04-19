using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reflection;

namespace Aki32Utilities.ConsoleAppUtilities.General;
public static partial class ChainableExtensions
{
    /// <summary>
    /// Fast AddRange() to ObservableCollection
    /// </summary>
    /// <remarks>
    /// Thanks for https://artfulplace.hatenablog.com/entry/2016/12/29/133950
    /// </remarks>
    public static void AddRange<T>(this ObservableCollection<T> source, IEnumerable<T> collection)
    {
        var itProperty = typeof(ObservableCollection<T>)
            .GetProperty("Items", BindingFlags.NonPublic | BindingFlags.Instance);

        var colResetMethod = typeof(ObservableCollection<T>).GetMethod("OnCollectionReset", BindingFlags.NonPublic | BindingFlags.Instance);

        //var colChangedMethod = typeof(ObservableCollection<T>)
        //    .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
        //    .Where(m => m.Name == "OnCollectionChanged")
        //    .FirstOrDefault(m => m.IsFamily)
        //    ;

        var list = itProperty!.GetValue(source) as List<T>;
        if (list != null)
        {
            var collectionList = collection.ToList();
            list.AddRange(collectionList);

            colResetMethod!.Invoke(source, null);

            //foreach (var item in collectionList)
            //    colChangedMethod!.Invoke(source, new object[] { new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item) });

            //colChangedMethod!.Invoke(source, new object[] { new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, collectionList) });
        }
    }
}
