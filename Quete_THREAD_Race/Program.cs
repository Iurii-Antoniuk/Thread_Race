using System;
using System.Threading;
using System.Collections.Generic;

namespace Quete_THREAD_Race
{
    class Program
    {
        private static Int32 winnerThreadId { get; set; }
        private static Int32 timeTaken { get; set; } = 0;
        private static Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            var threadStartDelegate = new ThreadStart(OnThreadStart);
            List<Thread> raceThreads = new List<Thread>();

            for (int i = 0; i < 5; i++)
            {
                Thread myThread = new Thread(threadStartDelegate);
                raceThreads.Add(myThread);
            }

            foreach (var t in raceThreads)
            {
                t.Start();
            }

            foreach (var t in raceThreads)
            {
                t.Join();
            }

            Console.WriteLine("The winner Thread Id is - {0}. It's taken {1} milliseconds to complete", winnerThreadId, timeTaken);
        }

        private static void OnThreadStart()
        {
            int sleepTime = new System.Random().Next(1000, 8000);
            
            mutex.WaitOne();
            if (timeTaken == 0 || sleepTime < timeTaken)
            {
                timeTaken = sleepTime;
                winnerThreadId = Thread.CurrentThread.ManagedThreadId;
            }
            mutex.ReleaseMutex();

            Thread.Sleep(sleepTime);
            Console.WriteLine("The thread with the Id of {0} has completed", Thread.CurrentThread.ManagedThreadId);
        }
    }
}
