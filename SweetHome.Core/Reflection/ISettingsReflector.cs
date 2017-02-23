using System.Collections.Generic;

namespace SweetHome.Core.Reflection
{
    public interface ISettingsReflector
    {
        string[] GetKeys<T>(bool useLowerCase = true);
        T CreateNewObject<T>(Dictionary<string, string> properties) where T : new();
    }
}
