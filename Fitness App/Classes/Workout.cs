using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_App.Classes
{
    //workout object
    public class Workout
    {
        public string exerciseName { get; set; }
        public int sets { get; set; }
        public List<int> reps { get; set; } = new List<int>();
        public List<int> weight { get; set; } = new List<int>();
        public List<Superset> supersets { get; set; } = new List<Superset>();
    }

    //superset object
    public class Superset
    {
        public string supersetName { get; set; }
        public int sets { get; set; }
        public List<int> reps { get; set; } = new List<int>();
        public List<int> weight { get; set; } = new List<int>();
    }

    //all workouts go into userworkout 
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
        public bool InitWorkout(string routineName, string workoutName, int sets)
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

        public bool AddSuperset(string routineName, string exerciseName, string supersetName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int pos = Common.userWorkouts[index].workout.FindIndex(y => y.exerciseName == exerciseName);
                if (pos > -1)
                {
                    if (Common.userWorkouts[index].workout[pos].supersets.Count > 0)
                    {
                        int find = Common.userWorkouts[index].workout[pos].supersets.FindIndex(z => z.supersetName == supersetName);
                        if(find == -1)
                        {
                            Superset super = new Superset();
                            super.supersetName = supersetName;
                            Common.userWorkouts[index].workout[pos].supersets.Add(super);
                            return true;
                        }
                    }

                    else
                    {
                        Superset super = new Superset();
                        super.supersetName = supersetName;
                        Common.userWorkouts[index].workout[pos].supersets.Add(super);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool RemoveSuperset(string routineName, string exerciseName, string supersetName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int pos = Common.userWorkouts[index].workout.FindIndex(y => y.exerciseName == exerciseName);
                if (pos > -1)
                {
                    int find = Common.userWorkouts[index].workout[pos].supersets.FindIndex(z => z.supersetName == supersetName);
                    if (find > -1)
                    {
                        Common.userWorkouts[index].workout[pos].supersets.RemoveAt(find);
                        return true;
                    }
                }
            }

            return false;
        }

        public bool ModifySuperset(string routineName, string exerciseName, string oldName, string newName)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == exerciseName);
                if (find > -1)
                {
                    int pos = Common.userWorkouts[index].workout[find].supersets.FindIndex(z => z.supersetName == oldName);
                    if (pos > -1)
                    {
                        Common.userWorkouts[index].workout[find].supersets[pos].supersetName = newName;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool UpdateSuperset(string routineName, string exerciseName, string supersetName, int sets)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int pos = Common.userWorkouts[index].workout.FindIndex(y => y.exerciseName == exerciseName);
                if (pos > -1)
                {
                    int find = Common.userWorkouts[index].workout[pos].supersets.FindIndex(z => z.supersetName == supersetName);
                    if (find > -1)
                    {
                        if (sets > 0)
                        {
                            int oldNum = Common.userWorkouts[index].workout[pos].supersets[find].sets;
                            Common.userWorkouts[index].workout[pos].supersets[find].sets = sets;

                            //remove sets, reps, weight from our array
                            if (oldNum > sets)
                            {
                                while (sets != oldNum)
                                {
                                    oldNum -= 1;
                                    Common.userWorkouts[index].workout[pos].supersets[find].reps.RemoveAt(Common.userWorkouts[index].workout[pos].supersets[find].reps.Count - 1);
                                    Common.userWorkouts[index].workout[pos].supersets[find].weight.RemoveAt(Common.userWorkouts[index].workout[pos].supersets[find].weight.Count - 1);

                                    if (Common.userWorkouts[index].workout[pos].supersets[find].sets == oldNum)
                                    {
                                        break;
                                    }
                                }
                            }

                            //add sets, reps, weight to our array
                            else
                            {
                                while (sets != oldNum)
                                {
                                    oldNum += 1;
                                    Common.userWorkouts[index].workout[pos].supersets[find].reps.Add(0);
                                    Common.userWorkouts[index].workout[pos].supersets[find].weight.Add(0);

                                    if (Common.userWorkouts[index].workout[pos].supersets[find].sets == oldNum)
                                    {
                                        break;
                                    }
                                }
                            }

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //update superset reps or weight
        public bool UpdateSupersetRW(string routineName, string exerciseName, string supersetName, int rep, int weight, int row)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == exerciseName);
                if (find > -1)
                {
                    int pos = Common.userWorkouts[index].workout[find].supersets.FindIndex(z => z.supersetName == supersetName);
                    if (pos > -1)
                    {
                        if (rep > 0)
                        {
                            Common.userWorkouts[index].workout[find].supersets[pos].reps[row] = rep;
                            return true;
                        }

                        else if(weight > 0)
                        {
                            Common.userWorkouts[index].workout[find].supersets[pos].weight[row] = weight;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        //when the user initally selects a number of sets, initalize the reps/weights
        public bool InitSuperset(string routineName, string workoutName, string superset, int sets)
        {
            int index = Common.userWorkouts.FindIndex(x => x.routine == routineName);
            if (index > -1)
            {
                int find = Common.userWorkouts[index].workout.FindIndex(x => x.exerciseName == workoutName);
                if (find > -1)
                {
                    int pos = Common.userWorkouts[index].workout[find].supersets.FindIndex(x => x.supersetName == superset);
                    if (pos > -1)
                    {
                        Common.userWorkouts[index].workout[find].supersets[pos].sets = sets;
                        int r = Common.userWorkouts[index].workout[find].supersets[pos].reps.Count;
                        int w = Common.userWorkouts[index].workout[find].supersets[pos].weight.Count;

                        //check to see if we really need to init reps
                        if (Common.userWorkouts[index].workout[find].supersets[pos].reps.Count != sets)
                        {
                            for (int i = r; i < sets; i++)
                            {
                                Common.userWorkouts[index].workout[find].supersets[pos].reps.Add(0);
                            }
                        }

                        //check to see if we really need to init weights
                        if (Common.userWorkouts[index].workout[find].supersets[pos].weight.Count != sets)
                        {
                            for (int i = w; i < sets; i++)
                            {
                                Common.userWorkouts[index].workout[find].supersets[pos].weight.Add(0);
                            }
                        }

                        return true;
                    }
                }
            }

            return false;
        }
    }
}
