using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SkypeNet.Lib.Core;
using SkypeNet.Lib.Core.Objects;

namespace SkypeNet.Lib
{
    /// <summary>
    /// Higher level client class that knows how to interpret various elements of the Skype Desktop API
    /// and provides a high-level access to skype functionality through direct action manipulation
    /// 
    /// This client is aware of different protocol support and can adjust it's support set accordingly
    /// </summary>
    public sealed class SkypeNetClient : SkypeNet
    {
        #region Members

        /// <summary>
        /// Contains all calls made and received from Skype
        /// </summary>
        private readonly Dictionary<string, SkypeCall> _calls = new Dictionary<string, SkypeCall>();

        private readonly Dictionary<string, SkypeUser> _users = new Dictionary<string, SkypeUser>();

        /// <summary>
        /// Temporarilly holds a pending call that the user initiated him/herself. Waiting to get a call id
        /// </summary>
        private SkypeCall _pendingCall;

        #endregion

        #region Events

        /// <summary>
        /// Raised when a new call is received in the application. This is usually when 
        /// someone calls the current user.
        /// </summary>
        public event GenericEventHandler<SkypeCall> CallReceived;
        private void OnCallReceived( SkypeCall call )
        {
            CallReceived.Raise(this, call);
        }

        /// <summary>
        /// Raised when an existing call that is either over or in progress is updated
        /// </summary>
        public event GenericEventHandler<SkypeCall> CallUpdated;
        private void OnCallUpdated( SkypeCall call )
        {
            CallUpdated.Raise(this, call);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The current user that is logged into the Skype instance that is connected
        /// </summary>
        public SkypeUser CurrentUser { get; private set; }

        /// <summary>
        /// Gets the version of skype that the user is running
        /// </summary>
        public string SkypeVersion { get; private set; }

        /// <summary>
        /// The currently supported protocol version for this skype client.
        /// Default is <see cref="SkypeApiVersions.All"/>
        /// </summary>
        public SkypeApiVersions ProtocolVersion { get; set; }

        /// <summary>
        /// Gets the connection status of the Skype application
        /// </summary>
        public string ConnectionStatus { get; private set; }

        #endregion

        #region Constructor and Disposal

        public SkypeNetClient()
            : base()
        {
            ProtocolVersion = SkypeApiVersions.All;
            this.MessageReceived += OnMessageReceived_ForParsingOfMessageData;
        }

        protected override void Dispose(bool disposing)
        {
            this.MessageReceived -= OnMessageReceived_ForParsingOfMessageData;

            // Delete all the calls and clean up events
            foreach (var call in _calls.Values)
                call.RequestCallUpdate -= call_RequestCallUpdate_ToSendMessageToSkypeForData;

            base.Dispose(disposing);
        }

        #endregion

        #region Creation of internal objects

        /// <summary>
        /// Gets a user by username from the list of all known users. If user does not exist then 
        /// a new user is created with that username and added to the dictionary. The client then 
        /// requests basic information for this new user from the Skype application.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private SkypeUser GetOrCreateUser(string username)
        {
            SkypeUser user;
            if (!_users.TryGetValue(username, out user))
            {
                user = new SkypeUser() { UserName = username };
                _users.Add(username, user);
            }
            return user;
        }

        #endregion


        #region Handling Async Message retrieval and parsing

        private void OnMessageReceived_ForParsingOfMessageData(object sender, string rawResponse)
        {
            if( rawResponse == null ) return;
            var values = rawResponse.Split(' ');
            if (values.Length <= 1) return;

            string action = values[0];
            string subjectId = values[1];
            string property = values.Length > 2 ? values[2] : null;
            string value = values.Length > 3 ? values.Length == 4 ? values[3] : string.Join(" ", values, 3, values.Length - 4) : null;

            switch (action)
            {
                // Objects
                case "CALL":
                    HandleCallMessage(subjectId, property, value);
                    break;
                case "CHAT":
                    HandleChatMessage(subjectId, property, value);
                    break;
                case "GROUP":
                    HandleGroupMessage(subjectId, property, value);
                    break;
                case "USER":
                    HandleUserMessage(subjectId, property, value);
                    break;
                // Application state
                case "WINDOWSTATE":
                    break;
                case "SKYPEVERSION":
                    SkypeVersion = subjectId;
                    break;

                // Connection
                case "CONNSTATUS":
                    ConnectionStatus = subjectId;
                    break;

                // Current user
                case "CURRENTUSERHANDLE":
                    SetCurrentUser(subjectId);
                    break;
                case "USERSTATUS":
                    SetCurrentUserStatus(subjectId);
                    break;
                case "CHATMEMBER":
                    break;

                // Unknown stuff
                default:
                    Debug.Print("Unknown: "+rawResponse);
                    break;
            }
        }
        
        #endregion

        #region Updating Internal Client State From Messages

        private void SetCurrentUserStatus(string userstatus)
        {
            if (CurrentUser == null)
                return;

            CurrentUser.OnlineStatus = userstatus;
        }

        private void SetCurrentUser(string username)
        {
            CurrentUser = GetOrCreateUser(username);
        }

        #endregion

        #region Handling - Call Message

        private void HandleCallMessage(string callId, string property, string value)
        {
            // Attempt to locate the call message in all the calls available
            // if not found then we create a new one and raise the newCall event otherwise we invoke the call updated event
            SkypeCall call;
            bool isUpdate = _calls.TryGetValue(callId, out call);

            if (!isUpdate)
            {
                // If the property is STATUS and value is UNPLACED then check to see if we have a pending call
                // in that case we associate that pending call with this information and continue
                if (_pendingCall != null && property == "STATUS" && value == "UNPLACED")
                {
                    call = _pendingCall;
                    call.Id = callId;
                    _pendingCall = null;
                }
                else
                {
                    call = new SkypeCall {Id = callId};
                }

                // Add the call to the list of all known calls
                call.RequestCallUpdate -= call_RequestCallUpdate_ToSendMessageToSkypeForData;
                call.RequestCallUpdate += call_RequestCallUpdate_ToSendMessageToSkypeForData;
                _calls.Add(callId, call);
            }


            // Update the property value if it is set
            if (property != null)
                SkypeSerializer.Update(call, property, value);

            // Raise the correct events
            if (isUpdate)
                OnCallUpdated(call);
            else
                OnCallReceived(call);
        }

        private void call_RequestCallUpdate_ToSendMessageToSkypeForData(object sender, string propertyName, object[] values)
        {
            // TODO: Get the skype parameter value for the property name and instigate a skype get call data request!
        }

        #endregion
        
        private void HandleChatMessage(string chatId, string property, string value)
        {

        }

        private void HandleGroupMessage(string groupId, string property, string value)
        {

        }


        private void HandleUserMessage(string username, string property, string value)
        {

        }

        #region Core Commands

        /// <summary>
        /// Initiates a call to one or more targets
        /// </summary>
        /// <param name="targets">
        /// targets to be called. In case of multiple targets conference is created. Available target types:
        /// USERNAME – Skype username, e.g. “pamela”, “echo123”
        /// PSTN – PSTN phone number, e.g. “+18005551234”, “003725555555”
        /// SPEED DIAL CODE – 1 or 2 character speeddial code
        /// </param>
        public void Call(params string[] targets)
        {
            if (targets == null || targets.Length <= 0) throw new ArgumentNullException("targets", "You must specify at least one target for your call");
            if( _pendingCall != null ) throw new ServiceAccessException("You already have a pending outgoing call. Please either wait for that to complete or hang up");

            _pendingCall = new SkypeCall() { Targets = targets};

            SendMessage("CALL " + string.Join(", ", targets));
        }

        /*/// <summary>
        /// Sends a SMS text message to one or more targets
        /// </summary>
        /// <param name="target">the target of the sms (to add more recipients use the <see cref="SkypeAlterActions.SMS"/></param>
        /// <param name="smsType">The type of sms to send, ignored for now (fixed as <see cref="SkypeSmsTypes.OUTGOING"/>)</param>
        public void Sms(string target, SkypeSmsTypes smsType)
        {

        }*/

        #endregion
    }
}
