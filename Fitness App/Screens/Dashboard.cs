using Fitness_App.Classes;
using Fitness_App.Properties;
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
    public partial class Dashboard : Form
    {
        public bool exit = false;//public var to close form

        public Dashboard()
        {
            InitializeComponent();

            //some actions
            exitButton.Click += button_Click;
            logoutButton.Click += button_Click;
            minimizeButton.Click += button_Click;
            routineButton.Click += button_Click;
            workoutButton.Click += button_Click;
            userButton.Click += button_Click;
            movePanel.MouseDown += mouse_Down;
            logoPanel.MouseDown += mouse_Down;

            //set metrics
            if (Common.Metric == null) Common.Metric = "US";
            if (Common.Metric == "US") Common.weight = "lb"; else Common.weight = "kg";

            //setup user info
            if (!string.IsNullOrEmpty(Common.firstName))
            {
                userButton.Text = Common.firstName + " " + Common.lastName;
            }

            else
            {
                userButton.Text = "Anonymous";
            }

            //setup dash
            setData();
        }

        private void button_Click(object sender, EventArgs e)
        {
            bool setCollapse = false;
            string name = ((Control)sender).Name;
            switch (name)
            {
                case "exitButton":
                    Application.Exit();
                    break;
                case "logoutButton":
                    Application.Restart();
                    break;
                case "minimizeButton":
                    WindowState = FormWindowState.Minimized;
                    break;
                case "routineButton":
                    Routines routines = new Routines();
                    openForm(routines);
                    break;
                case "workoutButton":
                    Workouts workouts = new Workouts();
                    openForm(workouts);
                    break;
                case "userButton":
                    setCollapse = true;
                    break;
            }

            if (setCollapse)
            {
                if(userPanel.Size == userPanel.MaximumSize)
                {
                    userButton.Image = Resources.dropArrow;
                    userPanel.Size = userPanel.MinimumSize;
                }

                else
                {
                    userButton.Image = Resources.upArrow;
                    userPanel.Size = userPanel.MaximumSize;
                }
            }
        }

        private void openForm(Form form)
        {
            if(Common.activeForm != null)
            {
                if (Common.activeForm.Name == form.Name)
                {
                    return;//don't reopen any current active forms
                }

                else
                {
                    Common.activeForm.Close();//close out current active form
                }
            }

            //set new active form - append to dataPanl
            Common.activeForm = form;
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            dataPanel.Controls.Add(form);
            dataPanel.Tag = form;
            form.BringToFront();
            form.Show();
        }

        private void setData()
        {
            Panel border = new Panel();
            border.Name = "borderPanel";
            border.Size = new Size(1072, 2);
            border.BackColor = Color.FromArgb(37, 39, 77);
            border.Location = new Point(0, 0);
            border.Visible = true;
            border.BringToFront();
            dataPanel.Controls.Add(border);//insert dynamic border
            dateLbl.Text = Common.date.ToString("dddd, dd MMMM");

            if (dateLbl.Text.Length < 24)
            {
                int diff = 24 - dateLbl.Text.Length;
                int xNew = diff * 7;
                int y = dateLbl.Location.Y;
                int x = dateLbl.Location.X;
                xNew = x + xNew;

                dateLbl.Location = new Point(xNew, y);//set the label postioning correctly, pre length
            }
        }

        private void mouse_Down(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Common.ReleaseCapture();
                Common.SendMessage(Handle, Common.WM_NCLBUTTONDOWN, Common.HT_CAPTION, 0);
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            MsgBox msgbox = new MsgBox();
            msgbox.MessageBox("Question", "Exit Application?", Types.YesNo, Icons.Question);
            msgbox.ShowDialog();
            if (msgbox.yes) 
            {
                exit = true;
            }

            else
            {
                return;
            }
        }

        private void Dashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exit)
            {
                DataBaseRequest update = new DataBaseRequest();
                bool ok = update.SetData(Common.iduser);
                if (!ok)
                {
                    MessageBox.Show("Error updating online profile, going into offline mode.", "Profile Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Common.offline = true;
                }

                //always save offline profile
                OfflineMode offline = new OfflineMode();
                offline.offline();
            }

            else
            {
                e.Cancel = true;
                return;
            }
        }
    }
}
