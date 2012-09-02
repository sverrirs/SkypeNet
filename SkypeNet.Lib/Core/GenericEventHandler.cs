using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeNet.Lib.Core
{
    public delegate void GenericEventHandler<TEventArgs>(object sender, TEventArgs e);
    public delegate void GenericEventHandler<TEventArgs, REventArgs>(object sender, TEventArgs e1, REventArgs e2);

    public static class GenericEventHandlerExtensions
    {
        /// <summary>
        /// Raises events declared as <see cref="GenericEventHandler{TEventArgs}"/>
        /// </summary>
        public static void Raise<TEventArgs>(this GenericEventHandler<TEventArgs> handler, object sender, TEventArgs args)
        {
            if (handler != null)
                handler(sender, args);
        }

        /// <summary>
        /// Raises events declared as <see cref="GenericEventHandler{TEventArgs, REventArgs}"/>
        /// </summary>
        public static void Raise<TEventArgs, REventArgs>(this GenericEventHandler<TEventArgs, REventArgs> handler, object sender, TEventArgs args1, REventArgs args2)
        {
            if (handler != null)
                handler(sender, args1, args2);
        }

    }
}
