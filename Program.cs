using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ColumnArithmetic
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("Hello World!");

            var jsonData = File.ReadAllText("./data/generic.json");

            var dataList = JsonSerializer.Deserialize<List<Dictionary<string, int>>>(jsonData);

            Console.WriteLine("Data from JSON file:");
            PrintDictinaryList(dataList);

            bool firstHint = false;

            while (true)
            {
                Console.WriteLine("Please enter a command.");

                if (!firstHint)
                {
                    firstHint = true;
                    Console.WriteLine("(add A B C: A + B => C)");
                    Console.WriteLine("(quit: exit program)");
                }

                var userCommand = Console.ReadLine();

                if (userCommand == "quit")
                {
                    break;
                }

                var splitString = userCommand.Split(" ");

                if (splitString.Length == 4)
                {
                    if (splitString[0] == "add")
                    {
                        // > add A B C

                        foreach (Dictionary<string, int> dict in dataList)
                        {
                            dict[splitString[3]] = dict[splitString[1]] + dict[splitString[2]];
                        }

                        PrintDictinaryList(dataList);
                    }
                    else if (splitString[0] == "sub")
                    {
                        // > sub A B C

                        foreach (Dictionary<string, int> dict in dataList)
                        {
                            dict[splitString[3]] = dict[splitString[1]] - dict[splitString[2]];
                        }

                        PrintDictinaryList(dataList);
                    }
                    else if (splitString[0] == "mult")
                    {
                        // > mult A B C

                        foreach (Dictionary<string, int> dict in dataList)
                        {
                            dict[splitString[3]] = dict[splitString[1]] * dict[splitString[2]];
                        }

                        PrintDictinaryList(dataList);
                    }
                    else if (splitString[0] == "pow")
                    {
                        foreach (Dictionary<string, int> dict in dataList)
                        {
                            int product = 1;

                            for (int i = 0; i < dict[splitString[2]]; i++)
                            {
                                product *= dict[splitString[1]];
                            }

                            dict[splitString[3]] = product;
                        }

                        PrintDictinaryList(dataList);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid command.");
                }

            }
        }

        public static void PrintDictinaryList(List<Dictionary<string, int>> dataList)
        {
            foreach (Dictionary<string, int> dict in dataList)
            {
                foreach (KeyValuePair<string, int> pair in dict)
                {
                    Console.Write($"{pair.Key}: {pair.Value} ");
                }

                Console.WriteLine();
            }
        }
    }
}
