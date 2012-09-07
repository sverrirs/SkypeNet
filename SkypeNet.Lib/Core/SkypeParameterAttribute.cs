using System;

namespace SkypeNet.Lib.Core
{
    /// <summary>
    /// Defines a skype API parameter that should map to the property
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SkypeParameterAttribute : Attribute
    {
        /// <summary>
        /// The Skype parameter name, should be UPPER CASE
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// For properties that are arrays but need to be assembled via multiple events
        /// use this property to indicate the position of the indexer within the value 
        /// when the serializer attempts to parse it.
        /// See: SkypeCall.ConferenceParticipants for an example on its use
        /// </summary>
        public int IndexerAt { get; set; }

        public SkypeParameterAttribute(string name)
        {
            Name = name;
        }
    }
}