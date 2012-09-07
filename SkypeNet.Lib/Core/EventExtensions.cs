using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SkypeNet.Lib.Core
{
    public static class EventExtensions
    {
        public static void Raise(this PropertyChangedEventHandler handler, object sender, string propertyName )
        {
            if (handler != null)
                handler(sender, new PropertyChangedEventArgs(propertyName));
        }

        public static void Raise(this PropertyChangingEventHandler handler, object sender, string propertyName)
        {
            if (handler != null)
                handler(sender, new PropertyChangingEventArgs(propertyName));
        }        
    }
}
