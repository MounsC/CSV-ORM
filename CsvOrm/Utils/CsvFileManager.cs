using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvOrm.Attributes;
using System.Reflection;
using System.Text;

namespace CsvOrm.Utils
{
    public static class CsvFileManager
    {
        private static readonly object fileLock = new object();

        public static List<TEntity> ReadCsv<TEntity>(string filePath) where TEntity : class, new()
        {
            lock (fileLock)
            {
                var entities = new List<TEntity>();

                if (!File.Exists(filePath))
                    return entities;

                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    var headerLine = reader.ReadLine();
                    if (headerLine == null)
                        return entities;

                    var header = headerLine.Split(',');

                    var properties = typeof(TEntity).GetProperties()
                        .Where(p => Attribute.IsDefined(p, typeof(CsvColumnAttribute)))
                        .ToArray();

                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var entity = new TEntity();
                        var values = line.Split(',');

                        for (int j = 0; j < header.Length; j++)
                        {
                            var columnName = header[j];
                            var property = properties.FirstOrDefault(p => p.GetCustomAttribute<CsvColumnAttribute>().ColumnName == columnName);

                            if (property != null)
                            {
                                var value = values[j];
                                var convertedValue = Convert.ChangeType(value, property.PropertyType);
                                property.SetValue(entity, convertedValue);
                            }
                        }

                        entities.Add(entity);
                    }
                }

                return entities;
            }
        }

        public static void WriteCsv<TEntity>(string filePath, IEnumerable<TEntity> entities) where TEntity : class, new()
        {
            lock (fileLock)
            {
                var properties = typeof(TEntity).GetProperties()
                    .Where(p => Attribute.IsDefined(p, typeof(CsvColumnAttribute)))
                    .ToArray();

                var header = string.Join(",", properties.Select(p => p.GetCustomAttribute<CsvColumnAttribute>().ColumnName));

                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(header);

                foreach (var entity in entities)
                {
                    var values = properties.Select(p => p.GetValue(entity)?.ToString() ?? string.Empty);
                    var line = string.Join(",", values);
                    stringBuilder.AppendLine(line);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                using (var writer = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    writer.Write(stringBuilder.ToString());
                }
            }
        }
    }
}
