namespace TeamBuilder.Tests.Common
{
    using System;
    using System.Collections.Generic;

    public class Utilities
    {
        private static readonly Random Random = new Random();

        private static readonly HashSet<int> GivenIntegers = new HashSet<int>();

        public static string GenerateRandomString(int length)
        {
            string result = Guid.NewGuid().ToString().Substring(0, length);
            return result;
        }

        public static int GenerateRandomInteger(int minValue = 1, int maxValue = 100000)
        {
            return Random.Next(minValue, maxValue);
        }

        public static int GenerateUniqueRandomInteger()
        {
            int number = GenerateRandomInteger();

            while (GivenIntegers.Contains(number))
            {
                number = GenerateRandomInteger();
            }

            GivenIntegers.Add(number);
            return number;
        }
    }
}
