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
    public partial class Samples : Form
    {
        public string selected { get; set; }
        List<string> catageories = new List<string>();

        public Samples()
        {
            InitializeComponent();
        }

        //main process to be called when utilizing screen
        public void sample(bool isWorkout)
        {
            if (isWorkout)
            {
                infoLbl.Text = "Select Exercise Name:";
                if (Common.sampleWorkout.Count <= 0) 
                { 
                    return;
                }

                for (int i = 0; i < Common.sampleWorkout.Count; i++)
                {
                    string category = "--- " + Common.sampleWorkout[i].Category + " ---";
                    if (!sampleCB.Items.Contains(category))
                    {
                        catageories.Add(category);
                        sampleCB.Items.Add(category);
                    }

                    sampleCB.Items.Add(Common.sampleWorkout[i].Item);
                }
            }

            else
            {
                infoLbl.Text = "Select Routine Name:";
                if (Common.sampleRoutine.Count <= 0) 
                {
                    return; 
                }

                for (int i = 0; i < Common.sampleRoutine.Count; i++)
                {
                    sampleCB.Items.Add(Common.sampleRoutine[i]);
                }
            }
        }

        //combobox event handler
        private void sampleCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(sampleCB.SelectedItem != null && catageories.Contains(sampleCB.Text))
            {
                sampleCB.SelectedIndex = -1;
            }

            else
            {
                selected = sampleCB.Text;
            }
        }

        //process user request
        private void okBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selected))
            {
                MsgBox msgbox = new MsgBox();
                msgbox.MessageBox("Processing Error", "Please select a type.", Types.OK, Icons.Information);
                msgbox.ShowDialog();
                return;
            }

            this.Close();
        }

        //exit
        private void exitBtn_Click(object sender, EventArgs e)
        {
            selected = null;
            this.Close();
        }

        //some initalizing of combobox
        private void sampleCB_DrawItem(object sender, DrawItemEventArgs e)
        {
            if(e.Index == -1) 
            {
                return; 
            }

            ComboBox combo = ((ComboBox)sender);
            using (SolidBrush brush = new SolidBrush(e.ForeColor))
            {
                Font font = e.Font;
                if (catageories.Contains(combo.Items[e.Index].ToString()))
                {
                    font = new Font(font, FontStyle.Bold);
                }

                e.DrawBackground();
                e.Graphics.DrawString(combo.Items[e.Index].ToString(), font, brush, e.Bounds);
                e.DrawFocusRectangle();
            }
        }
    }
}
