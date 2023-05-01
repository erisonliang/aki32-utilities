

namespace Aki32Utilities.Azure.B2CAuth.FromApps.LogOn
{
    /// <summary>
    /// Simple platform specific service that is responsible for locating a 
    /// </summary>
    public interface IParentWindowLocatorService
    {
        object GetCurrentParentWindow();
    }
}
