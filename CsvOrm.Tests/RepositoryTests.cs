using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvOrm.Core;
using CsvOrm.Models;
using System;
using System.IO;

namespace CsvOrm.Tests
{
    [TestClass]
    public class RepositoryTests
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
        public void Add_DuplicatePrimaryKey_ThrowsException()
        {
            var context = new CsvContext(testDataPath);
            var user1 = new User { Id = 2, Username = "testuser1", Email = "testuser1@mouns.ms", RoleId = 1 };
            var user2 = new User { Id = 2, Username = "testuser2", Email = "testuser2@mouns.ms", RoleId = 1 };
            context.Users.Add(user1);
            context.SaveChanges();

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                context.Users.Add(user2);
                context.SaveChanges();
            });

            Assert.AreEqual("La valeur de Id doit être unique.", ex.Message);
        }

        [TestMethod]
        public void Add_NullNotAllowed_ThrowsException()
        {
            var context = new CsvContext(testDataPath);

            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                var user = new User { Id = 3, Username = null, Email = "testuser@mouns.ms", RoleId = 1 };
                context.Users.Add(user);
                context.SaveChanges();
            });

            Assert.AreEqual("La propriété Username ne peut pas être nulle.", ex.Message);
        }
    }
}