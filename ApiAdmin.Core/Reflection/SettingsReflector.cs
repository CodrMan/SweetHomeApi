using System;
using System.Collections.Generic;
using System.Reflection;

namespace ApiAdmin.Core.Reflection
{
    public class SettingsReflector : ISettingsReflector
    {
        private static readonly Dictionary<Type, SettingsTypeContainer> SettinsCache =
            new Dictionary<Type, SettingsTypeContainer>();

        public string[] GetKeys<T>(bool useLowerCase = false)
        {
            SettingsTypeContainer container = GetOrCreateContainer<T>();
            return container.GetKeys(useLowerCase);
        }

        public T CreateNewObject<T>(Dictionary<string, string> properties) where T : new()
        {
            var settingObject = new T();
            var container = GetOrCreateContainer<T>();

            foreach (var propertyPair in container.Properties)
            {
                string value;

                if (!properties.TryGetValue(propertyPair.Value.Name, out value))
                    value = propertyPair.Value.DefaultValue;

                propertyPair.Key.SetValue(settingObject, ConvertValue(propertyPair.Key, value));
            }

            return settingObject;
        }

        private object ConvertValue(PropertyInfo propertyInfo, string value)
        {
            var type = propertyInfo.PropertyType;

            if (string.IsNullOrWhiteSpace(value))
                return type.IsValueType ? Activator.CreateInstance(type) : null;

            return Convert.ChangeType(value, type);
        }

        private SettingsTypeContainer GetOrCreateContainer<T>()
        {
            SettingsTypeContainer container;

            if (SettinsCache.TryGetValue(typeof(T), out container))
                return container;

            container = CreateNewContainer<T>();
            return container;
        }

        private SettingsTypeContainer CreateNewContainer<T>()
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propertiesMap = new Dictionary<PropertyInfo, SettingPropertyAttribute>();

            foreach (var propertyInfo in properties)
            {
                var attribute = propertyInfo.GetCustomAttribute<SettingPropertyAttribute>();

                if (attribute != null)
                    propertiesMap.Add(propertyInfo, attribute);

            }

            var container = new SettingsTypeContainer(type, propertiesMap);
            SettinsCache.Add(type, container);
            return container;
        }
    }
}
