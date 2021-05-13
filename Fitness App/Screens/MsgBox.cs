using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_App.Screens
{
    public partial class MsgBox : Form
    {
        //settings to pass back
        public bool yes { get; set; }
        public bool no { get; set; }
        public bool ok { get; set; }
        public bool cancel { get; set; }
        public bool abort { get; set; }
        public bool retry { get; set; }
        public bool ignore { get; set; }

        public MsgBox()
        {
            InitializeComponent();
        }

        public void MessageBox(string title, string message, Types type, Icons icon)
        {
            this.Text = title;
            messageText.Text = message;

            //setup buttons based off user requested type
            switch (type)
            {
                case Types.Yes:
                    button4.Visible = true;
                    button4.Text = "Yes";
                    button4.Click += YesClicked;
                    break;
                case Types.YesNo:
                    button4.Visible = true;
                    button4.Text = "No";
                    button4.Click += NoClicked;
                    button3.Visible = true;
                    button3.Text = "Yes";
                    button3.Click += YesClicked;
                    break;
                case Types.YesNoCancel:
                    button4.Visible = true;
                    button4.Text = "Cancel";
                    button4.Click += CancelClicked;
                    button3.Visible = true;
                    button3.Text = "No";
                    button3.Click += NoClicked;
                    button2.Visible = true;
                    button2.Text = "Yes";
                    button2.Click += YesClicked;
                    break;
                case Types.AbortRetryIgnore:
                    button4.Visible = true;
                    button4.Text = "Ignore";
                    button4.Click += IgnoreClicked;
                    button3.Visible = true;
                    button3.Text = "Retry";
                    button3.Click += RetryClicked;
                    button2.Visible = true;
                    button2.Text = "Abort";
                    button2.Click += AbortClicked;
                    break;
                case Types.OK:
                    button4.Visible = true;
                    button4.Text = "OK";
                    button4.Click += OKClicked;
                    break;
                case Types.OKCancel:
                    button4.Visible = true;
                    button4.Text = "Cancel";
                    button4.Click += CancelClicked;
                    button3.Visible = true;
                    button3.Text = "OK";
                    button3.Click += OKClicked;
                    break;
                case Types.RetryCancel:
                    button4.Visible = true;
                    button4.Text = "Cancel";
                    button4.Click += CancelClicked;
                    button3.Visible = true;
                    button3.Text = "Retry";
                    button3.Click += RetryClicked;
                    break;
                default:
                    button4.Visible = true;
                    button4.Text = "OK";
                    button4.Click += OKClicked;
                    break;
            }

            //set icon
            switch (icon)
            {
                case Icons.Asterisk:
                    image.BackgroundImage = Properties.Resources.info;
                    break;
                case Icons.Error:
                    image.BackgroundImage = Properties.Resources.error;
                    break;
                case Icons.Exclamation:
                    image.BackgroundImage = Properties.Resources.exclamation;
                    break;
                case Icons.Hand:
                    image.BackgroundImage = Properties.Resources.error;
                    break;
                case Icons.Information:
                    image.BackgroundImage = Properties.Resources.info;
                    break;
                case Icons.None:
                    break;
                case Icons.Question:
                    image.BackgroundImage = Properties.Resources.question;
                    break;
                case Icons.Stop:
                    image.BackgroundImage = Properties.Resources.error;
                    break;
                case Icons.Warning:
                    image.BackgroundImage = Properties.Resources.exclamation;
                    break;
            }

            //set focus / refresh
            messageText.Focus();
            Refresh();
        }

        //custom button click events
        public void YesClicked(object sender, EventArgs e)
        {
            yes = true;
            this.Close();
        }

        public void NoClicked(object sender, EventArgs e)
        {
            no = true;
            this.Close();
        }

        public void OKClicked(object sender, EventArgs e)
        {
            ok = true;
            this.Close();
        }

        public void CancelClicked(object sender, EventArgs e)
        {
            cancel = true;
            this.Close();
        }

        public void RetryClicked(object sender, EventArgs e)
        {
            retry = true;
            this.Close();
        }

        public void AbortClicked(object sender, EventArgs e)
        {
            abort = true;
            this.Close();
        }

        public void IgnoreClicked(object sender, EventArgs e)
        {
            ignore = true;
            this.Close();
        }
    }

    //enumeration for buttons
    public enum Types
    {
        Yes = 0,
        YesNo = 1,
        YesNoCancel = 2,
        AbortRetryIgnore = 3,
        OK = 4,
        OKCancel = 5,
        RetryCancel = 6,
    }

    //enueration for icons
    public enum Icons
    {
        Asterisk = 0,
        Error = 1,
        Exclamation = 2,
        Hand = 3,
        Information = 4,
        None = 5,
        Question = 6,
        Stop = 7,
        Warning = 8
    }
}
