using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvOrm.Core;
using CsvOrm.Models;
using System.IO;
using System;

namespace CsvOrm.Tests
{
    [TestClass]
    public class CsvContextTests
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
        public void AddUser_Test()
        {
            var context = new CsvContext(testDataPath);
            var user = new User
            {
                Id = 1,
                Username = "mouns",
                Email = "contact@mouns.ms",
                RoleId = 1
            };
            context.Users.Add(user);
            context.SaveChanges();

            var retrievedUser = context.Users.Find(1);
            Assert.IsNotNull(retrievedUser);
            Assert.AreEqual("mouns", retrievedUser.Username);
        }

        [TestMethod]
        public void RemoveUser_Test()
        {
            var context = new CsvContext(testDataPath);

            var user = new User
            {
                Id = 2,
                Username = "mounsc",
                Email = "contacts@mouns.ms",
                RoleId = 1
            };
            context.Users.Add(user);
            context.SaveChanges();

            var userToRemove = context.Users.Find(2);
            Assert.IsNotNull(userToRemove);
            context.Users.Remove(userToRemove);
            context.SaveChanges();

            var retrievedUser = context.Users.Find(2);
            Assert.IsNull(retrievedUser);
        }
    }
}