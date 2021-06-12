using Fitness_App.Classes;
using MySqlX.XDevAPI.Relational;
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
    /// <NOTE>
    /// Any changes made here should be strongly considered in WorkoutRoutine
    /// Both these process function the same
    /// </NOTE>
    public partial class Superset : Form
    {
        public string superSetName { get; set; }
        public string routineName { get; set; }
        public string exerciseName { get; set; }
        public string currentName { get; set; }
        public bool setData { get; set; }
        public int setSets { get; set; }
        public List<int> setReps { get; set; } = new List<int>();
        public List<int> setWeight { get; set; } = new List<int>();

        //reference to see if this exercise has been setup fully
        public int thisSets { get; set; }
        public List<int> thisReps { get; set; } = new List<int>();
        public List<int> thisWeights { get; set; } = new List<int>();

        //local vars
        ToolTip tooltip = new ToolTip();

        public Superset()
        {
            InitializeComponent();
            setsCB.MouseWheel += new MouseEventHandler(mouseWheel);
        }

        private void Superset_Load(object sender, EventArgs e)
        {
            nameTB.Text = superSetName;
            currentName = superSetName;

            if (setData)
            {
                if (setSets > 0 && setReps.Count != 0 && setWeight.Count != 0)
                {
                    setUserData();
                }
            }

            setToolTip("Superset with " + exerciseName, supersetLbl);
            workoutGrid.ClearSelection();
        }

        public void setUserData()
        {
            setsCB.SelectedIndex = setsCB.Items.IndexOf(setSets.ToString());
            for (int i = 0; i < setReps.Count; i++)
            {
                try
                {
                    workoutGrid.Rows[i].Cells[1].Value = setReps[i];
                    workoutGrid.Rows[i].Cells[2].Value = setWeight[i];
                }

                catch (Exception ex)
                {
                    string m = ex.Message;
                }
            }
        }

        //autosize textbox to text length
        private void AutoSizeTextBox(TextBox txt)
        {
            const int x_margin = 0;
            const int y_margin = 2;
            Size size = TextRenderer.MeasureText(txt.Text, txt.Font);
            if (size.Width == 0) { size.Width = 140; }
            txt.ClientSize = new Size(size.Width + x_margin, size.Height + y_margin);
            txt.SelectionLength = 0;
            Refresh();
        }

        //update grid accordingly, this will update the user workout array too
        private void setsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            string item = ((ComboBox)sender).SelectedItem.ToString();
            int sets = int.Parse(item);
            int rows = workoutGrid.RowCount;
            bool init = false;

            //if user first time selecting sets, initalize the array
            if (rows <= 0)
            {
                init = true;
            }

            if (sets > rows)
            {
                int cnt = 1;
                foreach (DataGridViewRow row in workoutGrid.Rows)
                {
                    cnt++;
                }

                while (sets != rows)
                {
                    workoutGrid.Rows.Add(cnt.ToString(), null, null);
                    rows = workoutGrid.RowCount;
                    cnt++;
                }
            }

            else if (rows > sets)
            {
                while (sets != rows)
                {
                    int cnt = workoutGrid.RowCount - 1;
                    workoutGrid.Rows.Remove(workoutGrid.Rows[cnt]);
                    rows = workoutGrid.RowCount;
                }
            }

            if (init)
            {
                AddRemoveexercise update = new AddRemoveexercise();
                bool updated = update.InitSuperset(routineName, exerciseName, superSetName, sets);
                if (!updated)
                {
                    //something strange while attmepting to remove it
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to update " + superSetName + ".", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }

            else
            {
                AddRemoveexercise update = new AddRemoveexercise();
                bool updated = update.UpdateSuperset(routineName, exerciseName, superSetName, sets);

                if (!updated)
                {
                    //something strange while attmepting to remove it
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to update " + superSetName + ".", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
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
            if (workoutGrid.Columns[e.ColumnIndex].Name == "repColumn")
            {
                var cell = workoutGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                int reps = int.Parse(cell);
                AddRemoveexercise update = new AddRemoveexercise();
                bool updated = update.UpdateSupersetRW(routineName, exerciseName, superSetName, reps, 0, e.RowIndex);

                if (!updated)
                {
                    //something strange while attmepting to remove it
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to update " + superSetName + " reps.", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }

            else if (workoutGrid.Columns[e.ColumnIndex].Name == "weightColumn")
            {
                var value = workoutGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                int weight = int.Parse(value);
                AddRemoveexercise update = new AddRemoveexercise();
                bool updated = update.UpdateSupersetRW(routineName, exerciseName, superSetName, 0, weight, e.RowIndex);

                if (!updated)
                {
                    //something strange while attmepting to remove it
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to update " + superSetName + " weight.", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }
        }

        //event to handle enter key press in datagridview
        private void Column1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //event to update the name should the user hit enter
        private void nameTB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                updateName();
                setsLbl.Focus();
                Refresh();
            }
        }

        //delete superset
        private void deleteBtn_Click(object sender, EventArgs e)
        {
            MsgBox msgbox = new MsgBox();
            msgbox.MessageBox("Question", "Do you want to delete workout " + superSetName + "?", Types.YesNo, Icons.Question);
            msgbox.ShowDialog();

            if (msgbox.yes)
            {
                AddRemoveexercise remove = new AddRemoveexercise();
                bool removed = remove.RemoveSuperset(routineName, exerciseName, superSetName);
                if (!removed)
                {
                    //something strange while attmepting to remove it
                    msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to remove superset " + superSetName + ".", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }

                this.Close();
            }
        }

        //event to update the name, should the user leave tb
        private void nameTB_Leave(object sender, EventArgs e)
        {
            updateName();
            Refresh();
        }

        //update the exercise name
        private void updateName()
        {
            if (nameTB.Text != currentName)
            {
                superSetName = nameTB.Text;
                AutoSizeTextBox(nameTB);
                AddRemoveexercise update = new AddRemoveexercise();
                bool updated = update.ModifySuperset(routineName, exerciseName, currentName, superSetName);
                if (!updated)
                {
                    //something strange while attmepting to remove it
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to update " + superSetName + " reps.", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                }

                currentName = superSetName;
                return;
            }
        }

        //create tooltip text for SS (superset)
        private void setToolTip(string text, Control control)
        {
            tooltip = new ToolTip();
            tooltip.AutoPopDelay = 5000;
            tooltip.InitialDelay = 1000;
            tooltip.ReshowDelay = 500;
            tooltip.ShowAlways = true;

            //Set up the ToolTip text
            tooltip.SetToolTip(control, text);
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
    }
}
