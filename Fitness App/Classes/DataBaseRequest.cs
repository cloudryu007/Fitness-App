using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_App.Classes
{
    class DataBaseRequest
    {
        public string exMess { get; set; }

        public bool GetData(string iduser)
        {
            List<string> processedRoutines = new List<string>();
            string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";

            try
            {
                //establish connection
                MySqlConnection connection = new MySqlConnection(connetionString);
                connection.Open();

                //query user, validate to let them in
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM main.workout WHERE userID = @user";
                cmd.Parameters.AddWithValue("@user", iduser);
                cmd.Connection = connection;

                //loop through selected rows
                MySqlDataReader getData = cmd.ExecuteReader();
                while (getData.Read())
                {
                    //build local array
                    string routineName = getData[(int)WorkoutColumns.routineName].ToString();
                    string exerciseName = getData[(int)WorkoutColumns.exerciseName].ToString();
                    string set = getData[(int)WorkoutColumns.sets].ToString(); 
                    string rep = getData[(int)WorkoutColumns.reps].ToString();
                    string weight = getData[(int)WorkoutColumns.weight].ToString();
                    
                    int sets = int.Parse(set);
                    List<string> reps = rep.Split(',').ToList();
                    List<string> weights = weight.Split(',').ToList();

                    //set routine name in array
                    if (!processedRoutines.Contains(routineName))
                    {
                        processedRoutines.Add(routineName);
                        AddRemoveRoutine addRt = new AddRemoveRoutine();
                        bool add = addRt.AddRoutine(routineName);
                        if (!add)
                        {
                            exMess = "Database error while collecting routine information.";
                            Dispose(cmd, connection, getData);
                            return false;
                        }
                    }

                    //set exercise name in array
                    AddRemoveexercise addEx = new AddRemoveexercise();
                    bool ok = addEx.Addexercise(exerciseName, routineName);
                    if (!ok)
                    {
                        exMess = "Database error while collecting routine information.";
                        Dispose(cmd, connection, getData);
                        return false;
                    }

                    //update sets in array
                    AddRemoveexercise addSets = new AddRemoveexercise();
                    ok = addSets.UpdateSet(sets, routineName, exerciseName);
                    if (!ok)
                    {
                        exMess = "Database error while collecting routine information.";
                        Dispose(cmd, connection, getData);
                        return false;
                    }

                    //update reps in array, on the database side, this is delimited by commas i.e., 5 sets, reps would be 12,12,10,10,8
                    for (int i = 0; i < reps.Count; i++)
                    {
                        int r = int.Parse(reps[i]);
                        AddRemoveexercise addReps = new AddRemoveexercise();
                        ok = addReps.UpdateReps(r, routineName, exerciseName, i);

                        if (!ok)
                        {
                            exMess = "Database error while collecting routine information.";
                            Dispose(cmd, connection, getData);
                            return false;
                        }
                    }

                    for (int i = 0; i < weights.Count; i++)
                    {
                        //update reps in array, on the database side, this is delimited by commas i.e., 5 sets, weight would be 120,200,225,275,275
                        int w = int.Parse(weights[i]);
                        AddRemoveexercise addWeight = new AddRemoveexercise();
                        ok = addWeight.UpdateWeight(w, routineName, exerciseName, i);

                        if (!ok)
                        {
                            exMess = "Database error while collecting routine information.";
                            Dispose(cmd, connection, getData);
                            return false;
                        }
                    }
                }

                //made it this far, database request succeeded
                Dispose(cmd, connection, getData);
                return true;
            }

            catch(Exception ex)
            {
                exMess = ex.Message;
                return false;
            }
        }

        public bool SetData(string iduser)
        {
            string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";
            try
            {
                //establish connection
                MySqlConnection connection = new MySqlConnection(connetionString);
                connection.Open();

                //query user, validate to let them in
                MySqlCommand deleteCmd = new MySqlCommand();
                deleteCmd.CommandText = "DELETE FROM main.workout WHERE userID = @userID";
                deleteCmd.Parameters.AddWithValue("@userID", iduser);
                deleteCmd.Connection = connection;
                deleteCmd.ExecuteNonQuery();
                deleteCmd.Dispose();

                if (Common.userWorkouts.Count > 0)
                {
                    foreach (var routine in Common.userWorkouts)
                    {
                        var workout = routine.workout;
                        foreach (var exercise in workout)
                        {
                            string reps = null; string weights = null;
                            MySqlCommand insertCmd = new MySqlCommand();
                            insertCmd.CommandText = "INSERT INTO workout(routineName, exerciseName, sets, reps, weight, userID) " +
                                                    "VALUES(@routineName, @exerciseName, @sets, @reps, @weight, @userID)";
                            //parameters
                            insertCmd.Parameters.AddWithValue("@routineName", routine.routine);
                            insertCmd.Parameters.AddWithValue("@exerciseName", exercise.exerciseName);
                            insertCmd.Parameters.AddWithValue("@sets", exercise.sets);
                            insertCmd.Parameters.AddWithValue("@reps", reps = getValue(exercise.reps));
                            insertCmd.Parameters.AddWithValue("@weight", weights = getValue(exercise.weight));
                            insertCmd.Parameters.AddWithValue("@userID", iduser);
                            insertCmd.Connection = connection;
                            insertCmd.ExecuteNonQuery();
                            insertCmd.Dispose();
                        }
                    }
                }

                connection.Close();
                return true;
            }

            catch (Exception ex)
            {
                exMess = ex.Message;
                return false;
            }
        }

        //return comma delimited string
        private string getValue(List<int> list)
        {
            return string.Join(",", list.Select(x => x.ToString()).ToArray());
        }

        //get sample workouts/routines from database
        public void GetSample()
        {
            string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";
            try
            {
                //establish connection
                MySqlConnection connection = new MySqlConnection(connetionString);
                connection.Open();

                //query user, validate to let them in
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM main.routine_samples";
                cmd.Connection = connection;

                //loop through selected rows
                MySqlDataReader routineData = cmd.ExecuteReader();
                while (routineData.Read())
                {
                    Common.sampleRoutine.Add(routineData[1].ToString());
                }

                cmd.Dispose();
                routineData.Dispose();

                //now get exercise samples
                cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM main.workout_samples";
                cmd.Connection = connection;

                //loop through selected rows
                MySqlDataReader workoutData = cmd.ExecuteReader();
                while (workoutData.Read())
                {
                    Common.sampleWorkout.Add(new SampleWorkout() { Category = workoutData[1].ToString(), Item = workoutData[2].ToString() });
                }

                //made it this far, database request succeeded
                Dispose(cmd, connection, workoutData);
                return;
            }

            catch (Exception ex)
            {
                exMess = ex.Message;
                return;
            }
        }

        private void Dispose(MySqlCommand cmd, MySqlConnection connection, MySqlDataReader getData)
        {
            cmd.Dispose();
            getData.Dispose();
            connection.Close();
        }
    }

    public class SampleWorkout
    {
        public string Category { get; set; }
        public string Item { get; set; }
    }

    //sql workout table columns
    public enum WorkoutColumns : int
    {
        routineName = 0,
        exerciseName = 1,
        sets = 2,
        reps = 3,
        weight = 4,
        userID = 5
    }
}
