using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema.DataTables
{
    public class DataTablesBinder : IModelBinder
    {
        protected readonly string COLUMN_DATA_FORMATTING = "columns[{0}][data]";
        protected readonly string COLUMN_NAME_FORMATTING = "columns[{0}][name]";
        protected readonly string COLUMN_SEARCHABLE_FORMATTING = "columns[{0}][searchable]";
        protected readonly string COLUMN_ORDERABLE_FORMATTING = "columns[{0}][orderable]";
        protected readonly string COLUMN_SEARCH_VALUE_FORMATTING = "columns[{0}][search][value]";
        protected readonly string COLUMN_SEARCH_REGEX_FORMATTING = "columns[{0}][search][regex]";
        protected readonly string ORDER_COLUMN_FORMATTING = "order[{0}][column]";
        protected readonly string ORDER_DIRECTION_FORMATTING = "order[{0}][dir]";
        public virtual object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            return this.Bind(controllerContext, bindingContext, typeof(DefaultDataTablesRequest));
        }
        protected virtual object Bind(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            HttpRequestBase request = controllerContext.RequestContext.HttpContext.Request;
            IDataTablesRequest dataTablesRequest = (IDataTablesRequest)Activator.CreateInstance(modelType);
            NameValueCollection nameValueCollection = this.ResolveNameValueCollection(request);
            dataTablesRequest.Draw = this.Get<int>(nameValueCollection, "draw");
            dataTablesRequest.Start = this.Get<int>(nameValueCollection, "start");
            dataTablesRequest.Length = this.Get<int>(nameValueCollection, "length");
            string value = this.Get<string>(nameValueCollection, "search[value]");
            bool isRegexValue = this.Get<bool>(nameValueCollection, "search[regex]");
            dataTablesRequest.Search = new Search(value, isRegexValue);
            List<Column> columns = this.GetColumns(nameValueCollection);
            this.ParseColumnOrdering(nameValueCollection, columns);
            dataTablesRequest.Columns = new ColumnCollection(columns);
            this.MapAditionalProperties(dataTablesRequest, nameValueCollection);
            return dataTablesRequest;
        }
        protected virtual void MapAditionalProperties(IDataTablesRequest requestModel, NameValueCollection requestParameters)
        {
        }
        protected virtual NameValueCollection ResolveNameValueCollection(HttpRequestBase request)
        {
            if (request.HttpMethod.ToLower().Equals("get"))
            {
                return request.QueryString;
            }
            if (request.HttpMethod.ToLower().Equals("post"))
            {
                return request.Form;
            }
            throw new ArgumentException(string.Format("The provided HTTP method ({0}) is not a valid method to use with DataTablesBinder. Please, use HTTP GET or POST methods only.", request.HttpMethod), "method");
        }
        protected virtual T Get<T>(NameValueCollection collection, string key)
        {
            return collection.G<T>(key);
        }
        protected virtual List<Column> GetColumns(NameValueCollection collection)
        {
            List<Column> result;
            try
            {
                List<Column> list = new List<Column>();
                for (int i = 0; i < collection.Count; i++)
                {
                    string text = this.Get<string>(collection, string.Format(this.COLUMN_DATA_FORMATTING, i));
                    string text2 = this.Get<string>(collection, string.Format(this.COLUMN_NAME_FORMATTING, i));
                    if (text == null || text2 == null)
                    {
                        break;
                    }
                    bool searchable = this.Get<bool>(collection, string.Format(this.COLUMN_SEARCHABLE_FORMATTING, i));
                    bool orderable = this.Get<bool>(collection, string.Format(this.COLUMN_ORDERABLE_FORMATTING, i));
                    string searchValue = this.Get<string>(collection, string.Format(this.COLUMN_SEARCH_VALUE_FORMATTING, i));
                    bool isRegexValue = this.Get<bool>(collection, string.Format(this.COLUMN_SEARCH_REGEX_FORMATTING, i));
                    list.Add(new Column(text, text2, searchable, orderable, searchValue, isRegexValue));
                }
                result = list;
            }
            catch
            {
                result = new List<Column>();
            }
            return result;
        }
        protected virtual void ParseColumnOrdering(NameValueCollection collection, IEnumerable<Column> columns)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                int num = this.Get<int>(collection, string.Format(this.ORDER_COLUMN_FORMATTING, i));
                string text = this.Get<string>(collection, string.Format(this.ORDER_DIRECTION_FORMATTING, i));
                if (num > -1 && text != null)
                {
                    columns.ElementAt(num).SetColumnOrdering(i, text);
                }
            }
        }
    }
}
