using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;

namespace Labb_1._SQL
{
    internal class Actions
    {
        private static void ExitOrReturn()
        {
            while (true)//option to return to main 
            {
                Console.WriteLine("Press [enter key] to return to the main menu or [escape key} to exit the program");
                ConsoleKeyInfo keyInfo2 = Console.ReadKey();
                if (keyInfo2.Key == ConsoleKey.Enter)
                {
                    Meny.MainMeny();
                }
                else if (keyInfo2.Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("\nPlease type in the right command");
                }
            }
        }
        public static void SearchPerson(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand("SELECT FirstName, LastName, Role FROM People WHERE FirstName = @FirstName AND LastName = @LastName", connection))
            {
                Console.Write("First name to search for: ");
                string firstName = Console.ReadLine();
                cmd.Parameters.AddWithValue("@FirstName", firstName);

                Console.Write("Last name to search for: ");
                string lastName = Console.ReadLine();
                cmd.Parameters.AddWithValue("@Lastname", lastName);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string first = reader.GetString(reader.GetOrdinal("FirstName")).TrimEnd();
                        string last = reader.GetString(reader.GetOrdinal("Lastname")).TrimEnd();
                        string role = reader.GetString(reader.GetOrdinal("Role")).TrimEnd();
                        Console.WriteLine($"{first} {last} is a {role}");
                    }
                }
            }

        }
        public static void ListClasses(SqlConnection connection)
        {

            connection.Open();
            using (SqlCommand cmd = new SqlCommand("GetClasses", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine("List of all the Classes: ");
                    while (reader.Read())
                    {
                        int classId = reader.GetInt32(reader.GetOrdinal("ClassId"));
                        string className = reader.GetString(reader.GetOrdinal("ClassName"));
                        Console.WriteLine($"Class ID: {classId}\t Class name: {className}");
                    }
                }
            }
            connection.Close();

        }
        public static void ViewStudents(SqlConnection connection)
        {
            while (true)
            {
            StartOfName: Console.Clear();
                Console.WriteLine("To view students please choose from the meny\n" +
                    "[1] sorted list by first name\n" +
                    "[2] sorted list by last name\n" +
                    "[Enter key] Returns you to main meny");

                ConsoleKeyInfo keyInfo = Console.ReadKey();
                int selectedIndex;
                int selectedIndex1;
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    selectedIndex = 3;
                }
                else if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D2)
                {
                    selectedIndex = (int)(keyInfo.Key - ConsoleKey.D0); // Convert key to numeric value
                }
                else
                {

                    goto StartOfName;
                }

                string orderByName = "";    //Strings used to be used in wuerry later on the following uses a switch to sort them out 
                string orderBySort = "";


                switch (selectedIndex)
                {
                    case 1:
                        Console.Clear();
                        orderByName = "FirstName";
                        break;
                    case 2:
                        orderByName = "LastName";
                        break;
                    case 3:
                        Meny.MainMeny();
                        break;

                    default:
                        Console.WriteLine("Invalid input, please choose from the meny: ");
                        continue;
                }

            StartOfSort: Console.Clear();
                Console.WriteLine("To view students please choose from the meny\n" +
                    "[1] sorted list by Ascending\n" +
                    "[2] sorted list by Descending\n" +
                    "[enter key] Returns you to main meny");

                ConsoleKeyInfo keyInfo1 = Console.ReadKey(true);

                if (keyInfo1.Key == ConsoleKey.Enter)
                {
                    selectedIndex1 = 3;
                }
                else if (keyInfo1.Key >= ConsoleKey.D1 && keyInfo1.Key <= ConsoleKey.D7)
                {
                    selectedIndex1 = (int)(keyInfo1.Key - ConsoleKey.D0); // Convert key to numeric value
                }
                else
                {

                    goto StartOfSort;
                }

                switch (selectedIndex1)
                {
                    case 1:
                        Console.Clear();
                        orderBySort = "ASC";
                        break;
                    case 2:
                        orderBySort = "DESC";
                        break;
                    case 3:
                        Meny.MainMeny();
                        break;
                    default:
                        Console.WriteLine("Invalid input, please choose from the meny: ");
                        continue;
                }

                //querry using join to add classes related to student name 
                using (SqlCommand cmd = new SqlCommand($"SELECT Students.FirstName, Students.LastName, Classes.ClassName " +
                                                       $"FROM Students " +
                                                       $"INNER JOIN Classes ON Students.ClassId = Classes.ClassId " +
                                                       $"ORDER BY {orderByName} {orderBySort}", connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Console.WriteLine($"List of all students sorted by {orderByName}");//Prints out the querry 
                        while (reader.Read())
                        {
                            string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                            string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                            string className = reader.GetString(reader.GetOrdinal("ClassName"));
                            Console.WriteLine($"{firstName,-15} {lastName,-15} belongs to {className}'s class");
                        }
                    }
                    connection.Close();
                }
                ExitOrReturn();
            }


        }
        public static void ViewAllStaffMembers(SqlConnection connection)
        {
        StartOfStaff: Console.Clear();
            Console.WriteLine("Do you wish to view all staff members or just from a certain category?\n" +
                "[1] for all staff members\n" +
                "[2] for a certain category\n" +
                "[enter key to return to main]\n");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            int selectedIndex;

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                selectedIndex = 3;
            }
            else if (keyInfo.Key >= ConsoleKey.D1 && keyInfo.Key <= ConsoleKey.D2)
            {
                selectedIndex = (int)(keyInfo.Key - ConsoleKey.D0); // Convert key to numeric value
            }
            else
            {

                goto StartOfStaff;
            }
            switch (selectedIndex)
            {
                case 1:

                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetStaffWithRole", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.Clear();
                            Console.WriteLine("List of all the staffmembers with their roles: ");
                            while (reader.Read())
                            {
                                string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                string role = reader.GetString(reader.GetOrdinal("RoleName"));
                                Console.WriteLine($"{firstName,-15} {lastName,-15}  {role}");
                                Console.WriteLine("press [space bar] to try again or enter to return to main");
                                ConsoleKeyInfo keyInfo2 = Console.ReadKey(true);
                                if (keyInfo2.Key == ConsoleKey.Spacebar)
                                {
                                    connection.Close();
                                    goto StartOfStaff;

                                }
                                else
                                {
                                    Meny.MainMeny();
                                }
                            }
                        }
                    }
                    connection.Close();
                    ExitOrReturn();
                    break;
                case 2:
                    Console.Clear();
                    ListAllRoles(connection);
                    Console.WriteLine("Please type the roleID that you want to view");

                    ConsoleKeyInfo keyInfo1 = Console.ReadKey(true);

                    if (int.TryParse(keyInfo1.KeyChar.ToString(), out int selectedRoleId))

                    {
                        using (SqlCommand cmd = new SqlCommand($"SELECT Staff.FirstName,Staff.LastName,Role.RoleName AS Role " +
                        $"                                   FROM Staff " +
                        $"                                   JOIN Role ON Staff.RoleId = Role.RoleId" +
                        $"                                   WHERE Staff.RoleId = @SelectedRoleId;", connection))
                        {
                            cmd.Parameters.AddWithValue("@SelectedRoleId", selectedRoleId);
                            connection.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {

                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                        string staffName = reader.GetString(reader.GetOrdinal("Role"));
                                        Console.WriteLine($"{firstName,-15} {lastName,-15} belongs to {staffName}'s class");
                                        Console.WriteLine("press [space bar] to try again or enter to return to main");
                                        ConsoleKeyInfo keyInfo3 = Console.ReadKey(true);
                                        if (keyInfo3.Key == ConsoleKey.Spacebar)
                                        {
                                            connection.Close();
                                            goto StartOfStaff;
                                        }
                                        else
                                        {
                                            Meny.MainMeny();
                                        }
                                    }
                                }
                                else
                                {

                                    Console.WriteLine("No staff where found in the selected class: press [space bar] to try again or enter to return to main");
                                    ConsoleKeyInfo keyInfo4 = Console.ReadKey(true);
                                    if (keyInfo4.Key == ConsoleKey.Spacebar)
                                    {
                                        connection.Close();
                                        goto StartOfStaff;

                                    }
                                    else
                                    {
                                        Meny.MainMeny();
                                    }

                                }

                            }
                            connection.Close();

                        }
                        ExitOrReturn();
                    }


                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        Meny.MainMeny();
                    }

                    else
                    {
                        Console.WriteLine("please choose from the list");

                        //Meny.MainMeny();

                    }

                    break;
                case 3:
                    Meny.MainMeny();
                    break;

            }
        }
        public static void GetStudentsFromClasses(SqlConnection connection)
        {
            Console.Clear();
            //Användaren ska först få se en lista med alla klasser som finns,
            //sedan får användaren välja en av klasserna och då skrivs alla elever i den klassen ut.
            ListClasses(connection);

            Console.WriteLine("Please choose the class you want to view students in or press Enter to return to main meny or press enter :");

            //int selectedIndex;
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (int.TryParse(keyInfo.KeyChar.ToString(), out int selectedClassId))

            {
                using (SqlCommand cmd = new SqlCommand($"SELECT Students.FirstName, Students.LastName, Classes.ClassName " +
                $"                                   FROM Students " +
                $"                                   INNER JOIN Classes ON Students.ClassId = Classes.ClassId " +
                $"                                   WHERE Classes.classId = @ClassId;", connection))
                {
                    cmd.Parameters.AddWithValue("@ClassId", selectedClassId);
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                string className = reader.GetString(reader.GetOrdinal("ClassName"));
                                Console.WriteLine($"{firstName,-15} {lastName,-15} belongs to {className}'s class");
                            }
                        }
                        else
                        {

                            Console.WriteLine("No students where found in the selected class: press [space bar] to try again or enter to return to main");
                            ConsoleKeyInfo keyInfo1 = Console.ReadKey(true);
                            if (keyInfo1.Key == ConsoleKey.Spacebar)
                            {
                                connection.Close();
                                GetStudentsFromClasses(connection);

                            }
                            else
                            {
                                Meny.MainMeny();
                            }

                        }

                    }
                    connection.Close();

                }
                ExitOrReturn();
            }


            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Meny.MainMeny();
            }

            else
            {
                Console.WriteLine("please choose from the list");

                //Meny.MainMeny();

            }

        }
        public static void ListAllRoles(SqlConnection connection)
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("GetRoles", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine("List of all the school roles: ");
                    while (reader.Read())
                    {
                        int roleId = reader.GetInt32(reader.GetOrdinal("RoleId"));
                        string roleName = reader.GetString(reader.GetOrdinal("RoleName"));
                        Console.WriteLine($"Role ID: {roleId}\t Role name: {roleName}");
                    }
                }
            }
            connection.Close();
        }
        public static int GetRolesRowCount(SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand($"SELECT COUNT(*) FROM Role", connection))
            {
                connection.Open();
                int count = (int)cmd.ExecuteScalar();
                connection.Close();

                return count;
            }
        }
        public static void GetGrades(SqlConnection connection)
        {

            Console.Clear();
            ListAlStudents(connection);

            Console.WriteLine("Please type the studentId that you want to view or press [Enter key to return]");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (int.TryParse(keyInfo.KeyChar.ToString(), out int selectedPersonId))
            {
                List<string> results = new List<string>();

                using (SqlCommand cmd = new SqlCommand($"SELECT S.FirstName, S.LastName, C.CourseName, G.GradeValue, G.GradeDate " +
                     $"FROM Students S " +
                     $"JOIN Courses C ON S.PersonId = C.PersonId " +
                     $"JOIN Grades G ON S.PersonId = G.PersonId AND C.CourseId = G.CourseId " +
                     $"WHERE S.PersonId = @SelectedPersonId", connection))
                {
                    cmd.Parameters.AddWithValue("@SelectedPersonId", selectedPersonId);
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                                int gradeValue = reader.GetInt32(reader.GetOrdinal("GradeValue"));
                                results.Add($"{firstName,-15} {lastName,-15} {courseName,-15} got the grade {gradeValue} on {reader.GetDateTime(reader.GetOrdinal("GradeDate")).ToString("yyyy-MM-dd")}");
                            }
                        }
                    }
                    connection.Close();
                }

                foreach (var result in results)
                {
                    Console.WriteLine(result);
                }
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Meny.MainMeny();
            }

            else
            {
                Console.WriteLine("please choose from the list");

            }

            ExitOrReturn();

        }
        public static void GetAllClasses(SqlConnection connection)
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("GetAllCourses", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine("List of all the courses: ");
                    while (reader.Read())
                    {
                        int courseId = reader.GetInt32(reader.GetOrdinal("CourseId"));
                        string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                        Console.WriteLine($"Course ID: {courseId}\t Course name: {courseName}");
                    }
                }
            }
            connection.Close();
        }
        public static void GetMedianGrades(SqlConnection connection)
        {
            StartOfMedianGrades: Console.Clear();
            GetAllClasses(connection);
            Console.WriteLine("Please type the classID that you want to view");

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            if (int.TryParse(keyInfo.KeyChar.ToString(), out int selectedCourseId))

            {
                using (SqlCommand cmd = new SqlCommand($"SELECT C.CourseId, C.CourseName, AVG(G.GradeValue) AS AverageGrade, " +
                    $"                                  MAX(G.GradeValue) AS HighestGrade, MIN(G.GradeValue) AS LowestGrade" +
                    $"                                  FROM Courses C " +
                    $"                                  LEFT JOIN Grades G ON C.CourseId = G.CourseId" +
                    $"                                  WHERE C.CourseId = @SelectedCourseId" +
                    $"                                  GROUP BY" +
                    $"                                  C.CourseId, C.CourseName; ", connection))
                {
                    cmd.Parameters.AddWithValue("@SelectedCourseId", selectedCourseId);
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string courseName = reader.GetString(reader.GetOrdinal("CourseName"));
                                int highestGrade = reader.GetInt32(reader.GetOrdinal("HighestGrade"));
                                int lowestGrade = reader.GetInt32(reader.GetOrdinal("LowestGrade"));
                                int medianGrade = reader.GetInt32(reader.GetOrdinal("AverageGrade"));
                                Console.WriteLine($"{courseName,-5} highest grade:{highestGrade,-5} lowest grade:{lowestGrade,-10} medium grade:{medianGrade}");
                                Console.WriteLine("press [space bar] to try again or enter to return to main");
                                ConsoleKeyInfo keyInfo3 = Console.ReadKey(true);
                                if (keyInfo3.Key == ConsoleKey.Spacebar)
                                {
                                    connection.Close();
                                    goto StartOfMedianGrades;
                                }
                                else
                                {
                                    Meny.MainMeny();
                                }
                            }
                        }
                        else
                        {

                            Console.WriteLine("No staff where found in the selected class: press [space bar] to try again or enter to return to main");
                            ConsoleKeyInfo keyInfo4 = Console.ReadKey(true);
                            if (keyInfo4.Key == ConsoleKey.Spacebar)
                            {
                                connection.Close();
                                

                            }
                            else
                            {
                                Meny.MainMeny();
                            }

                        }

                    }
                    connection.Close();

                }
                ExitOrReturn();
            }


            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Meny.MainMeny();
            }

            else
            {
                Console.WriteLine("please choose from the list");

                //Meny.MainMeny();

            }

        }
        public static void AddNewStaff(SqlConnection connection)
        {
            while (true)
            {
                ListAllRoles(connection);
                Console.Write("Enter the RoleID for the new staffmember or [enter key to return to main]: ");

                int rows = GetRolesRowCount(connection);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                if (int.TryParse(keyInfo.KeyChar.ToString(), out int selectedRoleId))
                {
                    if (selectedRoleId >= 1 && selectedRoleId <= rows)
                    {
                        Console.WriteLine();
                        Console.Write("Insert first name: ");
                        string firstName = Console.ReadLine();
                        Console.Write("Insert last name: ");
                        string lastName = Console.ReadLine();

                        using (SqlCommand cmd = new SqlCommand($"INSERT INTO Staff (FirstName, LastName, RoleId) VALUES (@FirstName, @LastName, @RoleId)", connection))
                        {
                            connection.Open();
                            cmd.Parameters.AddWithValue("@FirstName", firstName);
                            cmd.Parameters.AddWithValue("@LastName", lastName);
                            cmd.Parameters.AddWithValue("@RoleId", selectedRoleId);

                            try
                            {
                                int rowsAffected = cmd.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    Console.WriteLine("New staffmember added successfully");
                                }
                                else
                                {
                                    Console.WriteLine("No rows were added");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error: {ex.Message}");
                            }
                            connection.Close();
                        }

                        break;
                    }


                }
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    Meny.MainMeny();
                }
                else
                {
                    Console.WriteLine("Invalid RoleID. Please choose from the list");

                }
            }

            ExitOrReturn();
        }
        public static void ListAlStudents(SqlConnection connection)
        {
            connection.Open();
            using (SqlCommand cmd = new SqlCommand("GetStudentDetailsWithId", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.Clear();
                    Console.WriteLine("List of all the Classes: ");
                    while (reader.Read())
                    {
                        string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                        string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                        string className = reader.GetString(reader.GetOrdinal("ClassName"));
                        int studentId = reader.GetInt32(reader.GetOrdinal("PersonId"));
                        Console.WriteLine($"ID:{studentId,-5}{firstName,-15}{lastName,-15} {className}");
                    }
                }
            }
            connection.Close();
        }
        public static void AddNewStudent(SqlConnection connection)
        {
            
            int selectedClassId;
            ListClasses(connection);

            Console.Write("Enter the ClassID for the new student or [Enter key to return to main]: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (int.TryParse(keyInfo.KeyChar.ToString(), out selectedClassId))
            {

                Console.Write("Insert first name: ");
                string firstName = Console.ReadLine();
                Console.Write("Insert last name: ");
                string lastName = Console.ReadLine();

                string sqlQuery = "INSERT INTO Students (FirstName, LastName, ClassId) VALUES (@FirstName, @LastName, @ClassId)";
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@ClassId", selectedClassId);

                    try
                    {
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("New person added successfully");
                        }
                        else
                        {
                            Console.WriteLine("No rows where added");
                        }
                    }
                    catch (Exception ex) { Console.WriteLine($"error {ex.Message}"); }
                }
            }
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Meny.MainMeny();
            }
            else
            {
                Console.WriteLine("Invalid input, No new student added.");
            }
            connection.Close();
            ExitOrReturn();
        }

    }
}
