using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Sistema.DataTables
{
    public class DataTablesResponse
    {
        public int draw
        {
            get;
            private set;
        }

        public IEnumerable data
        {
            get;
            set;
        }

        public int recordsTotal
        {
            get;
            private set;
        }

        public int recordsFiltered
        {
            get;
            private set;
        }

        public DataTablesResponse(int draw, IEnumerable data, int recordsFiltered, int recordsTotal)
        {
            this.draw = draw;
            this.data = data;
            this.recordsFiltered = recordsFiltered;
            this.recordsTotal = recordsTotal;
        }

        public DataTablesResponse(int draw, IList paginatedResult, int rowsLength)
        {
            this.draw = draw;
            this.data = paginatedResult;
            this.recordsFiltered = rowsLength;
            this.recordsTotal = paginatedResult.Count;
        }

        public DataTablesResponse(IDataTablesRequest requestModel, IQueryable<dynamic> query)
        {
            var totalResult = query.Count();

            var result = query.OrderBy(requestModel.Columns, requestModel.Start, requestModel.Length).ToList();
            this.draw = requestModel.Draw;
            this.data = result;
            this.recordsFiltered = totalResult;
            this.recordsTotal = result.Count;
        }
    }
}
