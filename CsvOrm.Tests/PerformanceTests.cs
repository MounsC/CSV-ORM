using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvOrm.Core;
using CsvOrm.Models;
using System;
using System.Diagnostics;
using System.IO;

namespace CsvOrm.Tests
{
    [TestClass]
    public class PerformanceTests
    {
        private string testDataPath;

        [TestInitialize]
        public void TestInitialize()
        {
            testDataPath = Path.Combine(Path.GetTempPath(), "CsvOrmTestData_" + Guid.NewGuid());
            Directory.CreateDirectory(testDataPath);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (Directory.Exists(testDataPath))
            {
                Directory.Delete(testDataPath, true);
            }
        }

        [TestMethod]
        public void LoadTest_AddingLargeNumberOfEntities()
        {
            var context = new CsvContext(testDataPath);
            int numberOfEntities = 500;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i <= numberOfEntities; i++)
            {
                var user = new User
                {
                    Id = i,
                    Username = $"testsuser{i}",
                    Email = $"testsusers{i}@mouns.ms",
                    RoleId = 1
                };
                context.Users.Add(user);
            }
            context.SaveChanges();

            stopwatch.Stop();
            Console.WriteLine($"Temps pour ajouter {numberOfEntities} utilisateurs : {stopwatch.Elapsed}");

            var users = context.Users.ToList();
            Assert.AreEqual(numberOfEntities, users.Count);
        }

        [TestMethod]
        public void LoadTest_QueryingLargeDataset()
        {
            var context = new CsvContext(testDataPath);
            int numberOfEntities = 500;
            for (int i = 1; i <= numberOfEntities; i++)
            {
                var user = new User
                {
                    Id = i,
                    Username = $"testsuser{i}",
                    Email = $"testsuser{i}@mouns.ms",
                    RoleId = 1
                };
                context.Users.Add(user);
            }
            context.SaveChanges();

            int numberOfQueries = 100;
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i <= numberOfQueries; i++)
            {
                var user = context.Users.Find(i);
                Assert.IsNotNull(user);
            }

            stopwatch.Stop();
            Console.WriteLine($"Temps pour interroger {numberOfQueries} utilisateurs : {stopwatch.Elapsed}");
        }
    }
}
