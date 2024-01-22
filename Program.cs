using Microsoft.Data.Sqlite;
using System.Runtime.CompilerServices;

namespace HabitTracker
{
    public class Program
    {
        static string connectionString = @"Data Source=habit-Tracker.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

                tableCmd.ExecuteNonQuery();
                connection.Close();
                GetUserInput();
            }    
        }

        static void GetUserInput()
        {
            Console.Clear();
            bool closeApp = false;
            while (!closeApp)
            {
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to close application");
                Console.WriteLine("Type 1 to view all records");
                Console.WriteLine("Type 2 to insert record");
                Console.WriteLine("Type 3 to delete record");
                Console.WriteLine("Type 4 to update record");
                Console.WriteLine("----------------------\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!");
                        closeApp = true;
                        break;
                    //case "1":
                    //    GetAllRecords();
                    //    break;
                    //case "2":
                    //    InsertRecord();
                    //    break;
                    //case "3":
                    //    DeleteRecord();
                    //    break;
                    //case "4":
                    //    UpdateRecord();
                    //    break;
                    //default:
                    //    Console.WriteLine("\nInvalid command, please type a number from 0 to 4");
                    //    break;
                }
            }
        }
        private static void Insert()
        {
            string date = GetDateInput();
            int quantityOfWater = GetNumberInput("\n\nPlease input number of glasses or unit of your choice (no decimals\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantityOfWater})";

                tableCmd.ExecuteNonQuery();
                connection.Close();
            }

        }

        internal static string GetDateInput() 
        {
            Console.WriteLine("\nPlease insert the date: (Format: dd-mm-yy) Type 0 to return to main menu");
            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();
            return dateInput;
        }

        internal static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }
    }
}