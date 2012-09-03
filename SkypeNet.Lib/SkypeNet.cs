using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkypeNet.Lib.Core;

namespace SkypeNet.Lib
{
    /// <summary>
    /// The status of the skype application
    /// </summary>
    public enum SkypeStatus : uint
    {
        /// <summary>
        /// The client is attached and the API window handle is provided 
        /// </summary>
        Success = 0,
        /// <summary>
        /// Skype acknowledges the connection request and is waiting for user confirmation. 
        /// The client is not yet attached and must wait for the <see cref="SkypeStatus.Success"/> message.
        /// </summary>
        PendingAuthorizaion = 1,
        /// <summary>
        /// The user has explicitly denied access to client.
        /// </summary>
        Refused = 2,
        /// <summary>
        /// The API is not available at the moment, for example because no user is currently logged in. 
        /// The client must wait for a <see cref="SkypeStatus.Available"/> broadcast before attempting to connect again.
        /// </summary>
        NotAvailable = 3,
        /// <summary>
        /// When the API becomes available, Skype broadcasts the <see cref="SkypeStatus.Available"/>message to all 
        /// application windows in the system.
        /// </summary>
        Available = 0x8001,
        /// <summary>
        /// Unknown initial status, this is not related to the official Skype API
        /// </summary>
        Unknown = 99
    }

    /// <summary>
    /// The API versions of the skype desktop protocol. This is used to determine which properties and features are 
    /// available and can be used by the system.
    /// 
    /// Currently only the latest protocol version is supported!
    /// 
    /// See: http://developer.skype.com/public-api-reference
    /// </summary>
    public enum SkypeApiVersions
    {
        /// <summary>
        /// No protocol version specified.
        /// </summary>
        All,
        /// <summary>
        /// Initial release, really? Support this? Perhaps not.
        /// </summary>
        Protocol1,
        /// <summary>
        /// Protocol 2 is used by the following version of 
        /// Skype: 1.0.0.94
        /// This protocol implemented the following changes:
        /// Introduced the SKYPEME online status
        /// For calls on hold, notifies clients with either LOCALHOLD or REMOTEHOLD . Protocol 1 simply returned ONHOLD .
        /// Introduces the call status, CANCELLED.
        /// </summary>
        Protocol2,
        /// <summary>
        /// Protocol 3 is used by the following version of Skype:
        /// 1.1.0.61 – Windows
        /// This protocol introduced a compatibility layer for previous versions of instant messaging.
        /// </summary>
        Protocol3,
        /// <summary>
        /// Date: 2005-03-04
        /// Protocol 4 is used by the following versions of Skype:
        /// 1.2.0.11 – Windows
        /// 1.1.0.3 – Windows and Linux
        /// This protocol introduced ISO code prefixes for language and country.
        /// </summary>
        Protocol4,
        /// <summary>
        /// Protocol 5 is used by the following versions of Skype:
        /// Date: 2005-06-11
        /// 2.0 – Windows
        /// 1.4.0.84 – Windows (Skype 1.3.0.42)
        /// 1.3.0.33 – Windows and Mac
        /// This protocol introduced multiperson chat commands, one-to-one video calls, call forwarding, and contact grouping.
        /// </summary>
        Protocol5,
        /// <summary>
        /// VOICEMAIL command enters deprecation process and is replaced by CALLVOICEMAIL command.
        /// </summary>
        Protocol6,
        /// <summary>
        /// Skype 3.0
        /// Call transfer API, We have two new CALL statuses: TRANSFERRING|TRANSFERRED
        /// Modified CHATMESSAGE property TYPE enumerations:
        /// TYPE = POSTEDCONTACTS|GAP_IN_CHAT|SETROLE|KICKED|SETOPTIONS| KICKBANNED|JOINEDASAPPLICANT|SETPICTURE|SETGUIDELINES 
        /// </summary>
        Protocol7,
        /// <summary>
        /// Skype 3.5
        /// Date: 2007-07-02
        /// New CALL STATUS enumerator – WAITING_REDIAL_COMMAND.
        /// New CALL STATUS enumerator – REDIAL_PENDING.
        /// New SMS FAILUREREASON enumerator – NO_SENDERID_CAPABILITY.
        /// Sending chat messages and CHAT CREATE commands may now fail with a new error code: 615, “CHAT: chat with given contact is disabled”.
        /// </summary>
        Protocol8
    }

    /// <summary>
    /// Low-level wrapper for the Skype Desktop API. It wraps the necessary window messaging communication and unmanaged 
    /// </summary>
    /// <remarks>
    /// To check if Skype is installed, in regedit check if the following key exists: HKCU\Software\Skype\Phone, SkypePath . 
    /// This key points to the location of the skype.exe file . If this key does not exist, check if the 
    /// HKLM\Software\Skype\Phone, SkypePath key exists. If the HKCU key does not exist but the HKLM key is present, 
    /// Skype has been installed from an administrator account but not been used from the current account.
    /// </remarks>
    public class SkypeNet : Control
    {
        #region Members

