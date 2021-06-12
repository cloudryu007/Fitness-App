using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fitness_App.Classes
{
    class ValidateLogin
    {
        public string exMess { get; set; }

        //connect to sql database to validate user login
        public bool Connect(string user, string password)
        {
            string connetionString = "Server = " + Common.server + "; Port = " + Common.port + "; Database = " + Common.database + "; Uid = " + Common.uid + "; Pwd = " + Common.pwd + ";";
            try
            {
                //establish connection
                MySqlConnection connection = new MySqlConnection(connetionString);
                connection.Open();

                //query user, validate to let them in
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = "SELECT * FROM main.user WHERE userName = @user AND userPassword = @password";
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Connection = connection;

                MySqlDataReader login = cmd.ExecuteReader();

                if (login.Read())
                {
                    //get the users information from the table
                    Common.userID = login[(int)UserColumns.iduser].ToString();
                    Common.userName = login[(int)UserColumns.userName].ToString();
                    Common.userPassword = login[(int)UserColumns.userPassword].ToString();
                    Common.firstName = login[(int)UserColumns.firstName].ToString();
                    Common.lastName = login[(int)UserColumns.lastName].ToString();
                    Common.city = login[(int)UserColumns.city].ToString();
                    Common.state = login[(int)UserColumns.state].ToString();
                    Common.zip = login[(int)UserColumns.zip].ToString();
                    Common.phone = login[(int)UserColumns.phone].ToString();

                    //close and return
                    connection.Close();
                    return true;
                }

                else
                {
                    //failed login, invalid login/password - close connection and notify user
                    connection.Close();
                    exMess = "Invalid login id or password";
                    return false;
                }
            }
            catch (Exception ex)
            {
                exMess = ex.Message;
                return false;
            }
        }
    }

    //enumerated user table columns
    public enum UserColumns : int
    {
        iduser = 0,
        userName = 1,
        userPassword = 2,
        firstName = 3,
        lastName = 4,
        city = 5,
        state = 6,
        zip = 7,
        phone = 8
    }
}
