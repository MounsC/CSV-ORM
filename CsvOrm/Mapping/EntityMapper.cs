using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsvOrm.Attributes;

namespace CsvOrm.Mapping
{
    public static class EntityMapper<TEntity> where TEntity : class, new()
    {
        public static Dictionary<string, PropertyInfo> GetColumnMappings()
        {
            return typeof(TEntity).GetProperties()
                .Select(p => new
                {
                    Property = p,
                    Attribute = p.GetCustomAttribute<CsvColumnAttribute>()
                })
                .Where(x => x.Attribute != null)
                .ToDictionary(x => x.Attribute.ColumnName, x => x.Property);
        }
    }
}