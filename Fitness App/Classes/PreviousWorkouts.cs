using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fitness_App.Classes
{
    //object array of previous workouts, this is any workout the users ever completed before
    //array is built off of completed_workout table
    class PreviousWorkouts
    {
        public string exerciseName { get; set; }
        public int sets { get; set; }
        public List<int> reps { get; set; } = new List<int>();
        public List<int> weight { get; set; } = new List<int>();
        public DateTime date { get; set; }
        public string notes { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
    }
}
