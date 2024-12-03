using Microsoft.VisualStudio.TestTools.UnitTesting;
using CsvOrm.Core;
using CsvOrm.Models;
using System.IO;
using System.Linq;
using System;

namespace CsvOrm.Tests
{
    [TestClass]
    public class IntegrationTests
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
        public void FullDataFlowTest()
        {
            var context = new CsvContext(testDataPath);

            var role = new Role { Id = 1, Name = "User" };
            context.Roles.Add(role);
            context.SaveChanges();

            var user = new User
            {
                Id = 1,
                Username = "testmouns",
                Email = "testmouns@mouns.ms",
                RoleId = role.Id
            };
            context.Users.Add(user);
            context.SaveChanges();

            var category = new Category { Id = 1, Name = "Books" };
            context.Categories.Add(category);
            context.SaveChanges();

            var supplier = new Supplier
            {
                Id = 1,
                Name = "Book Supplier",
                ContactEmail = "contact@mouns.ms"
            };
            context.Suppliers.Add(supplier);
            context.SaveChanges();

            var product = new Product
            {
                Id = 1,
                Name = "CSV Supplier Product",
                Price = 29.99m,
                CategoryId = category.Id,
                SupplierId = supplier.Id
            };
            context.Products.Add(product);
            context.SaveChanges();

            var order = new Order
            {
                Id = 1,
                UserId = user.Id,
                OrderDate = DateTime.Now
            };
            context.Orders.Add(order);
            context.SaveChanges();

            var orderItem = new OrderItem
            {
                Id = 1,
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = 2,
                UnitPrice = product.Price
            };
            context.OrderItems.Add(orderItem);
            context.SaveChanges();

            var retrievedOrder = context.Orders
                .Include(o => o.User)
                .IncludeCollection(o => o.OrderItems)
                .Find(order.Id);

            foreach (var item in retrievedOrder.OrderItems)
            {
                item.Product = context.Products.Find(item.ProductId);
            }

            Assert.IsNotNull(retrievedOrder);
            Assert.IsNotNull(retrievedOrder.User);
            Assert.AreEqual(user.Username, retrievedOrder.User.Username);
            Assert.IsNotNull(retrievedOrder.OrderItems);
            Assert.AreEqual(1, retrievedOrder.OrderItems.Count);

            var orderItemProductId = retrievedOrder.OrderItems.First().ProductId;
            var retrievedProduct = context.Products.Find(orderItemProductId);
            Assert.IsNotNull(retrievedProduct);
            Assert.AreEqual(product.Name, retrievedProduct.Name);
        }
    }
}
