using System;

namespace CsvOrm.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class CsvTableAttribute : Attribute
    {
        public string FileName { get; }

        public CsvTableAttribute(string fileName)
        {
            FileName = fileName;
        }
    }
}