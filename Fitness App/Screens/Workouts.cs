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
    public partial class Workouts : Form
    {
        List<stats> Stats = new List<stats>();
        Boolean changesMade = false;

        public Workouts()
        {
            InitializeComponent();
            DoubleBuffered = Common.doubleBuffer;
            addDummy(exercisePanel);
        }

        private void Workouts_Load(object sender, EventArgs e)
        {
            foreach(var item in Common.userWorkouts)
            {
                routineCB.Items.Add(item.routine);
            }

            Panel dataBorder = new Panel();
            dataBorder.Name = "borderPanel";
            dataBorder.Width = 2;
            dataBorder.BackColor = Color.FromArgb(37, 39, 77);
            dataBorder.Visible = true;
            dataBorder.BringToFront();
            dataBorder.Dock = DockStyle.Left;
            centerPanel.Controls.Add(dataBorder);
            dataPanel.Visible = false;
        }

        //user selected workout event handler
        private void routineCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (routineCB.SelectedIndex > -1)
            {
                if(exercisePanel.Controls.Count > 1)
                {
                    exercisePanel.Controls.Clear();
                    addDummy(exercisePanel);
                }

                int idx = Common.userWorkouts.FindIndex(i => i.routine == routineCB.SelectedItem.ToString());
                if (idx > -1)
                {
                    foreach (var workout in Common.userWorkouts[idx].workout)
                    {
                        Button exercise = new Button();
                        exercise.Name = workout.exerciseName;
                        exercise.Text = workout.exerciseName;
                        exercise.Size = new Size(exercisePanel.Width - 7, 25);
                        exercise.ForeColor = Color.White;
                        exercise.BackColor = Color.FromArgb(37, 39, 77);
                        exercise.FlatStyle = FlatStyle.Flat;
                        exercise.Click += displayexercise;
                        exercisePanel.Controls.Add(exercise);

                        Stats.Add(new stats
                        {
                            exerciseName = workout.exerciseName,
                            sets = workout.sets,
                            reps = null,
                            weight = null,
                            greps = workout.reps,
                            gweight = workout.weight
                        });
                    }
                }
            }
        }

        //event to display exercises for user
        private void displayexercise(object sender, EventArgs e)
        {
            Reset();
            string exName = ((Button)sender).Name.Replace("_", " ");
            int idx = Stats.FindIndex(i => i.exerciseName == exName);
            if(idx >= 0)
            {
                exLbl.Text = Stats[idx].exerciseName;
                for (int r = 0; r < Stats[idx].sets; r++)
                {
                    int rep = Stats[idx].greps[r]; 
                    int weight = Stats[idx].gweight[r];
                    workoutGrid.Rows.Add(r + 1, null, null, rep, weight);
                }

                //now show the user
                dataPanel.Visible = true;
            }
        }

        //give our grid some character
        private void workoutGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                return;
            }

            if (e.Value == null || e.Value == DBNull.Value)
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~(DataGridViewPaintParts.ContentForeground));
                if (e.ColumnIndex == 1)
                {
                    TextRenderer.DrawText(e.Graphics, "Enter Reps", e.CellStyle.Font, e.CellBounds, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                if (e.ColumnIndex == 2)
                {
                    TextRenderer.DrawText(e.Graphics, "Enter Weight", e.CellStyle.Font, e.CellBounds, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                }
                e.Handled = true;
            }
        }

        //user enter event handler
        private void workoutGrid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Column1_KeyPress);
            if (workoutGrid.CurrentCell.ColumnIndex == 1 || workoutGrid.CurrentCell.ColumnIndex == 2) //only allow numbers for columns 1/2
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Column1_KeyPress);
                }
            }
        }

        //keypress event
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //adjust text of weight box to users metric, i.e. US = lb, EU = kg
        private void workoutGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                return;
            }

            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (e.Value != null)
            {
                if (e.ColumnIndex == 2)
                {
                    e.Value = e.Value + " " + Common.weight;
                }
            }
        }

        //reset out fields
        private void Reset()
        {
            workoutGrid.Rows.Clear();
            workoutGrid.Refresh();
            exLbl.Text = string.Empty;
            dataPanel.Visible = false;
            return;
        }

        //add a dummy into flowlayout for padding
        private void addDummy(FlowLayoutPanel flow)
        {
            Panel dummy = new Panel();
            dummy.Name = "dummyPanel";
            dummy.Size = new Size(20, 20);
            flow.Controls.Add(dummy);
        }


        //class to keep track of workout stats
        private class stats
        {
            public string exerciseName { get; set; }
            public int sets { get; set; }
            public List<int> greps { get; set; }
            public List<int> gweight { get; set; }
            public List<int> reps { get; set; }
            public List<int> weight { get; set; }

        }
    }
}
