using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SqlClient;

namespace Labb_1._SQL
{
    internal class Meny
    {
        public static void MainMeny()
        {
            
            Console.Clear();
            Console.WriteLine("Welcome to the Jonzys school. ");
            Console.WriteLine("Choose your options \n " +
                "[1] View students\n" +
                "[2] Get all students from a certain class\n" +
                "[3] Add New staff\n" +
                "[4] View Staff\n" +
                "[5] View all grades\n" +
                "[6] Add new students\n" +
                "[escape key] to exit program");
            int selectedIndex;
            int.TryParse(Console.ReadLine(), out selectedIndex);

            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.Escape) { Environment.Exit(0); }
            switch (selectedIndex)
            {
                case 1:
                    Actions.ViewStudents();
                    break;
                case 2:
                    //Get all students from a certain class
                    break;
                case 3:
                    //Add new staff
                    break;
                case 4:
                    //View staff
                    break;
                case 5:
                    //View all grades
                    break;
                case 6:
                    //Add new students
                    break;

            }
        }
        
    }
}
