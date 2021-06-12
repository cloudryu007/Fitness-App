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
    public partial class Workouts : Form
    {
        //local vars
        List<stats> Stats = new List<stats>();
        bool save = false;

        public Workouts()
        {
            InitializeComponent();
            DoubleBuffered = Common.doubleBuffer;
        }

        //on screen load
        private void Workouts_Load(object sender, EventArgs e)
        {
            routineCB.Items.Add("Select Routine");
            foreach (var item in Common.userWorkouts)
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
            routineCB.SelectedIndex = 0;
            flowPanel.FlowDirection = FlowDirection.TopDown;
            tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.AddRows;
        }

        //user selected workout event handler
        private void routineCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (routineCB.SelectedIndex > -1 && routineCB.SelectedIndex > 0)
            {
                if(flowPanel.Controls.Count > 1)
                {
                    save = false;
                    foreach (ExercisePanel ex in flowPanel.Controls.OfType<ExercisePanel>())
                    {
                        //check if exercise has been completed or not setup correctly
                        if (ex.completed || ex.noStats)
                        {
                            continue;
                        }

                        //if not completed check to see if any changes were made, if none made we can exit without issues
                        for(int c = 0; c < ex.Stats.sets; c++)
                        {
                            if ((ex.Stats.reps[c] != ex.Original.reps[c]) || (ex.Stats.weight[c] != ex.Original.weight[c]))
                            {
                                if (!save)
                                {
                                    MsgBox msgbox = new MsgBox();
                                    msgbox.MessageBox("Question", "Save current exercise " + ex.Stats.exerciseName + "?", Types.YesNo, Icons.Question);
                                    msgbox.ShowDialog();
                                    if (msgbox.yes)
                                    {
                                        processDB(ex.Stats, ex.notes);
                                    }
                                }

                                else
                                {
                                    processDB(ex.Stats, ex.notes);
                                }
                            }
                        }
                    }

                    //clear out controls
                    flowPanel.Controls.Clear();
                }

                int idx = Common.userWorkouts.FindIndex(i => i.routine == routineCB.SelectedItem.ToString());
                if (idx > -1)
                {
                    foreach (var workout in Common.userWorkouts[idx].workout)
                    {
                        stats stats = SetStats(workout, false);
                        Stats.Add(stats);

                        //create object panel for workout
                        ExercisePanel exercise = new ExercisePanel();
                        exercise.TopLevel = false;
                        exercise.Stats = stats;
                        exercise.Padding = new Padding(25, 0, 0, 0);
                        flowPanel.Controls.Add(exercise);
                        exercise.Show();

                        //create object panel for superset (if applicable)
                        if(workout.supersets.Count > 0)
                        {
                            foreach(var superset in workout.supersets)
                            {
                                stats = new stats();
                                stats.exerciseName = superset.supersetName;
                                stats.sets = superset.sets;
                                stats.greps = superset.reps;
                                stats.gweight = superset.weight;

                                for (int i = 0; i < superset.sets; i++)
                                {
                                    stats.reps.Add(0);
                                    stats.weight.Add(0);
                                }

                                int pos = getPos(workout.exerciseName);
                                exercise = new ExercisePanel();
                                exercise.TopLevel = false;
                                exercise.Stats = stats;
                                exercise.mainExercise = workout.exerciseName;
                                exercise.Padding = new Padding(25, 0, 0, 0);
                                flowPanel.Controls.Add(exercise);
                                flowPanel.Controls.SetChildIndex(exercise, pos);//insert child control before main control
                                exercise.Show();
                            }
                        }
                    }

                    //deselect cb
                    tableLayoutPanel1.Select();
                    Refresh();
                }
            }

            else
            {
                //deselect cb, reset content
                flowPanel.Controls.Clear();
                tableLayoutPanel1.Select();
                Refresh();
            }
        }

        private stats SetStats(Workout workout, bool ss)
        {
            stats stats = new stats();
            stats.exerciseName = workout.exerciseName;
            stats.sets = workout.sets;
            stats.greps = workout.reps;
            stats.gweight = workout.weight;

            for (int i = 0; i < workout.sets; i++)
            {
                stats.reps.Add(0);
                stats.weight.Add(0);
            }

            return stats;
        }

        private int getPos(string exerciseName)
        {
            int pos = 0;
            foreach (Control c in flowPanel.Controls)
            {
                if (c is WorkoutRoutine)
                {
                    var temp = c as ExercisePanel;
                    if (temp.Stats.exerciseName == exerciseName)
                    {
                        return pos + 1;//increment just below original control
                    }
                }

                pos++;
            }

            return pos;
        }

        private void processDB(stats stats, string note)
        {
            save = true;
            DataBaseRequest saveEx = new DataBaseRequest();
            bool ok = saveEx.SaveExerciseData(stats, note);
            if (!ok)
            {
                MsgBox msgbox = new MsgBox();
                msgbox.MessageBox("Error", "Error while processing user request " + saveEx.exMess, Types.OK, Icons.Error);
                msgbox.ShowDialog();
            }
        }

        //class to keep track of workout stats
        public class stats
        {
            public string exerciseName { get; set; }
            public int sets { get; set; }
            public DateTime startTime { get; set; }
            public DateTime endTime { get; set; }
            public List<int> greps { get; set; }
            public List<int> gweight { get; set; }
            public List<int> reps { get; set; } = new List<int>();
            public List<int> weight { get; set; } = new List<int>();
        }
    }
}
