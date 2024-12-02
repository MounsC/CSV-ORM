using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using CsvOrm.Core;
using CsvOrm.Models;

namespace CsvOrmApp
{
    class Program
    {
        static void Main(string[] args)
        {
            RunLoadTestAsync()
                .GetAwaiter()
                .GetResult();

            Console.WriteLine("Appuyez sur Entrée pour quitter...");
            Console.ReadLine();
        }

        static async Task RunLoadTestAsync()
        {
            int numberOfTasks = 5;
            int usersPerTask = 100;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = new Task[numberOfTasks];

            for (int t = 0; t < numberOfTasks; t++)
            {
                int taskIndex = t;
                tasks[taskIndex] = Task.Run(() =>
                {
                    var context = new CsvContext();
                    for (int i = 1; i <= usersPerTask; i++)
                    {
                        int userId = taskIndex * usersPerTask + i;
                        var user = new User
                        {
                            Id = userId,
                            Username = $"user{userId}",
                            Email = $"user{userId}@mouns.ms",
                            RoleId = 1
                        };
                        context.Users.Add(user);
                    }
                    context.SaveChanges();
                });
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();
            Console.WriteLine($"Temps pour ajouter {numberOfTasks * usersPerTask} utilisateurs : {stopwatch.Elapsed}");
        }
    }
}