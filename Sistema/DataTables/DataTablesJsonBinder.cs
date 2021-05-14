using System;
using System.Web;
using System.Web.Mvc;

namespace Sistema.DataTables
{
    public abstract class DataTablesJsonBinder : IModelBinder
    {
        protected virtual string JSON_PARAMETER_NAME
        {
            get
            {
                return "json";
            }
        }
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.RequestContext.HttpContext.Request;
            request.HttpMethod.ToLower();
            string arg_23_0 = request.ContentType;
            if (!this.IsJsonRequest(request))
            {
                throw new ArgumentException("You must provide a JSON request and set it's content type to match a JSON request content type.");
            }
            return this.Deserialize(bindingContext.ValueProvider.GetValue(this.JSON_PARAMETER_NAME).AttemptedValue);
        }
        public virtual bool IsJsonRequest(HttpRequestBase request)
        {
            return request.ContentType.ToLower().Contains("json");
        }
        protected abstract IDataTablesRequest Deserialize(string jsonData);
    }
}
