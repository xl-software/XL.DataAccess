using System;
using System.Collections.Generic;

namespace XL.DataAccess
{
    public class QueryResult
    {
        public List<Column> Columns { get; set; }
        public List<List<Column>> Rows { get; set; }
        public string SQLQuery { get; set; }
    }

    public class Column
    {
        public string SqlType { get; set; }
        public Type Type { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Value + " (" + SqlType + ")";
        }
    }
}
