using System;

namespace ApiAdmin.Core.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class SettingPropertyAttribute : Attribute
    {
        public string Name { get; set; }

        public string DefaultValue { get; set; }

        public SettingPropertyAttribute(string name)
        {
            Name = name;
        }
    }
}
