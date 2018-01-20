using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

        static string[] shuffledKeys;
        static string[] ShuffledKeys
        {
            get
            {
                if (shuffledKeys == null)
                {
                    var rnd = new Random();
                    shuffledKeys = Keys.OrderBy(x => rnd.Next()).ToArray();
                }

                return shuffledKeys;
            }
        }

        static void Main(string[] args)
        {
            // We run both of these one time first to let the program "warm up" before we start the timing.
            CreateAndPopulateCustomDictionary();
            CreateAndPopulateStandardDictionary();

            TimeSettingCustomDictionary();
            TimeSettingStandardDictionary();
            TimeGettingCustomDictionary();
            TimeGettingStandardDictionary();
        }

        private static void TimeSettingCustomDictionary()
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

         private static void TimeSettingStandardDictionary()
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

        private static void TimeGettingCustomDictionary()
        {
            var dict = CreateAndPopulateCustomDictionary();

            var startTime = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                ReadFromDictionary(dict);
            }
            var endTime = DateTime.Now;

            //Console.Out.WriteLine(dict.ToString());
            int numKeys = Keys.Length * 1000;
            Console.Out.WriteLine($"[CustomDictionary] Time taken: {endTime - startTime}");
            Console.Out.WriteLine($"[CustomDictionary] Average time per get operation: {(endTime - startTime) / numKeys}");
        }

        private static void TimeGettingStandardDictionary()
        {
            var dict = CreateAndPopulateStandardDictionary();

            var startTime = DateTime.Now;
            for (int i = 0; i < 1000; i++)
            {
                ReadFromDictionary(dict);
            }
            var endTime = DateTime.Now;

            //Console.Out.WriteLine(dict.ToString());
            int numKeys = Keys.Length * 1000;
            Console.Out.WriteLine($"[Dictionary] Time taken: {endTime - startTime}");
            Console.Out.WriteLine($"[Dictionary] Average time per get operation: {(endTime - startTime) / numKeys}");
        }

       private static CustomDictionary<string, object> CreateAndPopulateCustomDictionary()
        {
            var dict = new CustomDictionary<string, object>();
            foreach (var key in Keys)
            {
                dict[key] = 12345;
            }

            return dict;
        }

        private static Dictionary<string, object> CreateAndPopulateStandardDictionary()
        {
            var dict = new Dictionary<string, object>();
            foreach (var key in Keys)
            {
                dict[key] = 12345;
            }

            return dict;
        }

        private static void ReadFromDictionary(IDictionary<string, object> dict)
        {
            foreach (var key in Keys)
            {
                var x = dict[key];
            }
        }
    }
}
