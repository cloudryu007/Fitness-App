using Fitness_App.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_App.Screens
{
    public partial class Routines : Form
    {
        public List<FlowLayoutPanel> routineFlows { get; set; } = new List<FlowLayoutPanel>();
        public List<string> routineNames = new List<string>();

        public Routines()
        {
            //initalize some varaibles and controls
            InitializeComponent();
            DoubleBuffered = Common.doubleBuffer;
            Init();

            //button handle event
            addRoutineBtn.Click += createRoutine;
        }

        private void Init()
        {
            addDummy(routineLayout);

            Panel dataBorder = new Panel();
            dataBorder.Name = "borderPanel";
            dataBorder.Width = 2;
            dataBorder.BackColor = Color.FromArgb(37, 39, 77);
            dataBorder.Visible = true;
            dataBorder.BringToFront();
            dataBorder.Dock = DockStyle.Left;
            dataPanel.Controls.Add(dataBorder);

            //see if we have data to display
            if (Common.userWorkouts.Count > 0)
            {
                foreach (var routine in Common.userWorkouts)
                {
                    string routineName = routine.routine;
                    var workout = routine.workout;
                    createRoutineObj(routineName);
                    displayRoutineObj(routineName);

                    foreach (var exercise in workout)
                    {
                        newWorkoutObj(exercise.exerciseName, exercise);
                    }
                }
            }

            Common.initalize = false;
            foreach(Button b in routineLayout.Controls.OfType<Button>())
            {
                b.PerformClick();
                break;
            }
        }

        //method will create a new flowlayout & button for new routine 
        private void createRoutine(object sender, EventArgs e)
        {
            InputDialog add = new InputDialog();
            add.setPlaceHolderText = "Enter routine name, i.e. Push Day or Leg Day";
            add.setDialogTitle = "New Routine";
            add.isRoutine = true;
            add.ShowDialog();

            if (string.IsNullOrEmpty(add.userInput)) { return; }
            createRoutineObj(add.userInput);
        }

        private void createRoutineObj(string name)
        {
            string rName = name;

            //create new button for the new routine
            Button newRoutine = new Button();
            newRoutine.Name = "newRoutine_" + rName;
            newRoutine.Text = rName;
            newRoutine.Size = new Size(routineLayout.Width - 7, 25);
            newRoutine.ForeColor = Color.White;
            newRoutine.BackColor = Color.FromArgb(37, 39, 77);
            newRoutine.FlatStyle = FlatStyle.Flat;
            newRoutine.Click += displayRoutine;
            routineLayout.Controls.Add(newRoutine);

            //create new flowlayout for user to use/see
            FlowLayoutPanel flow = new FlowLayoutPanel();
            flow.Name = "flow_" + rName;
            flow.FlowDirection = FlowDirection.TopDown;
            flow.Dock = DockStyle.Fill;
            flow.AutoScroll = true;
            flow.WrapContents = false;
            flow.MouseClick += setFocus;
            addDummy(flow);

            //create table layout panel for Workout name and delete/create buttons
            TableLayoutPanel flowTable = new TableLayoutPanel();
            flowTable.Name = "flowTable_" + rName;
            flowTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 90F));
            flowTable.Size = new Size(dataPanel.Width - 15, 100);
            flowTable.MouseClick += setFocus;

            //workout name
            TextBox routineName = new TextBox();
            routineName.Name = "routineName_" + rName;
            routineName.BorderStyle = BorderStyle.None;
            routineName.Anchor = (AnchorStyles.Top);
            routineName.ForeColor = Color.FromArgb(37, 39, 77);
            routineName.BackColor = Color.White;
            routineName.Font = new Font("Segoe UI", 15.0f, FontStyle.Bold);
            routineName.Text = rName;
            routineName.ReadOnly = true;
            routineName.KeyPress += EnterPress;
            routineName.LostFocus += ChangeFocus;
            routineName.MouseClick += EnableEdit;
            routineName.Margin = new Padding(100, 0, 0, 0);
            AutoSizeTextBox(routineName);

            //delete button
            Button deleteRoutine = new Button();
            deleteRoutine.Name = "deleteRoutine_" + rName;
            deleteRoutine.Text = "Delete Routine";
            deleteRoutine.Size = new Size(105, 25);
            deleteRoutine.ForeColor = Color.White;
            deleteRoutine.BackColor = Color.FromArgb(37, 39, 77);
            deleteRoutine.FlatStyle = FlatStyle.Flat;
            deleteRoutine.Click += delete;

            //create button
            Button addWorkoutBtn = new Button();
            addWorkoutBtn.Name = "addWorkoutBtn_" + rName;
            addWorkoutBtn.Text = "Add Workout";
            addWorkoutBtn.Size = new Size(105, 25);
            addWorkoutBtn.ForeColor = Color.White;
            addWorkoutBtn.BackColor = Color.FromArgb(37, 39, 77);
            addWorkoutBtn.FlatStyle = FlatStyle.Flat;
            addWorkoutBtn.Click += newWorkout;

            //add controls to table layout panel, then add table layout panel to flow layout panel
            flowTable.Controls.Add(routineName, 0, 0);
            flowTable.Controls.Add(addWorkoutBtn, 1, 0);
            flowTable.Controls.Add(deleteRoutine, 1, 1);
            flow.Controls.Add(flowTable);

            //keep list of routines to select on the fly
            routineFlows.Add(flow);
        }

        //display the user request routine 
        private void displayRoutine(object sender, EventArgs e)
        {
            string name = ((Control)sender).Name;
            displayRoutineObj(name);
        }

        private void displayRoutineObj(string routineName)
        {
            //clear out main data panel
            foreach (FlowLayoutPanel remove in dataPanel.Controls.OfType<FlowLayoutPanel>())
            {
                dataPanel.Controls.Remove(remove);
            }

            //now grab the newly clicked routine's panel for display
            string name = routineName;
            string rName = name;
            if (name.Contains("_"))
            {
                rName = name.Split('_').Last();
            }

            foreach (FlowLayoutPanel flow in routineFlows)
            {
                //find our matching panel
                string pName = flow.Name.Split('_').Last();
                if (rName == pName)
                {
                    dataPanel.Controls.Add(flow);
                    break;
                }
            }
        }

        //create new workout
        private void newWorkout(object sender, EventArgs e)
        {
            FlowLayoutPanel main = null;
            string routineName = null;
            foreach (FlowLayoutPanel f in dataPanel.Controls.OfType<FlowLayoutPanel>())
            {
                if (f.Name.Contains("flow_"))
                {
                    main = f;
                    routineName = f.Name.Split('_').Last();
                    break;
                }
            }

            InputDialog add = new InputDialog();
            add.setPlaceHolderText = "Enter Workout name, i.e. Incline Bench or Barbell Row";
            add.setDialogTitle = "New Workout";
            add.isWorkout = true;
            add.routineName = routineName;
            add.ShowDialog();

            if (string.IsNullOrEmpty(add.userInput)) { return; }
            newWorkoutObj(add.userInput, null);
        }

        //create new workout
        private void newWorkoutObj(string name, Workout exercise)
        {
            FlowLayoutPanel main = null;
            string routineName = null;
            foreach (FlowLayoutPanel f in dataPanel.Controls.OfType<FlowLayoutPanel>())
            {
                if (f.Name.Contains("flow_"))
                {
                    main = f;
                    routineName = f.Name.Split('_').Last();
                    break;
                }
            }

            if (exercise == null)
            {
                string wName = name;
                WorkoutRoutine workout = new WorkoutRoutine();
                workout.TopLevel = false;
                workout.workoutName = wName;
                workout.routineName = routineName;
                workout.FormBorderStyle = FormBorderStyle.None;
                workout.Dock = DockStyle.Fill;
                main.Controls.Add(workout);
                workout.Show();
            }

            else
            {
                string wName = name;
                WorkoutRoutine workout = new WorkoutRoutine();
                workout.TopLevel = false;
                workout.workoutName = wName;
                workout.routineName = routineName;
                workout.setData = true;
                workout.setSets = exercise.sets;
                workout.setReps = exercise.reps;
                workout.setWeight = exercise.weight;
                workout.FormBorderStyle = FormBorderStyle.None;
                workout.Dock = DockStyle.Fill;
                main.Controls.Add(workout);
                workout.Show();
            }
        }

        //update the name of the routine
        private void adjustRoutineName(TextBox routine)
        {
            string oldName = routine.Name.Split('_').Last();
            string newName = routine.Text;

            //same name, do nothing
            if (oldName == newName)
            {
                routine.ReadOnly = true;
                dataPanel.Focus();
                return;
            }

            //check if the name already exists
            else
            {
                AddRemoveRoutine check = new AddRemoveRoutine();
                bool exists = check.Exists(newName);
                if (exists)
                {
                    routine.Text = oldName; dataPanel.Focus(); routine.ReadOnly = true;
                    MsgBox msgbox = new MsgBox();
                    msgbox.MessageBox("Information", newName + " routine already exists!", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;

                }
            }

            //update main textboxes text + name
            foreach (Button routineButton in routineLayout.Controls.OfType<Button>())
            {
                string bName = routineButton.Name.Split('_').Last();
                if (oldName == bName)
                {
                    string orignalText = routineButton.Text;
                    AddRemoveRoutine modifyRoutine = new AddRemoveRoutine();
                    modifyRoutine.ModifyRoutine(oldName, newName);
                    routineButton.Text = newName;
                    routineButton.Name = "newRoutine_" + newName;
                    routine.Name = "routineName_" + newName;
                    AutoSizeTextBox((TextBox)routine);
                    break;
                }
            }

            FlowLayoutPanel flow = null;
            //adjust all controls names
            foreach (FlowLayoutPanel f in dataPanel.Controls.OfType<FlowLayoutPanel>())
            {
                if (f.Name == "flow_" + oldName)
                {
                    f.Name = "flow_" + newName;
                    flow = f;

                    foreach (TableLayoutPanel table in f.Controls.OfType<TableLayoutPanel>())
                    {
                        if (table.Name == "flowTable_" + oldName)
                        {
                            table.Name = "flowTable_" + newName;
                            foreach (Button delete in table.Controls.OfType<Button>())
                            {
                                if (delete.Name == "deleteRoutine_" + oldName)
                                {
                                    delete.Name = "deleteRoutine_" + newName;
                                    break;
                                }

                                else if (delete.Name == "addWorkoutBtn_" + oldName)
                                {
                                    delete.Name = "addWorkoutBtn_" + newName;
                                }
                            }

                            break;
                        }
                    }

                    break;
                }
            }

            //adjust routine name
            foreach(Control workout in flow.Controls.OfType<WorkoutRoutine>())
            {
                if(((WorkoutRoutine)workout).routineName == oldName)
                {
                    ((WorkoutRoutine)workout).routineName = newName;
                    break;
                }
            }

            dataPanel.Focus();
            routine.ReadOnly = true;
            Refresh();
        }

        //when user presses enter after changing name
        private void EnterPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
                adjustRoutineName((TextBox)sender);
            }
        }

        //if the user tabs or clicks off after changing routine name
        private void ChangeFocus(object sender, EventArgs e)
        {
            adjustRoutineName((TextBox)sender);
        }

        //should the user click the textbox, enable editing
        private void EnableEdit(object sender, EventArgs e)
        {
            TextBox text = sender as TextBox;
            text.ReadOnly = false;
            text.Focus();
            Refresh();
        }

        private void setFocus(object sender, EventArgs e)
        {
            dataPanel.Focus();
            Refresh();
        }

        //add a dummy panel for spacing, usually at the header
        private void addDummy(FlowLayoutPanel flow)
        {
            Panel dummy = new Panel();
            dummy.Name = "dummyPanel";
            dummy.Size = new Size(20, 20);
            flow.Controls.Add(dummy);
        }

        //adjust width of textboxes based on text
        private void AutoSizeTextBox(TextBox txt)
        {
            const int x_margin = 0;
            const int y_margin = 2;
            Size size = TextRenderer.MeasureText(txt.Text, txt.Font);
            if (size.Width == 0) { size.Width = 150; }
            txt.ClientSize = new Size(size.Width + x_margin, size.Height + y_margin);
            txt.SelectionStart = 0;
        }

        private void AdjustWhileTyping(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            const int x_margin = 0;
            const int y_margin = 10;
            Size size = TextRenderer.MeasureText(txt.Text, txt.Font);
            if (size.Width == 0) { size.Width = 150; }
            txt.ClientSize = new Size(size.Width + x_margin, size.Height + y_margin);
        }

        //delete routine
        private void delete(object sender, EventArgs e)
        {
            string name = ((Button)sender).Name;
            string routineName = name.Split('_').Last();
            FlowLayoutPanel flow = new FlowLayoutPanel();

            foreach (FlowLayoutPanel panel in dataPanel.Controls.OfType<FlowLayoutPanel>())
            {
                flow = panel;
                break;
            }

            MsgBox msgbox = new MsgBox();
            msgbox.MessageBox("Question", "Do you want to delete routine " + routineName + "?", Types.YesNo, Icons.Warning);
            msgbox.ShowDialog();

            if (msgbox.yes)
            {
                dataPanel.Controls.Remove(flow);
                foreach (Button btn in routineLayout.Controls.OfType<Button>())
                {
                    if (btn.Name == "newRoutine_" + routineName)
                    {
                        routineLayout.Controls.Remove(btn);
                        break;
                    }
                }

                foreach (FlowLayoutPanel f in routineFlows)
                {
                    if (f.Name == "flow_" + routineName)
                    {
                        routineFlows.Remove(f);
                        break;
                    }
                }

                AddRemoveRoutine remove = new AddRemoveRoutine();
                bool removed = remove.RemoveRoutine(routineName);
                if (!removed)
                {
                    //something strange while attmepting to remove it
                    msgbox = new MsgBox();
                    msgbox.MessageBox("Information", "Error while trying to remove " + routineName + " routine.", Types.OK, Icons.Information);
                    msgbox.ShowDialog();
                    return;
                }
            }
        }
    }
}

