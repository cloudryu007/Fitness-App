using Fitness_App.Classes;
using Fitness_App.Screens;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_App
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            Select();

            Common.SendMessage(loginTextBox.Handle, Common.EM_SETCUEBANNER, 0, "USER");
            Common.SendMessage(passwordTextBox.Handle, Common.EM_SETCUEBANNER, 0, "PASSWORD");

            loginBtn.Click += button_Click;
            exitBtn.Click += button_Click;
            minimizeBtn.Click += button_Click;
            movePanel.MouseDown += mouseDown;
            movePanel2.MouseDown += mouseDown;
            offlineCheckBox.CheckedChanged += OfflineCheck;
        }

        //see if user logging in offline mode
        private void OfflineCheck(object sender, EventArgs e)
        {
            Common.offline = offlineCheckBox.Checked;
        }

        //button handlers
        private void button_Click(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            switch (name)
            {
                case "exitBtn":

                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Question", "Exit Application?", Types.YesNoCancel, Icons.Question);
                    msgbox.ShowDialog();
                    if (msgbox.yes) { Application.Exit(); }
                    break;
                case "loginBtn":
                    logon();
                    break;
                case "minimizeBtn":
                    WindowState = FormWindowState.Minimized;
                    break;
            }
        }

        //validate and log user in, collect data from DB if applicable
        private void logon()
        {
            string user = loginTextBox.Text;
            string pass = passwordTextBox.Text;
            bool ok = false;

            if (user.Length > 0 && pass.Length > 0)
            {
                if (!Common.offline)
                {
                    ValidateLogin validate = new ValidateLogin();
                    ok = validate.Connect(user, pass);
                    if (!ok)
                    {
                        MsgBox msgbox = new MsgBox();
                        msgbox.MessageBox("Connection Error", validate.exMess + ".", Types.OK, Icons.Error);
                        msgbox.ShowDialog();
                        return;
                    }
                }

                //display splash screen, hide original login screen
                Splash splash = new Splash();
                splash.Show();
                this.Hide();

                if (Common.offline)
                {
                    OfflineMode offline = new OfflineMode();
                    ok = offline.getOfflineData();//don't worry about failing

                    //grab offline samples if we have
                    offline = new OfflineMode();
                    offline.getOfflineSample();
                }

                else
                {
                    //check if we had offline mode file was geneated
                    OfflineMode offline = new OfflineMode();
                    ok = offline.getOfflineData();
                    if (!ok)
                    {
                        //if no offline mode then go fetch the users data from DB
                        DataBaseRequest request = new DataBaseRequest();
                        ok = request.GetRoutine(Common.userID);
                        if (!ok)
                        {
                            MsgBox msgbox = new MsgBox();
                            msgbox.MessageBox("Initalizing Error", request.exMess + ".", Types.OK, Icons.Error);
                            msgbox.ShowDialog();
                            Application.Exit();
                        }

                        //get completed workout related data
                        request = new DataBaseRequest();
                        ok = request.GetExerciseData(90);
                        if (!ok)
                        {
                            MsgBox msgbox = new MsgBox();
                            msgbox.MessageBox("Initalizing Error", request.exMess + ".", Types.OK, Icons.Error);
                            msgbox.ShowDialog();
                            Application.Exit();
                        }
                    }

                    else
                    {
                        //try to update the database
                        DataBaseRequest update = new DataBaseRequest();
                        ok = update.SaveRoutine(Common.userID);
                        if (!ok)
                        {
                            MsgBox msgbox = new MsgBox();
                            msgbox.MessageBox("Profile Error", "Error updating online profile, going into offline mode.", Types.OK, Icons.Error);
                            msgbox.ShowDialog();
                        }
                    }

                    //grab sample routines/exercises from database
                    DataBaseRequest samples = new DataBaseRequest();
                    samples.GetSample();
                }

                //use doublebuffer if possible
                DoubleBuff doubleBuff = new DoubleBuff();
                Common.doubleBuffer = doubleBuff.doubleBuffer();

                //keep track of changes
                Common.currentWorkouts = Common.userWorkouts;
                Dashboard dashboard = new Dashboard();
                splash.Close();
                dashboard.Show();
            }

            else
            {
                MsgBox msgbox = new MsgBox();
                msgbox.MessageBox("Input Error", "User or Password field cannot be empty.", Types.OK, Icons.Error);
                msgbox.ShowDialog();
                return;
            }
        }

        //allow us to move app window if mouse held down
        private void mouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Common.ReleaseCapture();
                Common.SendMessage(Handle, Common.WM_NCLBUTTONDOWN, Common.HT_CAPTION, 0);
            }
        }

        //enter button pressed process as login request
        private void Login_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                logon();
            }
        }
    }
}
