using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SkypeNet.Lib.Core
{
    public static class ThreadingExtensions
    {
        public static void InvokeThreadSafe(this SynchronizationContext context, Action action)
        {
            if (action == null)
                return;

            if (context != null && context != SynchronizationContext.Current)
                context.Post(_ => action(), null);
            else
                action();
        }
    }
}
