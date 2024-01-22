using Microsoft.Data.Sqlite;
using System.Globalization;
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
                Console.WriteLine("-------------------------------\n");

                string commandInput = Console.ReadLine();

                switch (commandInput)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!");
                        closeApp = true;
                        break;
                    case "1":
                        GetAllRecords();
                        break;
                    case "2":
                        InsertRecord();
                        break;
                    case "3":
                        DeleteRecord();
                        break;
                        //case "4":
                        //    UpdateRecord();
                        //    break;
                        //default:
                        //    console.writeline("\ninvalid command, please type a number from 0 to 4");
                        //    break;
                }
            }
        }

        private static void GetAllRecords()
        {
            Console.Clear();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM drinking_water";

                List<DrinkingWater> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tableData.Add(new DrinkingWater
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2)
                        }); 
                    }
                }
                else
                {
                    Console.WriteLine("No rows found");
                }

                connection.Close();
                Console.WriteLine("||---------------------------||");
                Console.WriteLine("      AVAILABLE RECORDS");
                Console.WriteLine("||---------------------------||");

                foreach (var drinkingWater in tableData)
                {
                    Console.WriteLine($"   {drinkingWater.Id} - {drinkingWater.Date.ToString("dd-MM-yy")} - Quantity: {drinkingWater.Quantity}");
                }
                Console.WriteLine("-------------------------------\n");
            }

        }
        private static void InsertRecord()
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

        private static void DeleteRecord()
        {
            Console.Clear();
            GetAllRecords();

            var recordId = GetNumberInput("\n\nPlease type the ID of the record you want to delete, or press 0 to return to the main menu");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"DELETE from drinking_water WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if (rowCount == 0)
                {
                    Console.WriteLine($"\n\nRecord with ID {recordId} does not exist \n\n");
                    DeleteRecord();
                }
            }
            Console.WriteLine($"\n\nRecord with ID {recordId} has been deleted.\n\n");
        }

        public static string GetDateInput() 
        {
            Console.WriteLine("\nPlease insert the date: (Format: dd-mm-yy) Type 0 to return to main menu");
            string dateInput = Console.ReadLine();

            if (dateInput == "0") GetUserInput();
            return dateInput;
        }

        public static int GetNumberInput(string message)
        {
            Console.WriteLine(message);
            string numberInput = Console.ReadLine();

            if (numberInput == "0") GetUserInput();

            int finalInput = Convert.ToInt32(numberInput);
            return finalInput;
        }

        public class DrinkingWater
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
        }
    }
}