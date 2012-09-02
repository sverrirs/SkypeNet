using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SkypeNet.Lib.Core.Objects
{
    public sealed class SkypeUser
    {
        /// <summary>
        ///  – username, for example: USER pamela HANDLE pamela .
        /// </summary>
        [SkypeParameter("HANDLE")]
        public string UserName { get; set; }

        /// <summary>
        ///  user online status, for example: USER mike ONLINESTATUS ONLINE . Possible values:
        /// UNKNOWN – unknown user.
        /// OFFLINE – user is offline (not connected). Will also be returned if current user is not authorized by other user to see his/her online status.
        /// ONLINE – user is online.
        /// AWAY – user is away (has been inactive for certain period).
        /// NA – user is not available.
        /// DND – user is in “Do not disturb” mode.
        /// </summary>
        [SkypeParameter("ONLINESTATUS")]
        public string OnlineStatus { get; set; }

        /// <summary>
        /// Stores the number of authorized contacts in the contact list.
        /// </summary>
        [SkypeParameter("NROF_AUTHED_BUDDIES")]
        public string AuthorizedContacts { get; set; }

    }
}
