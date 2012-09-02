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
using SkypeNet.Lib.Core.Messages;

namespace SkypeNet.App
{
    public partial class Form1 : Form
    {
        private TaskScheduler _uiScheduler;
        private Lib.SkypeNet _skype;

        public Lib.SkypeNet Skype
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

                    _skype.Dispose();
                    _skype = null;
                }

                _skype = value;

                if (_skype != null )
                {
                    _skype.StatusChanged += SkypeOnStatusChanged;
                    _skype.MessageReceived += SkypeOnMessageReceived;
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

        private void btnTest_Click(object sender, EventArgs e)
        {
            rtbOutput.Text += "============ New Client ==========\n";
            rtbMessages.Text += "============ New Client ==========\n";

            Skype = new Lib.SkypeNet();

            var task = Skype.ConnectAsync();
            task.ContinueWith(ret =>
                                  {
                                      if( ret.IsCompleted )
                                      {
                                          rtbOutput.Text += "Connected!";
                                      }
                                      else
                                      {
                                          rtbOutput.Text += ret.Exception != null ? ret.Exception.ToString() : "Failed!";
                                      }
                                  }, _uiScheduler);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if( Skype == null )
                return;

            Skype.SendMessage("GET SKYPEVERSION");
        }
    }
}
