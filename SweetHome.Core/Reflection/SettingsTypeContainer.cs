using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SweetHome.Core.Reflection
{
    public class SettingsTypeContainer
    {
        public Dictionary<PropertyInfo, SettingPropertyAttribute> Properties { get; private set; }

        public Type Type { get; private set; }

        public SettingsTypeContainer(Type type, Dictionary<PropertyInfo, SettingPropertyAttribute> properties)
        {
            Properties = properties;
            Type = type;
        }

        public string[] GetKeys(bool useLowerCase = false)
        {
            var keys = Properties.Select(e => e.Value.Name);
            return (!useLowerCase ? keys.Select(e => e.ToLower()) : keys).ToArray();
        }
    }
}
