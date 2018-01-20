using System;
using System.Collections.Generic;
using System.IO;

namespace DictionaryTest
{
    class Program
    {
        static string[] keys;
        static string[] Keys
        {
            get
            {
                if (keys == null)
                {
                    keys = File.ReadAllLines("cities.txt");

                }

                return keys;
            }
        }

        static void Main(string[] args)
        {
            // We run both of these one time first to let the program "warm up" before we start the timing.
            CreateAndPopulateCustomDictionary();
            CreateAndPopulateStandardDictionary();

            TimeCustomDictionary();
            TimeStandardDictionary();
        }

        private static void TimeCustomDictionary()
        {
            var startTime = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                CreateAndPopulateCustomDictionary();
            }
            var endTime = DateTime.Now;

            //Console.Out.WriteLine(dict.ToString());
            int numKeys = Keys.Length * 1000;
            Console.Out.WriteLine($"[CustomDictionary] Time taken: {endTime - startTime}");
            Console.Out.WriteLine($"[CustomDictionary] Average time per set operation: {(endTime - startTime) / numKeys}");
        }

        private static void CreateAndPopulateCustomDictionary()
        {
            var dict = new CustomDictionary<string, object>();
            foreach (var key in Keys)
            {
                dict[key] = 12345;
            }
        }

        private static void TimeStandardDictionary()
        {
            var startTime = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                CreateAndPopulateStandardDictionary();
            }
            var endTime = DateTime.Now;

            //Console.Out.WriteLine(dict.ToString());
            int numKeys = Keys.Length * 1000;
            Console.Out.WriteLine($"[Dictionary] Time taken: {endTime - startTime}");
            Console.Out.WriteLine($"[Dictionary] Average time per set operation: {(endTime - startTime) / numKeys}");
        }

        private static void CreateAndPopulateStandardDictionary()
        {
            var dict = new Dictionary<string, object>();
            foreach (var key in Keys)
            {
                dict[key] = 12345;
            }
        }
    }
}
