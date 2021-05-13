using Fitness_App.Classes;
using Fitness_App.Screens;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Fitness_App
{
    //common name space, contains static variables that my by called
    class Common
    {
        //users data
        public static string iduser = null;
        public static string userName = null;
        public static string userPassword = null;
        public static string firstName = null;
        public static string lastName = null;
        public static string city = null;
        public static string state = null;
        public static string zip = null;
        public static string phone = null;

        //database vars
        public static string server = "localhost";
        public static string database = "main";
        public static string port = "3306";
        public static string uid = "root";
        public static string pwd = "Fall1990";

        //app vars
        public static Form activeForm { get; set; }
        public static DateTime date = DateTime.Now;
        public static string Metric = null;
        public static string weight = null;
        public static List<UserWorkout> userWorkouts = new List<UserWorkout>();
        public static List<UserWorkout> currentWorkouts = new List<UserWorkout>();
        public static bool initalize = true;
        public static bool offline = false;
        public static bool doubleBuffer;

        //encrypt & decrypt
        public static string encryptPass = "AFall!(()!*";
        [DllImport("KERNEL32.DLL", EntryPoint = "RtlZeroMemory")]
        public static extern bool ZeroMemory(IntPtr Destination, int Length);

        //sample routine/workouts
        public static List<string> sampleRoutine = new List<string>();
        public static List<SampleWorkout> sampleWorkout = new List<SampleWorkout>();

        //Dll that allows set areas to be clicked and dragged
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        //Dll that allows place holder text for text boxes
        public const int EM_SETCUEBANNER = 0x1501;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern Int32 SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);
    }
}
