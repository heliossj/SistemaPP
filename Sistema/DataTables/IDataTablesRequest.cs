namespace Sistema.DataTables
{
    public interface IDataTablesRequest
    {
        int Draw
        {
            get;
            set;
        }
        int Start
        {
            get;
            set;
        }
        int Length
        {
            get;
            set;
        }
        Search Search
        {
            get;
            set;
        }
        ColumnCollection Columns
        {
            get;
            set;
        }
    }
}
