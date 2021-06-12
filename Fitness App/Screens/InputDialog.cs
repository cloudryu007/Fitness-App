using Fitness_App.Classes;
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
    public partial class InputDialog : Form
    {
        public string setPlaceHolderText { get; set; }
        public string setDialogTitle { get; set; }
        public string userInput { get; set; }
        public bool isRoutine { get; set; }
        public bool isWorkout { get; set; }
        public string routineName { get; set; }//must be set when adding exercise

        public InputDialog()
        {
            InitializeComponent();
        }

        //onload event
        private void InputDialog_Load(object sender, EventArgs e)
        {
            this.Text = "Input Request";

            if(!string.IsNullOrEmpty(setDialogTitle))
            {
                this.Text = setDialogTitle;
            }

            if (!string.IsNullOrEmpty(setPlaceHolderText))
            {
                Common.SendMessage(inputTextBox.Handle, Common.EM_SETCUEBANNER, 0, setPlaceHolderText);
            }

            if(isRoutine && Common.sampleRoutine.Count <= 0 || isWorkout && Common.sampleWorkout.Count <= 0)
            {
                disableSample();
            }
        }

        //handle user request
        private void okBtn_Click(object sender, EventArgs e)
        {
            string text = inputTextBox.Text;
            if (!string.IsNullOrEmpty(text))
            {
                if (isRoutine)
                {
                    AddRemoveRoutine addRt = new AddRemoveRoutine();
                    bool ok = addRt.AddRoutine(text);
                    while (!ok)
                    {
                        MsgBox msgbox = new MsgBox();
                        msgbox.MessageBox("Input Error", "Routine name already exists, please enter a different routine name.", Types.OK, Icons.Warning);
                        msgbox.ShowDialog();
                        return;
                    }
                }

                else if (isWorkout)
                {
                    AddRemoveexercise addEx = new AddRemoveexercise();
                    bool ok = addEx.Addexercise(text, routineName);
                    while (!ok)
                    {
                        MsgBox msgbox = new MsgBox();
                        msgbox.MessageBox("Input Error", "Workout name already exists, please enter a different workout name.", Types.OK, Icons.Warning);
                        msgbox.ShowDialog();
                        return;
                    }
                }

                userInput = text;
                this.Close();
            }

            else
            {
                MsgBox msgbox = new MsgBox();
                msgbox.MessageBox("Input Error", "Input cannot be empty, please enter a valid input.", Types.OK, Icons.Warning);
                msgbox.ShowDialog();
                return;
            }
        }

        //clear event
        private void clearBtn_Click(object sender, EventArgs e)
        {
            inputTextBox.Clear();
        }

        //if user selects samples, display accordingly (exercise, or routines)
        private void sampleBtn_Click(object sender, EventArgs e)
        {
            Samples sample = new Samples();
            sample.sample(isWorkout);
            sample.ShowDialog(this);
            if (!string.IsNullOrEmpty(sample.selected))
            {
                inputTextBox.Text = sample.selected;
            }
        }

        //disable sample if offline mode and no offline data found for samples
        private void disableSample()
        {
            sampleBtn.Enabled = false;
            sampleBtn.BackColor = Color.Gray;
            sampleBtn.ForeColor = Color.Black;
        }
    }
}
