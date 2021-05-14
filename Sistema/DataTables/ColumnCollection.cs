using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Sistema.DataTables
{
    public class ColumnCollection : IEnumerable<Column>, IEnumerable
    {
        private IReadOnlyList<Column> Data;
        public ColumnCollection(IEnumerable<Column> columns)
        {
            if (columns == null)
            {
                throw new ArgumentNullException("A lista de coluna fornecida não pode ser nula", "colunas");
            }
            this.Data = columns.ToList<Column>().AsReadOnly();
        }
        public IOrderedEnumerable<Column> GetSortedColumns()
        {
            return
                from _column in this.Data
                where !string.IsNullOrWhiteSpace(_column.Data) && _column.IsOrdered
                select _column into _c
                orderby _c.OrderNumber
                select _c;
        }
        public IEnumerable<Column> GetFilteredColumns()
        {
            return
                from _column in this.Data
                where !string.IsNullOrWhiteSpace(_column.Data) && _column.Searchable && !string.IsNullOrWhiteSpace(_column.Search.Value)
                select _column;
        }
        public IEnumerator<Column> GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Data.GetEnumerator();
        }
    }
}
