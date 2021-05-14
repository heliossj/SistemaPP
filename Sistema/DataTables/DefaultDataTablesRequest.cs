namespace Sistema.DataTables
{
    public class DefaultDataTablesRequest : IDataTablesRequest
    {
        public virtual int Draw
        {
            get;
            set;
        }
        public virtual int Start
        {
            get;
            set;
        }
        public virtual int Length
        {
            get;
            set;
        }
        public virtual Search Search
        {
            get;
            set;
        }
        public virtual ColumnCollection Columns
        {
            get;
            set;
        }
    }
}
