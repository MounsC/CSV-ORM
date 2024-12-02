using System;
using System.Collections.Generic;
using CsvOrm.Models;

namespace CsvOrm.Core
{
    public sealed class CsvContext
    {
        private readonly Dictionary<Type, object> repositories = new();
        private readonly string basePath;

        public CsvContext(string basePath = null)
        {
            this.basePath = basePath ?? AppDomain.CurrentDomain.BaseDirectory;
        }

        public Repository<TEntity> GetRepository<TEntity>() where TEntity : class, new()
        {
            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type))
            {
                var repository = new Repository<TEntity>(this, basePath);
                repositories[type] = repository;
            }
            return (Repository<TEntity>)repositories[type];
        }

        public void SaveChanges()
        {
            foreach (var repo in repositories.Values)
            {
                var method = repo.GetType().GetMethod("SaveChanges");
                method?.Invoke(repo, null);
            }
        }

        public Repository<User> Users => GetRepository<User>();
        public Repository<Role> Roles => GetRepository<Role>();
        public Repository<Product> Products => GetRepository<Product>();
        public Repository<Order> Orders => GetRepository<Order>();
        public Repository<OrderItem> OrderItems => GetRepository<OrderItem>();
        public Repository<Category> Categories => GetRepository<Category>();
        public Repository<Supplier> Suppliers => GetRepository<Supplier>();
    }
}