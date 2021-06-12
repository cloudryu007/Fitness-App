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
    public partial class SelectExercise : Form
    {
        public string routineName { get; set; }
        public string supersetName { get; set; }
        public string exerciseName { get; set; }

        public SelectExercise()
        {
            InitializeComponent();
        }

        private void SelectExercise_Load(object sender, EventArgs e)
        {
            //set initally combobox item
            exerciseCB.Items.Add("Select Primary Exercise");
            exerciseCB.SelectedIndex = 0;

            //fill combobox with exercise for this routine
            if (Common.userWorkouts.Count > 0)
            {
                foreach (var routine in Common.userWorkouts)
                {
                    if (routineName == routine.routine)
                    {
                        var workout = routine.workout;
                        foreach (var info in workout)
                        {
                            exerciseCB.Items.Add(info.exerciseName);
                        }

                        break;
                    }
                }
            }

            //disable controls until user selects a exercise
            supersetNameTB.Enabled = false;
        }

        //event to handle combobox changes
        private void exerciseCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(exerciseCB.SelectedIndex > -1 && exerciseCB.SelectedIndex > 0)
            {
                supersetNameTB.Enabled = true;
                Common.SendMessage(supersetNameTB.Handle, Common.EM_SETCUEBANNER, 0, "Enter superset name...");
            }

            else
            {
                supersetNameTB.Enabled = false;
                supersetNameTB.Text = "";
                Common.SendMessage(supersetNameTB.Handle, Common.EM_SETCUEBANNER, 0, "");
            }
        }

        //event to handle OK request
        private void okBtn_Click(object sender, EventArgs e)
        {
            if(supersetNameTB.Text.Length > 0)
            {
                supersetName = supersetNameTB.Text;
                exerciseName = exerciseCB.SelectedItem.ToString();

                AddRemoveexercise addEx = new AddRemoveexercise();
                bool ok = addEx.AddSuperset(routineName, exerciseName, supersetName);
                while (!ok)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Input Error", "Superset exercise already exists, please enter a different exercise.", Types.OK, Icons.Warning);
                    msgbox.ShowDialog();
                    return;
                }

                this.Close();
            }

            else
            {
                MsgBox msgbox = new MsgBox();
                msgbox.MessageBox("Input Error", "Must have exercise and superset selected.", Types.OK, Icons.Warning);
                msgbox.ShowDialog();
                return;
            }
        }

        //event to handle clear request
        private void clearBtn_Click(object sender, EventArgs e)
        {
            exerciseCB.SelectedIndex = 0;
        }

        //event to handle exercise sample request
        private void sampleBtn_Click(object sender, EventArgs e)
        {
            Samples sample = new Samples();
            sample.sample(true);
            sample.ShowDialog(this);
            if (!string.IsNullOrEmpty(sample.selected))
            {
                supersetNameTB.Text = sample.selected;
            }
        }
    }
}
