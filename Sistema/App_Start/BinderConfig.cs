using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistema
{
    public class BinderConfig
    {
        public static void RegisterGlobalBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(DateTime), new DateTimeBinder());
            binders.Add(typeof(DateTime?), new DateTimeBinder());
            binders.Add(typeof(Decimal), new DecimalModelBinder());
            binders.Add(typeof(Decimal?), new DecimalModelBinder());
        }


        public class DateTimeBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                try
                {
                    if (value == null)
                        return null;
                    var date = value.ConvertTo(typeof(DateTime), new CultureInfo("pt-BR"));
                    return date;
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }

        public class DecimalModelBinder : IModelBinder
        {
            public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
                if ((valueResult != null && string.IsNullOrEmpty(valueResult.AttemptedValue)) || valueResult == null)
                {
                    return null;
                }
                try
                {
                    var value = Convert.ToDecimal(valueResult.AttemptedValue, new CultureInfo("pt-BR"));
                    return value;
                }
                catch (System.Exception)
                {
                    return null;
                }
            }
        }

        public class RPDefaultModelBinder : DefaultModelBinder
        {
            protected override object CreateModel(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext, Type modelType)
            {
                try
                {
                    return base.CreateModel(controllerContext, bindingContext, modelType);
                }
                catch (HttpRequestValidationException e)
                {
                    HandleHttpRequestValidationException(bindingContext, e);
                    return null; // Encode here
                }
            }

            protected override object GetPropertyValue(ControllerContext controllerContext, System.Web.Mvc.ModelBindingContext bindingContext,
                PropertyDescriptor propertyDescriptor, System.Web.Mvc.IModelBinder propertyBinder)
            {
                try
                {
                    return base.GetPropertyValue(controllerContext, bindingContext, propertyDescriptor, propertyBinder);
                }
                catch (HttpRequestValidationException e)
                {
                    HandleHttpRequestValidationException(bindingContext, e);
                    return null; // Encode here
                }
            }

            protected void HandleHttpRequestValidationException(System.Web.Mvc.ModelBindingContext bindingContext, HttpRequestValidationException ex)
            {
                var valueProviderCollection = bindingContext.ValueProvider as System.Web.Mvc.ValueProviderCollection;
                if (valueProviderCollection != null)
                {
                    System.Web.Mvc.ValueProviderResult valueProviderResult = valueProviderCollection.GetValue(bindingContext.ModelName, skipValidation: true);
                    bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueProviderResult);
                }

                string errorMessage = string.Format(CultureInfo.CurrentCulture, "{0} contém caracteres inválidos: <, & ou >",
                         bindingContext.ModelMetadata.DisplayName);

                bindingContext.ModelState.AddModelError(bindingContext.ModelName, errorMessage);
            }

        }
    }
}