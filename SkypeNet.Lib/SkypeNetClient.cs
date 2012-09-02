using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkypeNet.Lib.Core.Messages;

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
        public SkypeNetClient() : base()
        {
            this.MessageReceived += OnMessageReceived_ForParsingOfMessageData;
        }

        protected override void Dispose(bool disposing)
        {
            this.MessageReceived -= OnMessageReceived_ForParsingOfMessageData;

            base.Dispose(disposing);
        }

        private void OnMessageReceived_ForParsingOfMessageData(object sender, string s)
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

        }

        /// <summary>
        /// Sends a SMS text message to one or more targets
        /// </summary>
        /// <param name="target">the target of the sms (to add more recipients use the <see cref="SkypeAlterActions.SMS"/></param>
        /// <param name="smsType">The type of sms to send, ignored for now (fixed as <see cref="SkypeSmsTypes.OUTGOING"/>)</param>
        public void Sms(string target, SkypeSmsTypes smsType)
        {

        }

        #endregion


        #region Sending of Commands

        public void Get(SkypeGetActions action, params SkypeActionParameter[] parameters)
        {

        }

        public void Set(SkypeSetActions action, string subject, params SkypeActionParameter[] parameters)
        {

        }

        public void Alter(SkypeAlterActions action, string subject, params SkypeActionParameter[] parameters)
        {

        }

        public void Search(SkypeSearchActions action, string subject, params SkypeActionParameter[] parameters)
        {

        }

        public void Open(SkypeOpenActions action, string subject, params SkypeActionParameter[] parameters)
        {

        }

        #endregion

        /// <summary>
        /// Attempts to parse the event coming from skype and raise the appropriate 
        /// event back to the listener
        /// </summary>
        /// <param name="response">the raw response string from the app</param>
        private void ParseEventAndReport(string response)
        {
            
        }
    }
}
