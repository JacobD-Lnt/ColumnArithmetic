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
            Action<List<Dictionary<string, float>>, Func<float, float, float>, string, string, string> doFunction = (dataList, someFunc, col1, col2, col3) =>  dataList.ForEach(dict => dict[col3] = someFunc(dict[col1], dict[col2]));
            Action<List<Dictionary<string, float>>, Func<float, float>, string, string> doFunctionSimple = (dataList, someFunc, col1, col2) =>  dataList.ForEach(dict => dict[col2] = someFunc(dict[col1]));


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
                    var col1 = splitString[1];
                    var col2 = splitString[2];
                    var col3 = splitString[3];

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
                else if(splitString.Length==3)
                {
                    var col1 = splitString[1];
                    var col2 = splitString[2];

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
                else
                {
                    Console.WriteLine("Invalid command.");
                }


            }
        }

        public static void PrintDictionaryList(List<Dictionary<string, float>> dataList)
        {
            foreach (Dictionary<string, float> dict in dataList)
            {
                foreach (KeyValuePair<string, float> pair in dict)
                {
                    Console.Write($"{pair.Key}: {pair.Value} ");
                }

                Console.WriteLine();
            }
        }
    }
}
