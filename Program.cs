using Microsoft.Data.Sqlite;

namespace HabitTracker
{
    public class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Data Source=habit-Tracker.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText = "";
                tableCmd.ExecuteNonQuery();
            }
        }
    }
}