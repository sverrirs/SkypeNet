using System;

namespace SkypeNet.Lib.Core
{
    /// <summary>
    /// Defines a skype API parameter that should map to the property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SkypeParameterAttribute : Attribute
    {
        public string Name { get; set; }

        public SkypeParameterAttribute(string name)
        {
            Name = name;
        }
    }
}