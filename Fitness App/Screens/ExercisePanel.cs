using Fitness_App.Classes;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Fitness_App.Screens.Workouts;

namespace Fitness_App.Screens
{
    public partial class ExercisePanel : Form
    {
        public stats Stats { get; set; }
        public stats Original { get; set; } = new stats();
        public bool completed { get; set; }
        public bool noStats { get; set; }
        public string mainExercise { get; set; }
        public string notes { get; set; }


        public ExercisePanel()
        {
            InitializeComponent();
            previousCB.MouseWheel += new MouseEventHandler(mouseWheel);
            Common.SendMessage(noteTB.Handle, Common.EM_SETCUEBANNER, 0, "Enter notes...");
        }

        private void ExercisePanel_Load(object sender, EventArgs e)
        {
            //setup exercise data for user
            for(int i = 0; i < Stats.sets; i++)
            {
                workoutGrid.Rows.Add(i + 1, null, null, Stats.greps[i], Stats.gweight[i] + Common.weight);
            }

            if(workoutGrid.Rows.Count == 0)
            {
                noStats = true;
                workoutGrid.Rows.Add("Workout not setup...", " - ", " - ", " - ", " - ");           
            }

            //build local array to mark changes
            Original.exerciseName = Stats.exerciseName;
            Original.sets = Stats.sets;
            Original.greps = Stats.greps;
            Original.gweight = Stats.gweight;

            for(int i = 0; i < Original.sets; i++)
            {
                Original.reps.Add(0);
                Original.weight.Add(0);
            }

            //if superset, make it apparent
            previousCB.SelectedIndex = 0;
            exerciseLbl.Text = Stats.exerciseName;
            if (!string.IsNullOrEmpty(mainExercise))
            {
                exerciseLbl.Text = Stats.exerciseName + " Superset of " + mainExercise;
            }

            //note tb init
            notes = string.Empty;
            previousLbl.Text = "Previous Preformance - " + Stats.exerciseName;
            previousCB.SelectedIndex = 0;
        }

        //user selecting previous workout event
        private void previousCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (previousCB.SelectedIndex > -1 && previousCB.SelectedIndex > 0)
            {
                //clear previous
                previousGridView.Rows.Clear();
                exerciseLbl.Select();
                Refresh();

                string days = previousCB.SelectedItem.ToString(); days = days.Split(' ').First().Trim();
                DateTime date = DateTime.Now.AddDays(-int.Parse(days));
                var find = from PreviousWorkouts previous in Common.previousWorkouts
                           where previous.exerciseName == Stats.exerciseName
                           select previous;

                if (find.Count() > 0)
                {
                    foreach (PreviousWorkouts p in find)
                    {
                        //if date falls within range
                        if (DateTime.Compare(p.date, date) > 0)
                        {
                            string dt = p.date.ToString("MM/dd/yy");
                            string set = p.sets.ToString();
                            string rep = string.Join(",", p.reps);
                            string weight = string.Join(", ", p.weight) + Common.weight;
                            string notes = p.notes;

                            //null it out
                            if (string.IsNullOrEmpty(notes))
                            {
                                notes = "-";
                            }

                            //add to datagridview
                            previousGridView.Rows.Add(set, rep, weight, dt, notes);
                        }
                    }
                }

                else
                {
                    previousGridView.Rows.Add("No data found...", "-", "-", "-", "-");
                }
            }

            else
            {
                //select cb and continue
                exerciseLbl.Select();
                Refresh();
                return;
            }
        }


        //add some life to our datagridview - place holder text, center text etc...
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

