using System;

namespace CsvOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class CsvColumnAttribute : Attribute
    {
        public string ColumnName { get; }

        public CsvColumnAttribute(string columnName)
        {
            ColumnName = columnName;
        }
    }
}