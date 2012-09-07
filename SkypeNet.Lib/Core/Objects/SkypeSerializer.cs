using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SkypeNet.Lib.Core.Objects
{
    /// <summary>
    /// Serializes all available Skype objects
    /// </summary>
    public static class SkypeSerializer
    {
        private static readonly Dictionary<Type, Dictionary<string,PropertyInfo>> _types = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        static SkypeSerializer()
        {
            Cache(typeof (SkypeCall));
            Cache(typeof (SkypeChat));
            Cache(typeof (SkypeUser));
        }

        private static Dictionary<string, PropertyInfo> Cache(Type objectType)
        {
            // Attempt to locate the type in the cache, if found then simpy return the already reflected data
            Dictionary<string, PropertyInfo> pDict;
            if( _types.TryGetValue(objectType, out pDict))
                return pDict;

            // Create a new cache dict and populate it
            pDict = new Dictionary<string, PropertyInfo>();
            _types.Add(objectType, pDict);

            // Reflect all public properties of this type
            foreach (var pInfo in objectType.GetProperties(BindingFlags.Instance|BindingFlags.Public ))
            {
                var tmpAtt = pInfo.GetCustomAttributes(typeof (SkypeParameterAttribute), false);
                if( tmpAtt.Length <= 0 ) continue;

                var pAtt = (SkypeParameterAttribute) tmpAtt[0];

                // Make sure that the property has both a getter and setter (public or private)
                if (pInfo.GetGetMethod(true) != null && pInfo.GetSetMethod(true) != null)
                    pDict.Add(pAtt.Name.ToUpper(), pInfo);
            }

            return pDict;
        }

        /// <summary>
        /// Updates an already live instance of a Skype object with a new value for the specified property
        /// </summary>
        /// <param name="instance">the skype object instance</param>
        /// <param name="property">the property name (not case sensitive)</param>
        /// <param name="value">the value to set the property to</param>
        public static void Update(object instance, string property, object value)
        {
            // Find the type in the cache, if not found then cache it first
            Type instanceType = instance.GetType();
            Dictionary<string, PropertyInfo> pDict = Cache(instanceType);

            // Find the property name in the cached dictionary (values are stored in UPPER)
            PropertyInfo pInfo;
            if (pDict == null || !pDict.TryGetValue(property.ToUpper(), out pInfo))
            {
                Debug.Print("Could not locate property '" + property + "' for type '" + instanceType.FullName+ "'");
                return;
            }

            // TODO: if the property is an array type, pull out the indexat property and attempt to assign the 
            // location in the array the value we have!!!

            // Check if the value can be assigned to this type
            if (value != null && !pInfo.PropertyType.IsInstanceOfType(value))
                value = Convert.ChangeType(value, pInfo.PropertyType);

            // Invoke the setter using the value
            pInfo.GetSetMethod(true).Invoke(instance, new[] {value});
        }
    }
}
