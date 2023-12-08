using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;

namespace Labb_1._SQL
{
    internal class Actions
    {
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
            //connection.Open();
            using (SqlCommand cmd = new SqlCommand("GetClasses", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    Console.WriteLine("List of all the Classes: ");
                    while (reader.Read())
                    {
                        int classId = reader.GetInt32(reader.GetOrdinal("ClassId"));
                        string className = reader.GetString(reader.GetOrdinal("ClassName"));
                        Console.WriteLine($"Class ID: {classId} Class name: {className}");
                    }
                }
            }
            //connection.Close();
        }
        public static void ViewStudents(SqlConnection connection)
        {
            StartOfView: Console.Clear();
            Console.WriteLine("To view students please choose from the meny" +
                "[1] sorted list by first name" +
                "[2] sorted list by last name" +
                "[enter key] return to main meny");
            int selectedIndex;
            int.TryParse(Console.ReadLine(), out selectedIndex);
            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Enter)
            {
                selectedIndex = 3;
            }
            switch (selectedIndex)
            {
                case 1:
                    Console.Clear();
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("GetStudentDetailsSortedByFirstName", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            Console.WriteLine("List of all the Students sorted by first name: ");
                            while (reader.Read())
                            {
                                string firstName = reader.GetString(reader.GetOrdinal("FirstName"));
                                string lastName = reader.GetString(reader.GetOrdinal("LastName"));
                                string className = reader.GetString(reader.GetOrdinal("ClassName"));
                                Console.WriteLine($"{firstName,-15} {lastName,-15} belongs to {className}'s class");
                            }
                        }
                    }
                    connection.Close();
                    Console.WriteLine("Press enter to return to main meny or escape to exit program");
                    ConsoleKeyInfo keyInfo1 = Console.ReadKey();
                    if ( keyInfo1.Key == ConsoleKey.Enter) { Meny.MainMeny(); }
                    if(keyInfo1.Key==ConsoleKey.Escape) { Environment.Exit(0); }
                    else { Console.WriteLine("Please type in right command"); }
                    break;
                case 2:
                    break;
                case 3:
                    Meny.MainMeny();
                    break;
            }


        }
        public static void AddNewStudent(SqlConnection connection)
        {
            connection.Open();
            int selectedClassId;
            ListClasses(connection);
            Console.Write("Enter the ClassID for the new student: ");
            if (int.TryParse(Console.ReadLine(), out selectedClassId))
            {

                Console.Write("Insert first name: ");
                string firstName = Console.ReadLine();
                Console.Write("Insert last name: ");
                string lastName = Console.ReadLine();

                string sqlQuery = "INSERT INTO Students (FirstName, LastName, ClassId) VALUES (@FirstName, @LastName, @ClassId)";

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
            else
            {
                Console.WriteLine("Invalid input, No new student added.");
            }
            connection.Close();
        }
    }
}