        private const string SkypeWindowMessageDiscover = "SkypeControlAPIDiscover";
        private const string SkypeWindowMessageAttach = "SkypeControlAPIAttach";

        private IntPtr _skypeWindowHandle = IntPtr.Zero;

        private SkypeStatus _status = SkypeStatus.Unknown;
        
        private uint _skypeWindowMessageDiscoverId;
        private uint _skypeWindowMessageAttachId;
        private readonly SynchronizationContext _context;

        /// <summary>
        /// Tracks the last message sent by the application 
        /// used to quickly discard of confirmation ping backs from the skype app
        /// </summary>
        private string _lastMessageSent;

        #endregion
        
        #region Events

        /// <summary>
        /// Raised before the <see cref="Status"/> property is changed. Provides access to the current status (first argument) 
        /// and the soon to be new status (second argument)
        /// </summary>
        public event GenericEventHandler<SkypeStatus, SkypeStatus> StatusChanging;
        private void OnStatusChanging(SkypeStatus currentStatus, SkypeStatus newStatus)
        {
            _context.InvokeThreadSafe(() => StatusChanging.Raise(this, currentStatus, newStatus));
        }

        /// <summary>
        /// Raised after the <see cref="Status"/> property has changed
        /// </summary>
        public event GenericEventHandler<SkypeStatus> StatusChanged;
        private void OnStatusChanged(SkypeStatus status)
        {
            _context.InvokeThreadSafe(() => StatusChanged.Raise(this, status));
        }

        /// <summary>
        /// Raised when a message is recieved from the Skype application
        /// </summary>
        public event GenericEventHandler<string> MessageReceived;
        private void OnMessageReceived(string message)
        {
            _context.InvokeThreadSafe(() => MessageReceived.Raise(this, message));
        }
        #endregion
        
        #region Properties

        /// <summary>
        /// The current status of the skype application.
        /// </summary>
        public SkypeStatus Status
        {
            get { return _status; }
            set
            {
                if (_status == value) return;

                OnStatusChanging(_status, value);

                _status = value;

                OnStatusChanged(_status);
            }
        }

        #endregion

        public SkypeNet()
        {
            _context = SynchronizationContext.Current;
        }

        protected override void Dispose(bool disposing)
        {
            // Destroy the connection
            Disconnect();

            _skypeWindowMessageDiscoverId = 0;
            _skypeWindowMessageAttachId = 0;

            Status = SkypeStatus.Unknown;
            
            // Do I need to destroy the handle as well?
            DestroyHandle();

            base.Dispose(disposing);
        }

        #region Connection and Disconnection

        /// <summary>
        /// Connects to an active instance of skype
        /// </summary>
        public Task ConnectAsync()
        {
            if (_skypeWindowHandle != IntPtr.Zero)
                throw new InvalidOperationException("This client is already connected to a Skype instance. Please disconnect first");

            // If the skype client is in a NotAvailable state we need to throw an error and ask the user to wait until 
            // the client becomes Available again before attempting a connection
            if( Status == SkypeStatus.NotAvailable )
                throw new ServiceAccessException("The API is not available at the moment, possible explanation is that there is no user currently logged in. Please wait until the client updates its status to 'Available' before attempting to connect again");

            // Begin by creating a handle for this class so that we can register for Messages
            // Is it better to force the re-creation of a handle? 
            RecreateHandle();

            TaskCompletionSource<bool> sSource = new TaskCompletionSource<bool>();
            GenericEventHandler<SkypeStatus> taskHandler = null;
            taskHandler = (s, e) =>
                              {
                                  Debug.Print("Task : " + e);
                                  if (e == SkypeStatus.Success)
                                  {
                                      sSource.TrySetResult(true);

                                      // Unsubscribe from this handler
                                      this.StatusChanged -= taskHandler;
                                  }
                                  else if (e == SkypeStatus.Refused || e == SkypeStatus.NotAvailable || e == SkypeStatus.Unknown)
                                  {
                                      sSource.TrySetException(new ServiceAccessException("Could not connect to an active skype instance. Skype response '" + e + "'"));

                                      // Unsubscribe from this handler
                                      this.StatusChanged -= taskHandler;
                                  }
                              };
            this.StatusChanged += taskHandler;

            // Register for the window messages
            _skypeWindowMessageDiscoverId = Win32Api.RegisterWindowMessage(SkypeWindowMessageDiscover);
            _skypeWindowMessageAttachId = Win32Api.RegisterWindowMessage(SkypeWindowMessageAttach);

            // Send the connect message to the Skype applications
            IntPtr result;
            IntPtr aRetVal = Win32Api.SendMessageTimeout(Win32Api.HWND_BROADCAST, _skypeWindowMessageDiscoverId, Handle, IntPtr.Zero, Win32Api.SendMessageTimeoutFlags.SMTO_NORMAL, 100, out result);

            // If the return value is the zero pointer then we couldn't connect to a skype application
            // maybe one isn't running at the moment
            if (aRetVal == IntPtr.Zero)
                sSource.TrySetException(new ServiceAccessException("Could not connect to an active skype application (no response received). Make sure that at least one instance of Skype is running on your computer."));

            return sSource.Task;
        }

