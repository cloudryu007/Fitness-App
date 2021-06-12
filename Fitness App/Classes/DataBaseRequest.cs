using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Fitness_App.Screens.Workouts;

namespace Fitness_App.Classes
{
    class DataBaseRequest
    {
        public string exMess { get; set; }
        
        //get users exercise/workout data and build our local object
        public bool GetRoutine(string iduser)
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

                    Workout workout = new Workout();
                    workout.exerciseName = exerciseName;
                    workout.sets = sets;
                    workout.reps = reps.Select(int.Parse).ToList();
                    workout.weight = weights.Select(int.Parse).ToList();

                    //get superset related data
                    bool ok = GetSuperset(routineName, exerciseName, workout);
                    if (!ok)
                    {
                        exMess = "Database error while collecting user workout information.";
                        Dispose(cmd, connection, getData);
                        return false;
                    }

                    //accumulate local array
                    int pos = Common.userWorkouts.FindIndex(x => x.routine == routineName);
                    if (pos > -1)
                    {
                        Common.userWorkouts[pos].workout.Add(workout);
                    }

                    else
                    {
                        UserWorkout userWorkout = new UserWorkout();
                        userWorkout.routine = routineName;
                        userWorkout.workout.Add(workout);
                        Common.userWorkouts.Add(userWorkout);
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

        //get superset data into object array
        public bool GetSuperset(string routineName, string exerciseName, Workout workout)
        {
            bool ok = true;
            string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";
            
            try
            {
                //establish connection
                MySqlConnection connection = new MySqlConnection(connetionString);
                connection.Open();

                //query user, validate to let them in
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM main.superset WHERE userID = @user AND exerciseName = @exerciseName AND routineName = @routineName";
                cmd.Parameters.AddWithValue("@user", Common.userID);
                cmd.Parameters.AddWithValue("@exerciseName", exerciseName);
                cmd.Parameters.AddWithValue("@routineName", routineName);
                cmd.Connection = connection;

                //loop through selected rows
                MySqlDataReader getData = cmd.ExecuteReader();
                while (getData.Read())
                {
                    string supersetName = getData[(int)SuperSetColumns.supersetName].ToString();
                    string set = getData[(int)SuperSetColumns.sets].ToString();
                    string rep = getData[(int)SuperSetColumns.reps].ToString();
                    string weight = getData[(int)SuperSetColumns.weight].ToString();

                    int sets = int.Parse(set);
                    List<string> reps = rep.Split(',').ToList();
                    List<string> weights = weight.Split(',').ToList();

                    Superset superset = new Superset();
                    superset.supersetName = supersetName;
                    superset.sets = sets;
                    superset.reps = reps.Select(int.Parse).ToList();
                    superset.weight = weights.Select(int.Parse).ToList();
                    workout.supersets.Add(superset);
                }
            }

            catch (Exception ex)
            {
                ok = false;
                exMess = ex.Message;
            }

            return ok;
        }

        //sync appdata to DB, i.e., user exercises/workouts/completed workouts
        public bool SaveRoutine(string iduser)
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
                            //validate our reps/weight so it doesn't conflict with the database
                            //skip these, they haven't been setup correctly
                            if (exercise.sets == 0)
                            {
                                continue;
                                //if (exercise.reps.Count <= 0)
                                //{
                                //    exercise.reps.Add(0);
                                //}

                                //if (exercise.weight.Count <= 0)
                                //{
                                //    exercise.weight.Add(0);
                                //}
                            }

                            string reps = null; string weights = null;
                            MySqlCommand insertCmd = new MySqlCommand();
                            insertCmd.CommandText = "INSERT INTO workout(routineName, exerciseName, sets, reps, weight, userID) " +
                                                    "VALUES(@routineName, @exerciseName, @sets, @reps, @weight, @userID)";
                            //parameters for workout
                            insertCmd.Parameters.AddWithValue("@routineName", routine.routine);
                            insertCmd.Parameters.AddWithValue("@exerciseName", exercise.exerciseName);
                            insertCmd.Parameters.AddWithValue("@sets", exercise.sets);
                            insertCmd.Parameters.AddWithValue("@reps", reps = getValue(exercise.reps));
                            insertCmd.Parameters.AddWithValue("@weight", weights = getValue(exercise.weight));
                            insertCmd.Parameters.AddWithValue("@userID", iduser);
                            insertCmd.Connection = connection;
                            insertCmd.ExecuteNonQuery();
                            insertCmd.Dispose();

                            //remove all related supersets, we're repopulating the DB
                            deleteCmd = new MySqlCommand();
                            deleteCmd.CommandText = "DELETE FROM main.superset WHERE userID = @userID";
                            deleteCmd.Parameters.AddWithValue("@userID", iduser);
                            deleteCmd.Connection = connection;
                            deleteCmd.ExecuteNonQuery();
                            deleteCmd.Dispose();

                            foreach (var superset in exercise.supersets)
                            {
                                //validate our reps/weight so it doesn't conflict with the database
                                //skip these, they haven't been setup correctly
                                if (superset.sets == 0)
                                {
                                    //if (superset.reps.Count <= 0)
                                    //{
                                    //    superset.reps.Add(0);
                                    //}

                                    //if (superset.weight.Count <= 0)
                                    //{
                                    //    superset.weight.Add(0);
                                    //}
                                    continue;
                                }

                                insertCmd = new MySqlCommand();
                                insertCmd.CommandText = "INSERT INTO superset(supersetName, routineName, exerciseName, sets, reps, weight, userID) " +
                                                        "VALUES(@supersetName, @routineName, @exerciseName, @sets, @reps, @weight, @userID)";
                                //parameters for workout
                                insertCmd.Parameters.AddWithValue("@supersetName", superset.supersetName);
                                insertCmd.Parameters.AddWithValue("@routineName", routine.routine);
                                insertCmd.Parameters.AddWithValue("@exerciseName", exercise.exerciseName);
                                insertCmd.Parameters.AddWithValue("@sets", superset.sets);
                                insertCmd.Parameters.AddWithValue("@reps", reps = getValue(superset.reps));
                                insertCmd.Parameters.AddWithValue("@weight", weights = getValue(superset.weight));
                                insertCmd.Parameters.AddWithValue("@userID", iduser);
                                insertCmd.Connection = connection;
                                insertCmd.ExecuteNonQuery();
                                insertCmd.Dispose();
                            }
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

        //update db with completed user exercise
        public bool SaveExerciseData(stats stats, string note)
        {
            if (stats != null)
            {
                string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";
                try
                {
                    //establish connection
                    MySqlConnection connection = new MySqlConnection(connetionString);
                    connection.Open();

                    string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    string reps = null; string weights = null;
                    MySqlCommand insertCmd = new MySqlCommand();
                    insertCmd.CommandText = "INSERT INTO completed_workouts(exerciseName, sets, reps, weight, date, userID, notes, startTime, endTime) " +
                                            "VALUES(@exerciseName, @sets, @reps, @weight, @date, @userID, @notes, @startTime, @endTime)";
                    //parameters
                    insertCmd.Parameters.AddWithValue("@exerciseName", stats.exerciseName);
                    insertCmd.Parameters.AddWithValue("@sets", stats.sets);
                    insertCmd.Parameters.AddWithValue("@reps", reps = getValue(stats.reps));
                    insertCmd.Parameters.AddWithValue("@weight", weights = getValue(stats.weight));
                    insertCmd.Parameters.AddWithValue("@date", date);
                    insertCmd.Parameters.AddWithValue("@userID", Common.userID);
                    insertCmd.Parameters.AddWithValue("@notes", note);
                    insertCmd.Parameters.AddWithValue("@startTime", note);
                    insertCmd.Parameters.AddWithValue("@endTime", note);
                    insertCmd.Connection = connection;
                    insertCmd.ExecuteNonQuery();
                    insertCmd.Dispose();

                    connection.Close();

                    //since we made a db update, resync previous workouts array with DB data
                    bool ok = GetExerciseData(90);
                    if (ok)
                    {
                        return true;
                    }

                    //made it here not good...
                    return false;
                }

                catch (Exception ex)
                {
                    exMess = ex.Message;
                    return false;
                }
            }

            else
            {
                exMess = "No exercises passed in!";
                return false;
            }
        }

        //get user exercise related data
        public bool GetExerciseData(int days)
        {
            string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";
            try
            {
                if (days == 0)
                {
                    days = 90;//default to 90 days
                }

                DateTime sub = DateTime.Now.AddDays(-days);
                string start = sub.ToString("yyyy-MM-dd HH:mm:ss");
                string end = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                //establish connection
                MySqlConnection connection = new MySqlConnection(connetionString);
                connection.Open();

                //query user, validate to let them in
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM main.completed_workouts WHERE userID = @user AND date BETWEEN @start AND @end";
                cmd.Parameters.AddWithValue("@user", Common.userID);
                cmd.Parameters.AddWithValue("@start", start);
                cmd.Parameters.AddWithValue("@end", end);
                cmd.Connection = connection;

                //loop through selected rows
                MySqlDataReader getData = cmd.ExecuteReader();
                Common.previousWorkouts.Clear();//clear local array, we are repopulating it
                while (getData.Read())
                {
                    //build local array
                    string exerciseName = getData[(int)CompletedWorkoutColumns.exerciseName].ToString();
                    string set = getData[(int)CompletedWorkoutColumns.sets].ToString();
                    string rep = getData[(int)CompletedWorkoutColumns.reps].ToString();
                    string weight = getData[(int)CompletedWorkoutColumns.weight].ToString();
                    string dateTime = getData[(int)CompletedWorkoutColumns.dateTime].ToString();
                    string notes = getData[(int)CompletedWorkoutColumns.notes].ToString();

                    int sets = int.Parse(set);
                    List<string> reps = rep.Split(',').ToList();
                    List<string> weights = weight.Split(',').ToList();

                    //build array of previous workouts
                    PreviousWorkouts previousWorkouts = new PreviousWorkouts();
                    previousWorkouts.exerciseName = exerciseName;
                    previousWorkouts.sets = int.Parse(set);
                    previousWorkouts.reps = reps.Select(int.Parse).ToList();
                    previousWorkouts.weight = weights.Select(int.Parse).ToList();
                    previousWorkouts.date = DateTime.Parse(dateTime);
                    previousWorkouts.notes = notes;
                    Common.previousWorkouts.Add(previousWorkouts);
                }

                //made it this far, database request succeeded
                Dispose(cmd, connection, getData);
                return true;
            }

            catch (Exception ex)
            {
                exMess = ex.Message;
                return false;
            }
        }

        //return comma delimited as string
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

        //dispose un-need processes from memory
        private void Dispose(MySqlCommand cmd, MySqlConnection connection, MySqlDataReader getData)
        {
            cmd.Dispose();
            getData.Dispose();
            connection.Close();
        }
    }

    //object for Sample workouts
    public class SampleWorkout
    {
        public string Category { get; set; }
        public string Item { get; set; }
    }

    //enumerated sql workout table columns
    public enum WorkoutColumns : int
    {
        routineName = 1,
        exerciseName = 2,
        sets = 3,
        reps = 4,
        weight = 5,
        userID = 6
    }

    //enumerated sql completed_workout table columns
    public enum CompletedWorkoutColumns: int
    {
        exerciseName = 1,
        sets = 2,
        reps = 3,
        weight = 4,
        dateTime = 5,
        userID = 6,
        notes = 7
    }

    //enumerated sql superset table columns
    public enum SuperSetColumns  : int
    {
        supersetName = 1,
        routineName = 2,
        exerciseName = 3,
        sets = 4,
        reps = 5,
        weight = 6,
        userID = 7
    }
}
