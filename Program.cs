using System;
using System.Collections.Generic;
using System.Text;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace FakeUserApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
                    theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            Log.Information("Application started.");

            var generator = new FakeUserGenerator();

            var sessionHistory = new List<User>();
            bool exitRequested = false;

            while (!exitRequested)
            {
                PrintMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Clear();
                        Log.Information("User selected: Option 1 (Single user generation).");

                        User singleUser = generator.Generate();
                        sessionHistory.Add(singleUser);
                        PrintUserCard(singleUser);
                        break;

                    case "2":
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("Enter the number of users to generate: ");
                        Console.ResetColor();

                        if (int.TryParse(Console.ReadLine(), out int count) && count > 0)
                        {
                            Log.Information("User selected: Option 2 (Batch generation of {Count} users).", count);
                            Console.Clear();

                            for (int i = 0; i < count; i++)
                            {
                                User user = generator.Generate();
                                sessionHistory.Add(user);
                                PrintUserCard(user);
                            }
                        }
                        else
                        {
                            Log.Warning("User provided invalid count input.");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error: Please enter a valid number greater than 0!");
                            Console.ResetColor();
                        }
                        break;

                    case "3":
                        Console.Clear();
                        Log.Information("User requested session history. Total count: {Count}", sessionHistory.Count);

                        if (sessionHistory.Count == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("No users have been generated yet in this session.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine($"=== Displaying All Generated Users ({sessionHistory.Count}) ===");
                            Console.ResetColor();

                            foreach (var savedUser in sessionHistory)
                            {
                                PrintUserCard(savedUser);
                            }
                        }
                        break;

                    case "4":
                        Log.Information("Exiting application.");
                        exitRequested = true;
                        break;

                    default:
                        Log.Warning("User entered an unknown option menu command: {Choice}", choice);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid selection! Please try again.");
                        Console.ResetColor();
                        break;
                }

                if (!exitRequested)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("\nPress any key to return to the menu...");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Log.CloseAndFlush();
        }
        static void PrintMenu()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============================================");
            Console.WriteLine("             FAKE USER GENERATOR             ");
            Console.WriteLine("=============================================");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(" 1. Generate single user");
            Console.WriteLine(" 2. Generate multiple users");
            Console.WriteLine(" 3. Show all generated users (History)");
            Console.WriteLine(" 4. Exit");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=============================================");
            Console.Write("Select an option (1-4): ");
            Console.ResetColor();
        }

        static void PrintUserCard(User user)
        {
            const int cardWidth = 65;

            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("┌" + new string('─', cardWidth - 2) + "┐");

            PrintCardLine($"User: {user.FirstName} {user.LastName}", ConsoleColor.White, cardWidth);
            PrintCardLine($"Phone: {user.PhoneNumber}", ConsoleColor.DarkYellow, cardWidth);
            PrintCardLine($"Email: {user.Email}", ConsoleColor.DarkCyan, cardWidth);
            PrintCardLine($"Address: {user.Address}", ConsoleColor.DarkGray, cardWidth);

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("└" + new string('─', cardWidth - 2) + "┘");
            Console.ResetColor();
        }

        static void PrintCardLine(string text, ConsoleColor textColor, int totalWidth)
        {
            int contentWidth = totalWidth - 4;

            if (text.Length > contentWidth)
            {
                text = text.Substring(0, contentWidth - 3) + "...";
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("│ ");
            Console.ForegroundColor = textColor;
            Console.Write(text.PadRight(contentWidth));
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(" │");
        }
    }
}
