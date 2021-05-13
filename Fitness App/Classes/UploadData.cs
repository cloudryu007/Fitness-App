using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_App.Classes
{
    //class to upload users data to database 
    class UploadData
    {
        public bool Upload()
        {
            bool success = false;
            foreach (var workout in Common.userWorkouts)
            {
                string routine = workout.routine;
                foreach(var data in workout.workout)
                {

                }
            }

            return success;
        }
    }
}
