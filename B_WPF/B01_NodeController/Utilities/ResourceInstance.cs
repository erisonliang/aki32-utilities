using System.Windows;
using System.Windows.Threading;

namespace Aki32Utilities.WPFAppUtilities.NodeController.Utilities;
public class ResourceInstance<T> where T : DispatcherObject
{
    public T Get(string resourceName) => resource ??= Application.Current.TryFindResource(resourceName) as T;
    T resource = null;
}
