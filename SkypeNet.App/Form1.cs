using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SkypeNet.Lib;
using SkypeNet.Lib.Core.Objects;

namespace SkypeNet.App
{
    public partial class Form1 : Form
    {
        private TaskScheduler _uiScheduler;
        private Lib.SkypeNetClient _skype;

        public SkypeNetClient Skype
        {
            get { return _skype; }
            set
            {
                if (_skype == value)
                    return;

                if (_skype != null )
                {
                    _skype.StatusChanged -= SkypeOnStatusChanged;
                    _skype.MessageReceived -= SkypeOnMessageReceived;

                    _skype.CallReceived -= SkypeOnCallReceived;
                    _skype.CallUpdated -= SkypeOnCallUpdated;

                    _skype.Dispose();
                    _skype = null;
                }

                _skype = value;

                if (_skype != null )
                {
                    _skype.StatusChanged += SkypeOnStatusChanged;
                    _skype.MessageReceived += SkypeOnMessageReceived;

                    _skype.CallReceived += SkypeOnCallReceived;
                    _skype.CallUpdated += SkypeOnCallUpdated;
                }

            }
        }

        public Form1()
        {
            InitializeComponent();

            this.Load += OnLoad_ToInitializeTaskScheduler_Correctly;

            // Hook the output textbox into the programs debug output listeners
        }

        private void OnLoad_ToInitializeTaskScheduler_Correctly(object sender, EventArgs eventArgs)
        {
            // Need to do this here as the SyncContext needs to be correctly set up
            // and you can only gurarantee that when the window message loop is running
            _uiScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        private void SkypeOnMessageReceived(object sender, string skypeMessage)
        {
            /*int highlightIdx = -1;
            if (skypeMessage.Error != 0)
                highlightIdx = (rtbMessages.Text??"").Length;*/

            rtbMessages.AppendText(skypeMessage +"\n");

            /*if( highlightIdx != -1)
            {
                rtbMessages.Select(highlightIdx, skypeMessage.Response.Length);
                rtbMessages.SelectionBackColor = Color.Pink;
                rtbMessages.DeselectAll();
            }*/
        }

        private void SkypeOnStatusChanged(object sender, SkypeStatus skypeStatus)
        {
            lblStatus.Text = skypeStatus.ToString();
        }

        private void SkypeOnCallUpdated(object sender, SkypeCall skypeCall)
        {
            rtbOutput.AppendText("Call Updated: " + skypeCall.Id +" [" + skypeCall.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "] > " + skypeCall.Status + " > " + skypeCall.Duration + "\n");
        }

        private void SkypeOnCallReceived(object sender, SkypeCall skypeCall)
        {
            rtbOutput.AppendText("Call Received: " + skypeCall.Id + " [" + skypeCall.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss") + "] > " + skypeCall.Status + " > " + skypeCall.Duration + "\n");
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            rtbOutput.Text += "============ Skype Client ==========\n";
            rtbMessages.Text += "============ Skype Net ==========\n";

            Skype = new Lib.SkypeNetClient();

            var task = Skype.ConnectAsync();
            task.ContinueWith(ret =>
                                  {
                                      if( ret.IsCompleted )
                                      {
                                          rtbOutput.AppendText("Connected!\n");
                                      }
                                      else
                                      {
                                          rtbOutput.AppendText(ret.Exception != null ? ret.Exception.ToString() : "Failed!");
                                      }
                                  }, _uiScheduler);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if( Skype == null )
                return;

            Skype.SendMessage("GET SKYPEVERSION");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Skype.SendMessage("CALL echo123");
        }
    }
}
