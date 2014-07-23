using System;
using System.Diagnostics;

namespace Schematron.Benchmark
{
    class Meter
    {
        // TODO:
        // - specify a requested variance and control the number of
        //   iterations according to it

        private static readonly int DEFAULT_MEASUREMENT_TIME_MILLIS = 4000;

        /// <summary>
        /// Measures one action repeated in a loop in given numer of iterations.
        /// Report total time, average time, frequency and a comment on the
        /// action.
        /// </summary>
        /// <remarks>
        /// Note: Using lambda functions for representing the action doesn't
        /// have any measurable negative impact on performance compared to
        /// calling the action as a function inside the loop.
        /// </remarks>
        /// <param name="action">action whose performance should be measured
        /// </param>
        /// <param name="iterationCount">number of iterations</param>
        /// <param name="description">description of the action</param>
        public static void MeasureActionByIterations(
            Action action,
            int iterationCount,
            string description)
        {
            Console.WriteLine("Action description: {0}", description);
            Console.WriteLine("Number of iterations: {0}", iterationCount);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();

            for (int i = 0; i < iterationCount; i++)
            {
                action();
            }

            stopWatch.Stop();
            float avgTime = stopWatch.ElapsedMilliseconds / (float)iterationCount;
            Console.WriteLine("Total time: {0} ms", stopWatch.ElapsedMilliseconds);
            Console.WriteLine("Average time: {0} ms", avgTime);
            Console.WriteLine("Throughput: {0} / sec", 1000 / avgTime);
            Console.WriteLine();
        }

        public static void MeasureActionByTime(Action action, string description)
        {
            MeasureActionByTime(action, DEFAULT_MEASUREMENT_TIME_MILLIS, description);
        }

        /// <summary>
        /// Measures one action repeated in a loop until a time limit.
        /// Report the number of iterations, average freqency and a comment
        /// on the action.
        /// </summary>
        /// <param name="action">action whose performance should be measured
        /// </param>
        /// <param name="timeMillis">Recommended time in milliseconds</param>
        /// <param name="description"></param>
        public static void MeasureActionByTime(
            Action action,
            int timeMillis,
            string description)
        {
            Console.WriteLine("Action description: {0}", description);
            Console.WriteLine("Recommended time: {0} ms", timeMillis);
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Reset();
            stopWatch.Start();

            int iterationCount = 0;
            while (stopWatch.ElapsedMilliseconds < timeMillis)
            {
                action();
                iterationCount++;
            }

            stopWatch.Stop();
            float avgTime = stopWatch.ElapsedMilliseconds / (float)iterationCount;
            Console.WriteLine("Total time: {0} ms", stopWatch.ElapsedMilliseconds);
            Console.WriteLine("Average time: {0} ms", avgTime);
            Console.WriteLine("Number of iterations: {0}", iterationCount);
            Console.WriteLine("Throughput: {0} / sec", 1000 / avgTime);
            Console.WriteLine();
        }
    }
}