            if (e.Value == null || e.Value == DBNull.Value || e.Value.ToString() == "0")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~(DataGridViewPaintParts.ContentForeground));
                if (e.ColumnIndex == 1)
                {
                    TextRenderer.DrawText(e.Graphics, "Enter Reps", e.CellStyle.Font, e.CellBounds, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }

                if (e.ColumnIndex == 2)
                {
                    TextRenderer.DrawText(e.Graphics, "Enter Weight", e.CellStyle.Font, e.CellBounds, SystemColors.GrayText, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                    e.Handled = true;
                }
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

        //adjust object array based on user input
        private void workoutGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            bool start = false;
            if (workoutGrid.Columns[e.ColumnIndex].Name == "repColumn")
            {
                var cell = workoutGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                int value = int.Parse(cell);
                Stats.reps[e.RowIndex] = value;
                start = true;
            }

            else if (workoutGrid.Columns[e.ColumnIndex].Name == "weightColumn")
            {
                var cell = workoutGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                int value = int.Parse(cell);
                Stats.weight[e.RowIndex] = value;
                start = true;
            }
            
            //set start value if we don't have one
            if (start && Stats.startTime == null) 
            {
                Stats.startTime = DateTime.Now;
            }
        }

        //don't allow cell adjustment if workout is completed
        private void workoutGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (workoutGrid.CurrentCell == null || workoutGrid.CurrentCell.Value == null || e.RowIndex == -1 || e.ColumnIndex <= 0) return;

            //check if rep/weght column handle accordingly
            if (workoutGrid.Columns[e.ColumnIndex].Name == "repColumn")
            {
                if (workoutGrid.Columns[e.ColumnIndex].ReadOnly == true)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Workout already saved! Cannot make adjustments", Types.OK, Icons.Exclamation);
                    msgbox.ShowDialog();
                }
            }

            else if (workoutGrid.Columns[e.ColumnIndex].Name == "weightColumn")
            {
                if (workoutGrid.Columns[e.ColumnIndex].ReadOnly == true)
                {
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Workout already saved! Cannot make adjustments", Types.OK, Icons.Exclamation);
                    msgbox.ShowDialog();
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

        //user complete exercise request event
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (noStats)
            {
                MsgBox msg = new MsgBox();
                msg.MessageBox("Information", "Please setup exercise correctly to begin and save workout.", Types.OK, Icons.Exclamation);
                msg.ShowDialog();

                return;
            }

            if (completed)
            {
                MsgBox msg = new MsgBox();
                msg.MessageBox("Information", "Exercise has already been completed!", Types.OK, Icons.Exclamation);
                msg.ShowDialog();

                return;
            }

            MsgBox msgbox = new MsgBox();
            msgbox.MessageBox("Question", "Complete exercise?", Types.YesNo, Icons.Question);
            msgbox.ShowDialog();
            if (msgbox.yes)
            {
                //deselect data grid row
                Stats.endTime = DateTime.Now;
                exerciseLbl.Select(); 
                Refresh();

                //validate all fields have been entered
                bool skip = true;
                for(int r = 0; r < workoutGrid.Rows.Count; r++)
                {
                    for(int c = 0; c < workoutGrid.Rows[r].Cells.Count; c++)
                    {
                        var cell = workoutGrid.Rows[r].Cells[c];
                        if (cell.Value == null || cell.Value == DBNull.Value || String.IsNullOrWhiteSpace(cell.Value.ToString()))
                        {
                            msgbox = new MsgBox();
                            msgbox.MessageBox("Question", "Exercise not fully complete, complete anyway?", Types.YesNo, Icons.Question);
                            msgbox.ShowDialog();
                            if (!msgbox.yes)
                            {
                                return;
                            }

                            skip = false;
                            break;
                        }
                    }

                    if (!skip)
                    {
                        break;
                    }
                }

                DataBaseRequest saveEx = new DataBaseRequest();
                bool ok = saveEx.SaveExerciseData(Stats, notes);
                if (!ok)
                {
                    msgbox = new MsgBox();
                    msgbox.MessageBox("Error", "Error while processing user request " + saveEx.exMess, Types.OK, Icons.Error);
                    msgbox.ShowDialog();
                }

                //set grid columns to RO, complete workout
                workoutGrid.Columns[1].ReadOnly = true;
                workoutGrid.Columns[2].ReadOnly = true;
                completed = true;
            }
        }

        //mousehover event over check mark (complete exercise)
        private void panel1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            //Set up the ToolTip text
            toolTip.SetToolTip(panel1, "Complete Exercise");
            panel1.Cursor = Cursors.Hand;
        }

        //mousehover event over check mark (complete exercise)
        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            panel1.Cursor = Cursors.Default;
        }

        //set tool tip for start exercise button
        private void startBtn_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            //Set up the ToolTip text
            toolTip.SetToolTip(startBtn, "Begin doing this exercise.");
        }

        //disallow scroll wheel on combobox, better user experience
        private void mouseWheel(object sender, MouseEventArgs e)
        {
            ComboBox control = (ComboBox)sender;
            if (!control.DroppedDown)
            {
                ((HandledMouseEventArgs)e).Handled = true;
            }
        }

        //event when user selects to begin the workout
        private void startBtn_Click(object sender, EventArgs e)
        {
            Stats.startTime = DateTime.Now;
            workoutGrid.Enabled = true;
            workoutGrid.Visible = true;
            panel1.Enabled = true;
            panel1.Visible = true;
            noteTB.Enabled = true;
            noteTB.Visible = true;
            previousPanel.Visible = true;
            startBtn.Visible = false;
            addSet.Visible = true;
            removeSet.Visible = true;
        }

        //add addtional set
        private void addSet_MouseEnter(object sender, EventArgs e)
        {
            addSet.Cursor = Cursors.Hand;
            addSet.ForeColor = Color.FromArgb(87, 39, 77);

            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            //Set up the ToolTip text
            toolTip.SetToolTip(addSet, "Add another set to exercise");
        }

        private void addSet_MouseLeave(object sender, EventArgs e)
        {
            addSet.Cursor = Cursors.Default;
            addSet.ForeColor = Color.FromArgb(37, 39, 77);
        }

        private void addSet_MouseClick(object sender, MouseEventArgs e)
        {
            int i = workoutGrid.Rows.Count + 1;
            workoutGrid.Rows.Add(i, null, null, 0, 0 + Common.weight);
        }

        //remove set
        private void removeSet_MouseEnter(object sender, EventArgs e)
        {
            removeSet.Cursor = Cursors.Hand;
            removeSet.ForeColor = Color.FromArgb(87, 39, 77);

            ToolTip toolTip = new ToolTip();
            toolTip.AutoPopDelay = 5000;
            toolTip.InitialDelay = 1000;
            toolTip.ReshowDelay = 500;
            toolTip.ShowAlways = true;

            //Set up the ToolTip text
            toolTip.SetToolTip(removeSet, "Remove set from exercise");
        }

        private void removeSet_MouseLeave(object sender, EventArgs e)
        {
            removeSet.Cursor = Cursors.Default;
            removeSet.ForeColor = Color.FromArgb(37, 39, 77);
        }

        private void removeSet_MouseClick(object sender, MouseEventArgs e)
        {
            if (workoutGrid.Rows.Count > 1)
            {
                workoutGrid.Rows.RemoveAt(workoutGrid.Rows.Count - 1);
            }
        }
    }
}
