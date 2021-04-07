using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace RaftSolution
{
    class Program
    {
        static void Main()
        {
            var inputArgs = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var n = inputArgs[0];
            var k = inputArgs[1];

            var goats = Console.ReadLine()
                .Split(' ')
                .Select(int.Parse)
                .ToList();

            goats.Sort();
            goats.Reverse();

            var baseLimit = goats[0];
            var currentBest = CreateRaft(goats, k, baseLimit);

            var sumOfGoatWeighs = goats.Sum();
            var incrementAmount = goats.Min();
            var biggerRaft = baseLimit + incrementAmount;

            while (biggerRaft <= sumOfGoatWeighs)
            {
                var newRaft = CreateRaft(goats, k, biggerRaft);

                var currentRaftLimitDoesntAllowCoursesUnderOrEqualToK = newRaft == int.MaxValue;

                if (currentRaftLimitDoesntAllowCoursesUnderOrEqualToK)
                {
                    biggerRaft++;
                    continue;
                }
                if (newRaft < currentBest)
                {
                    currentBest = biggerRaft;
                }

                biggerRaft+= incrementAmount;
            }

            Console.WriteLine("Current best is: " + currentBest.ToString());
            Console.WriteLine("Time elapsed: " + stopwatch.Elapsed.TotalSeconds);
        }

        private static int CreateRaft(List<int> goats, int k, int currentLimit)
        {
            var newGoats = goats.ToList();
            var goatsPassed = new List<int>();

            var coursesCounter = 0;

            while (newGoats.Count != 0)
            {
                var resetLimit = currentLimit;
                var currentGoat = newGoats[0];

                while (currentLimit >= currentGoat)
                {

                    goatsPassed.Add(currentGoat);
                    newGoats.Remove(currentGoat);
                    currentLimit -= currentGoat;
                    currentGoat = FindNextGoat(currentLimit, newGoats, currentGoat);
                }

                currentLimit = resetLimit;
                coursesCounter++;
            }

            if (coursesCounter > k)
            {
                return int.MaxValue;
            }

            return currentLimit;
        }

        private static int FindNextGoat(int currentLimit, List<int> newGoats, int currentGoat)
        {
            for (int i = 0; i < newGoats.Count; i++)
            {
                if (currentLimit >= newGoats[i])
                {
                    currentGoat = newGoats[i];
                    break;
                }
            }

            return currentGoat;
        }
    }
}