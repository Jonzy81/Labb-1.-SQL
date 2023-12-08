
using System.Data.SqlClient;
namespace Labb_1._SQL
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=(localdb)\.;Initial Catalog=Labb1SQL;Integrated Security=True;Pooling=False;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {


              


            };
        }
    }
}
