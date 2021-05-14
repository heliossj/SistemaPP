using System;

namespace Sistema.DataTables
{

    public class Column
    {
        public enum OrderDirection
        {
            Ascendant,
            Descendant
        }
        public string Data
        {
            get;
            set;
        }
        public string Name
        {
            get;
            private set;
        }
        public bool Searchable
        {
            get;
            set;
        }
        public bool Orderable
        {
            get;
            set;
        }
        public Search Search
        {
            get;
            private set;
        }
        public bool IsOrdered
        {
            get
            {
                return this.OrderNumber != -1;
            }
        }
        public int OrderNumber
        {
            get;
            set;
        }
        public Column.OrderDirection SortDirection
        {
            get;
            private set;
        }
        public void SetColumnOrdering(int orderNumber, string orderDirection)
        {
            this.OrderNumber = orderNumber;
            if (orderDirection.ToLower().Equals("asc"))
            {
                this.SortDirection = Column.OrderDirection.Ascendant;
                return;
            }
            if (orderDirection.ToLower().Equals("desc"))
            {
                this.SortDirection = Column.OrderDirection.Descendant;
                return;
            }
            throw new ArgumentException("The provided ordering direction was not valid. Valid values must be 'asc' or 'desc' only.");
        }
        public Column(string data, string name, bool searchable, bool orderable, string searchValue, bool isRegexValue)
        {
            this.Data = data;
            this.Name = name;
            this.Searchable = searchable;
            this.Orderable = orderable;
            this.Search = new Search(searchValue, isRegexValue);
            this.OrderNumber = -1;
        }
    }
}