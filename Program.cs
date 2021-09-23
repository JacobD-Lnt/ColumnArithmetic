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
            Func<float, float, float> sum = (a, b) => a + b;
            Func<float, float, float> sub = (a, b) => a - b;
            Func<float, float, float> mult = (a, b) => a * b;
            Func<float, float, float> div = (a, b) => a / b;
            Func<float, float> sqrt = (a) => (float)Math.Sqrt(a);
            Func<float, float> negate = (a) => -a;
            Func<float, float, float> pow = (a, b) => (float)Math.Pow(a, b);

            Action<List<Dictionary<string, float>>, Func<float, float, float>, string, string, string> doFunction = (dataList, someFunc, col1, col2, col3) => dataList.ForEach(dict => dict[col3] = someFunc(dict[col1], dict[col2]));
            Action<List<Dictionary<string, float>>, Func<float, float>, string, string> doFunctionSimple = (dataList, someFunc, col1, col2) => dataList.ForEach(dict => dict[col2] = someFunc(dict[col1]));
            Action<List<Dictionary<string, float>>, Func<float, float, float>, string, string, string> doFunctionWithNumber = (dataList, someFunc, startCol, num, destinationCol) => dataList.ForEach(dict => dict[destinationCol] = someFunc(dict[startCol], float.Parse(num)));

            var jsonData = File.ReadAllText("./data/generic.json");

            var dataList = JsonSerializer.Deserialize<List<Dictionary<string, float>>>(jsonData);

            Console.WriteLine("Data from JSON file:");
            PrintDictionaryList(dataList);

            bool firstHint = false;

            while (true)
            {
                Console.WriteLine("Please enter a command.");

                if (!firstHint)
                {
                    firstHint = true;
                    Console.WriteLine("Examples:");
                    Console.WriteLine("\"add A B C\" performs A + B => C");
                    Console.WriteLine("\"negate A B\" performs -A => B");
                    Console.WriteLine("\"mutate A mult 2 div 3\" performs (A * 2)/3 => A");
                    Console.WriteLine("\"quit\" exits program");
                }

                Console.Write("> ");
                var userCommand = Console.ReadLine();

                if (userCommand == "quit")
                {
                    break;
                }

                var splitString = userCommand.Split(" ");

                if (splitString[0] == "mutate")
                {
                    bool validCommand = true;

                    string col = "";

                    if (splitString.Length < 2)
                    {
                        validCommand = false;
                        Console.WriteLine("Invalid command.");
                    }
                    else
                    {
                        col = splitString[1];

                        if (!(col is string) || !dataList[0].ContainsKey(col))
                        {
                            validCommand = false;
                            Console.WriteLine($"Error: {col} is not a valid column.");
                        }
                    }

                    if (validCommand && col != "")
                    {
                        int stringIndex = 2;

                        while (stringIndex < splitString.Length)
                        {
                            string command = splitString[stringIndex];

                            if ((command == "add" || command == "sub" || command == "mult" || command == "div" || command == "pow") && (stringIndex == splitString.Length - 1 || !float.TryParse(splitString[stringIndex + 1], out var output2)))
                            {
                                Console.WriteLine($"Error: {command} needs a number proceeding it.");
                            }
                            else
                            {
                                if (command == "add")
                                {
                                    doFunctionWithNumber(dataList, sum, col, splitString[stringIndex + 1], col);
                                }
                                else if (command == "sub")
                                {
                                    doFunctionWithNumber(dataList, sub, col, splitString[stringIndex + 1], col);
                                }
                                else if (command == "mult")
                                {
                                    doFunctionWithNumber(dataList, mult, col, splitString[stringIndex + 1], col);
                                }
                                else if (command == "div")
                                {
                                    doFunctionWithNumber(dataList, div, col, splitString[stringIndex + 1], col);
                                }
                                else if (command == "pow")
                                {
                                    doFunctionWithNumber(dataList, pow, col, splitString[stringIndex + 1], col);
                                }
                            }

                            if (command == "sqrt")
                            {
                                doFunctionSimple(dataList, sqrt, col, col);
                            }
                            else if (command == "negate")
                            {
                                doFunctionSimple(dataList, negate, col, col);
                            }

                            stringIndex++;
                        }

                        PrintDictionaryList(dataList);
                    }
                }
                else
                {
                    if (splitString.Length == 4)
                    {
                        var col1 = splitString[1];
                        var col2 = splitString[2];
                        var col3 = splitString[3];

                        bool validCommand = true;

                        if (!(col1 is string) || !dataList[0].ContainsKey(col1))
                        {
                            validCommand = false;
                            Console.WriteLine($"Error: {col1} is not a valid column.");
                        }
                        else if (!(col2 is string) || !dataList[0].ContainsKey(col2))
                        {
                            validCommand = false;
                            Console.WriteLine($"Error: {col2} is not a valid column.");
                        }
                        else if (!(col3 is string))
                        {
                            validCommand = false;
                            Console.WriteLine($"Error: {col3} is not a valid column.");
                        }

                        if (validCommand)
                        {
                            if (splitString[0] == "add") // > add A B C
                            {
                                doFunction(dataList, sum, col1, col2, col3);
                                PrintDictionaryList(dataList);
                            }
                            else if (splitString[0] == "sub") // > sub A B C
                            {
                                doFunction(dataList, sub, col1, col2, col3);
                                PrintDictionaryList(dataList);
                            }
                            else if (splitString[0] == "mult") // > mult A B C
                            {
                                doFunction(dataList, mult, col1, col2, col3);
                                PrintDictionaryList(dataList);
                            }
                            else if (splitString[0] == "pow") // pow A B C
                            {
                                doFunction(dataList, pow, col1, col2, col3);
                                PrintDictionaryList(dataList);
                            }
                            else if (splitString[0] == "div") // div A B C
                            {
                                doFunction(dataList, div, col1, col2, col3);
                                PrintDictionaryList(dataList);
                            }
                            else
                            {
                                Console.WriteLine("Invalid command.");
                            }
                        }
                    }
                    else if (splitString.Length == 3)
                    {
                        var col1 = splitString[1];
                        var col2 = splitString[2];

                        bool validCommand = true;

                        if (!(col1 is string) || !dataList[0].ContainsKey(col1))
                        {
                            validCommand = false;
                            Console.WriteLine($"Error: {col1} is not a valid column.");
                        }
                        else if (!(col2 is string) || !dataList[0].ContainsKey(col2))
                        {
                            validCommand = false;
                            Console.WriteLine($"Error: {col2} is not a valid column.");
                        }

                        if (validCommand)
                        {
                            if (splitString[0] == "sqrt") // > sqrt A B
                            {
                                doFunctionSimple(dataList, sqrt, col1, col2);
                                PrintDictionaryList(dataList);
                            }
                            else if (splitString[0] == "negate") // > negate A B
                            {
                                doFunctionSimple(dataList, negate, col1, col2);
                                PrintDictionaryList(dataList);
                            }
                            else
                            {
                                Console.WriteLine("Invalid command.");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid command.");
                    }
                }
            }
        }

        public static void PrintDictionaryList(List<Dictionary<string, float>> dataList)
        {
            foreach (Dictionary<string, float> dict in dataList)
            {
                int index = 0;

                foreach (KeyValuePair<string, float> pair in dict)
                {
                    index++;
                    Console.Write($"{pair.Key}: {pair.Value}");

                    if (index != dict.Count)
                    {
                        Console.Write(", ");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
