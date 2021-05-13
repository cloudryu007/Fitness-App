using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_App.Classes
{
    //must set these to lists (sets, reps, weight)
    public class Workout
    {
        public string exerciseName { get; set; }
        public int sets { get; set; }
        public List<int> reps { get; set; } = new List<int>();
        public List<int> weight { get; set; } = new List<int>();
    }

    public class UserWorkout
    {
        public string routine { get; set; }
        public List<Workout> workout { get; set; } = new List<Workout>();
    }

    public class AddRemoveRoutine
    {
        public bool AddRoutine(string routineName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index == -1)
            {
                //init inital workoutlist
                List<Workout> Workout = new List<Workout>();

                //add new initalized workout to static list
                Common.userWorkouts.Add(new UserWorkout
                {
                    workout = Workout,
                    routine = routineName,
                });

                if (!Common.initalize)
                {

                }

                return true;
            }

            return false;
        }

        //remove entire routine
        public bool RemoveRoutine(string routineName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                Common.userWorkouts.RemoveAt(index);
                return true;
            }

            return false;
        }

        //update the name of a routine
        public bool ModifyRoutine(string oldName, string newName)
        {
            int find = Common.userWorkouts.FindIndex(x => x.routine == oldName);
            if (find > -1)
            {
                Common.userWorkouts[find].routine = newName;
                return true;
            }

            return false;
        }

        //check if the routine name exists
        public bool Exists(string newName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == newName);
            if (index == -1)
            {
                return false;
            }

            return true;
        }
    }

    public class AddRemoveexercise
    {
        public bool Addexercise(string exerciseName, string routineName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int pos = Common.userWorkouts[index].workout.FindIndex(y => y.exerciseName == exerciseName);
                if (pos == -1)
                {
                    Workout workout = new Workout();
                    workout.exerciseName = exerciseName;
                    Common.userWorkouts[index].workout.Add(workout);
                    return true;
                }
            }

            return false;
        }

        public bool Removeexercise(string exerciseName)
        {
            foreach (var ex in Common.userWorkouts)
            {
                int index = ex.workout.FindIndex(x => x.exerciseName == exerciseName);
                if (index > -1)
                {
                    ex.workout.RemoveAt(index);
                    return true;
                }
            }

            return false;
        }

        //update name of exercise
        public bool Modifyexercise(string newName, string routineName, string oldName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == oldName);
                if (find > -1)
                {
                    Common.userWorkouts[index].workout[find].exerciseName = newName;
                    return true;
                }
            }

            return false;
        }

        //when user updates sets i.e., had 4 sets selected but changed to 5 or 3 etc...
        //we will need to update accordingly, if the previous sets was greater then the new sets, remove the appropriate sets from the bottom
        public bool UpdateSet(int set, string routineName, string workoutName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == workoutName);
                if (find > -1)
                {
                    int oldNum = Common.userWorkouts[index].workout[find].sets;
                    Common.userWorkouts[index].workout[find].sets = set;

                    //remove sets, reps, weight from our array
                    if (oldNum > set)
                    {
                        while(set != oldNum)
                        {
                            oldNum -= 1;
                            Common.userWorkouts[index].workout[find].reps.RemoveAt(Common.userWorkouts[index].workout[find].reps.Count - 1);
                            Common.userWorkouts[index].workout[find].weight.RemoveAt(Common.userWorkouts[index].workout[find].weight.Count - 1);

                            if(Common.userWorkouts[index].workout[find].sets == oldNum)
                            {
                                break;
                            }
                        }
                    }

                    //add sets, reps, weight to our array
                    else
                    {
                        while (set != oldNum)
                        {
                            oldNum += 1;
                            Common.userWorkouts[index].workout[find].reps.Add(0);
                            Common.userWorkouts[index].workout[find].weight.Add(0);

                            if (Common.userWorkouts[index].workout[find].sets == oldNum)
                            {
                                break;
                            }
                        }
                    }

                    return true;
                }
            }

            return false;
        }

        public bool UpdateReps(int reps, string routineName, string workoutName, int pos)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == workoutName);
                if (find > -1)
                {
                    Common.userWorkouts[index].workout[find].reps[pos] = reps;
                    return true;
                }
            }

            return false;
        }

        public bool UpdateWeight(int weight, string routineName, string workoutName, int pos)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == workoutName);
                if (find > -1)
                {
                    Common.userWorkouts[index].workout[find].weight[pos] = weight;
                    return true;
                }
            }

            return false;
        }

        //when the user initally selects a number of sets, initalize the reps/weights
        public bool initWorkout(string routineName, string workoutName, int sets)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == workoutName);
                if (find > -1)
                {
                    Common.userWorkouts[index].workout[find].sets = sets;
                    int r = Common.userWorkouts[index].workout[find].reps.Count;
                    int w = Common.userWorkouts[index].workout[find].weight.Count;

                    //check to see if we really need to init reps
                    if (Common.userWorkouts[index].workout[find].reps.Count != sets)
                    {
                        for (int i = r; i < sets; i++)
                        {
                            Common.userWorkouts[index].workout[find].reps.Add(0);
                        }
                    }

                    //check to see if we really need to init weights
                    if (Common.userWorkouts[index].workout[find].weight.Count != sets)
                    {
                        for (int i = w; i < sets; i++)
                        {
                            Common.userWorkouts[index].workout[find].weight.Add(0);
                        }
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
