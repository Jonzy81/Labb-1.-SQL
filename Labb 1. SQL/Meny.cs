using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Runtime.ConstrainedExecution;

namespace Labb_1._SQL
{
    internal class Meny
    {

        static string connectionString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SQL;Integrated Security=True;Pooling=False;";


        public static void MainMeny()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                Console.Clear();
                Console.WriteLine("Welcome to the Jonzys school. ");
                Console.WriteLine("Choose your options \n" +
                    "[1] View students\n" +
                    "[2] Get all students from a certain class\n" +
                    "[3] Add New staff\n" +
                    "[4] View Staff\n" +
                    "[5] View all grades\n" +
                    "[6] Get median grade values\n" +
                    "[7] Add new students\n" +
                    "[escape key] to exit program");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                int selectedIndex;
                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    selectedIndex = 8;
                }
                else if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D7)
                {
                    selectedIndex = (int)(keyInfo.Key - ConsoleKey.D0); // Convert key to numeric value
                }
                else
                {
                    Console.WriteLine("please choose from the meny");
                    Console.Clear();
                    MainMeny();
                    return;
                }

                switch (selectedIndex)
                {
                    case 1:
                        Actions.ViewStudents(connection);   //Done!!
                        break;
                    case 2:
                        Actions.GetStudentsFromClasses(connection);  //Done!!
                        break;
                    case 3:

                        Actions.AddNewStaff(connection); //Done!!
                        break;
                    case 4:
                        Actions.ViewAllStaffMembers(connection);    //DONE!!
                        //View staff
                        break;
                    case 5:
                        Actions.GetGrades(connection);
                        break;
                    case 6:
                        Actions.GetMedianGrades(connection);
                        break;
                    case 7:
                        Actions.AddNewStudent(connection);
                        break;
                    case 8:
                        Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("Please choose any key from the meny!");
                        break;

                }



            }
        }

    }
}
