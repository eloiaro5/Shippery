using MySql.Data.MySqlClient;
using System;

namespace Prova
{
    class Program
    {
        static void Main(string[] args)
        {
            MySqlConnection conn = new MySqlConnection("server=localhost;user=root;database=shippery;port=3306;password=;ssl-mode=None");
            conn.Open();
            string sql = "SELECT * FROM user WHERE username = @username";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@username", "u26973760");
            Console.WriteLine(cmd.ExecuteScalar());
        }
    }
}