        public void Disconnect()
        {
            // If not connected, simply just exit without error
            if (_skypeWindowHandle == IntPtr.Zero)
                return;

            SendMessage(string.Empty);
            _skypeWindowHandle = IntPtr.Zero;
        }

        #endregion

        #region Sending Messages
        public bool SendMessage(string message)
        {
            if (_skypeWindowHandle == IntPtr.Zero)
                throw new InvalidOperationException("This client hasn't been connected to a Skype instance yet. Please call ConnectAsync first and wait for it to successfully complete.");

            Debug.Print("-> " + message);

            // Store the last sent message (before it is modified below)
            _lastMessageSent = message;

            var data = new Win32Api.CopyDataStruct {ID = 0};
            message += (char) 0; // The message needs to end with the zero terminator
            byte[] byteData = Encoding.UTF8.GetBytes(message);

            // Allocate an unmanaged block of memory (remember to clean up afterwards)
            IntPtr buffer = Marshal.AllocCoTaskMem(byteData.Length);
            Marshal.Copy(byteData, 0, buffer, byteData.Length);

            data.Data = buffer;
            data.Length = byteData.Length;

            IntPtr result;
            IntPtr returnCode = Win32Api.SendMessageTimeout(_skypeWindowHandle, Win32Api.WM_COPYDATA, Handle, ref data,
                                                            Win32Api.SendMessageTimeoutFlags.SMTO_NORMAL, 100, out result);

            // Yes, mother. Cleaning up the unmanaged memory block
            Marshal.FreeCoTaskMem(buffer);

            Debug.Print("SendMessageTimeout: " + returnCode.ToInt32() + " > " + result.ToInt32());

            return returnCode != IntPtr.Zero;
        }

        #endregion
        
        #region The Receiving of Messages

        protected override void WndProc(ref Message m)
        {
            // If the API client spends more than 1 second processing a message, the connection is disconnected. 
            // Use the PING command to test the connection status. To ease debugging during development, in 
            // regedit enter the key APITimeoutDisabled (DWORD value, 0 = timeout enabled 1 = timeout disabled) 
            // into the HKCU\Software\Skype\Phone\UI file in the registry to override the 1 second timeout.
            var msgId = (UInt32) m.Msg;

            // 
            // Reply from the async attach event, wooho we might have gotten our skype handle here!
            // or potentially lost it i guess :(
            if (msgId == _skypeWindowMessageAttachId)
            {
                var skypeStatus = (SkypeStatus) m.LParam;
                if (skypeStatus == SkypeStatus.Success)
                    _skypeWindowHandle = m.WParam;
                else
                    _skypeWindowHandle = IntPtr.Zero; // Clear out the handle if we get anything else than Success status

                Debug.Print("<< Attach: " + skypeStatus);

                // Signal a status changed for the skype client
                OnStatusChanged(skypeStatus);

                // The result of processing the message must be different from zero (0), otherwise Skype 
                // considers that the connection broken.
                m.Result = new IntPtr(1);
                return;
            }

            //
            // Not really sure why we would get this message back? Well in case we do 
            // lets at least log it
            if (msgId == _skypeWindowMessageDiscoverId )
            {
                Debug.Print("<< SkypeControlAPIDiscover");

                m.Result = new IntPtr(1);
                return;
            }
            
            //
            // We got some data sent from Skype, what could it be?
            if (msgId == Win32Api.WM_COPYDATA)
            {
                // Only handle messages from the attached skype instance
                if (m.WParam != _skypeWindowHandle)
                    return;

                var data = (Win32Api.CopyDataStruct)m.GetLParam(typeof( Win32Api.CopyDataStruct ));
                var byteData = new byte[data.Length - 1]; //The last byte is 0; bad text formatting if copied to data!
                Marshal.Copy(data.Data, byteData, 0, data.Length - 1);
                string response = Encoding.UTF8.GetString(byteData);

                // The result of processing the message must be different from zero (0), otherwise Skype 
                // considers that the connection broken.
                m.Result = new IntPtr(1);

                Debug.Print("<- " + response);

                // If the message we got was a simple ping back of the same message we sent then
                // we can safely ignore it
                if (0 == string.Compare(_lastMessageSent, response, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.Print("  Ping back of same data, ignoring");
                    return;
                }

                OnMessageReceived(response);
                
                // We want to handle this type of winmessages, rest can go to the base implementation
                return;
            }

            base.WndProc(ref m);
        }

        #endregion

    }
}
