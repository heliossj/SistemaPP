using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sistema.Util
{
    public class Util
    {
        public const string Input1 = "form-group col-md-1";
        public const string Input2 = "form-group col-md-2";
        public const string Input3 = "form-group col-md-3";
        public const string Input4 = "form-group col-md-4";
        public const string Input5 = "form-group col-md-5";
        public const string Input6 = "form-group col-md-6";
        public const string Input7 = "form-group col-md-7";
        public const string Input8 = "form-group col-md-8";
        public const string Input9 = "form-group col-md-9";
        public const string Input10 = "form-group col-md-10";
        public const string Input11 = "form-group col-md-11";
        public const string Input12 = "form-group col-md-12";
    }

    public class AlertMessage
    {
        public const string VALIDATION_MESSAGE = "Existem campos obrigatórios não preenchidos, verifique!";
        public const string INSERT_SUCESS = "Registro inserido com sucesso";
        public const string EDIT_SUCESS = "Registro alterado com sucesso";
        public const string DELETE_SUCESS = "Registro removido com sucesso";
        public const string ERROR_MESSAGE = "Ocorreram alguns erros, verifique!";
    }

    public static class MvcHtmlStringX
    {
        const string keyForBlockScript = "__key_For_Js_Block";
        const string keyForBlockStyle = "__key_For_Css_Block";

        public static MvcHtmlString AddScriptBlock(this HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template)
        {
            return AddBlock(helper, template, keyForBlockScript);
        }

        private static MvcHtmlString AddBlock(HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template, string type)
        {
            var stringBuilder = helper.ViewContext.HttpContext.Items[type] as StringBuilder ?? new StringBuilder();
            stringBuilder.Append(template(null).ToHtmlString());
            helper.ViewContext.HttpContext.Items[type] = stringBuilder;
            return new MvcHtmlString(string.Empty);
        }

    }

    public class SelectFunctions
    {
        public static string getInputId(string id, string prefixo)
        {
            return ((string.IsNullOrEmpty(prefixo) ? "" : prefixo + "_") + id).Replace('.', '_');
        }
        //public static MvcHtmlString ClientPrefixName(this HtmlHelper htmlHelper)

        public static string getInputName(string name, string prefixo)
        {
            return (string.IsNullOrEmpty(prefixo) ? "" : prefixo + ".") + name;
        }

    }


    public static partial class HtmlExtensions
    {
        public static MvcHtmlString ClientPrefixName(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix.Replace('.', '_'));
        }
        public static MvcHtmlString ClientPrefix(this HtmlHelper htmlHelper)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.HtmlFieldPrefix);
        }

        public static MvcHtmlString ClientNameFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(ExpressionHelper.GetExpressionText(expression)));
        }

        public static MvcHtmlString ClientIdFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return MvcHtmlString.Create(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
        }

    }

    public class FormatFlag
    {
        public static string Situacao(string flSituacao)
        {
            if (flSituacao == "A")
                return "ATIVA";
            if (flSituacao == "I")
                return "INATIVA";
            return flSituacao;
        }
        public static string TipoPessoa(string flPessoa)
        {
            if (flPessoa == "F")
                return "FÍSICA";
            if (flPessoa == "J")
                return "JURÍDICA";
            return flPessoa;
        }
    }
}


public static class FatHtml
{
    const string keyForBlockScript = "__key_For_Js_Block";
    const string keyForBlockStyle = "__key_For_Css_Block";

    private static MvcHtmlString AddBlock(HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template, string type)
    {
        var stringBuilder = helper.ViewContext.HttpContext.Items[type] as StringBuilder ?? new StringBuilder();
        stringBuilder.Append(template(null).ToHtmlString());
        helper.ViewContext.HttpContext.Items[type] = stringBuilder;
        return new MvcHtmlString(string.Empty);
    }

    /// <summary>
    /// Adiciona um bloco JavaScript ao fim da lista de bloco de scripts
    /// </summary> 
    public static MvcHtmlString AddScriptBlock(this HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template)
    {
        return AddBlock(helper, template, keyForBlockScript);
    }

    /// <summary>
    /// Adiciona um bloco Css ao fim da lista de bloco de estilos
    /// </summary>
    public static MvcHtmlString AddStyleBlock(this HtmlHelper helper, Func<dynamic, System.Web.WebPages.HelperResult> template)
    {
        return AddBlock(helper, template, keyForBlockStyle);
    }

    /// <summary>
    /// Renderiza todos blocos de scripts
    /// </summary>
    public static MvcHtmlString RenderScriptBlocks(this HtmlHelper helper)
    {
        var stringBuilder = helper.ViewContext.HttpContext.Items[keyForBlockScript] as StringBuilder ?? new StringBuilder();
        return new MvcHtmlString(stringBuilder.ToString());
    }

    /// <summary>
    /// Renderiza todos blocos de estilos
    /// </summary>
    public static MvcHtmlString RenderStyleBlocks(this HtmlHelper helper)
    {
        var stringBuilder = helper.ViewContext.HttpContext.Items[keyForBlockStyle] as StringBuilder ?? new StringBuilder();
        return new MvcHtmlString(stringBuilder.ToString());
    }
}
public class JsonSelect<T>
{
    public JsonSelect() { }

    public JsonSelect(IQueryable<T> query, int? page, int? pageSize)
    {
        page = page ?? 10;
        pageSize = pageSize ?? 10;

        totalCount = query.Count();
        totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        more = (page * pageSize) < totalCount;

        results = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
    }

    public List<T> results { get; set; }
    public int totalCount { get; set; }
    public int totalPages { get; set; }
    public bool more { get; set; }
}

//FlashMessage

public static class FlashMessage
{
    public const string ERROR = "error";
    public const string ALERT = "alert";
    public const string INFO = "info";
    public const string SUCCESS = "success";

    [Serializable]
    public class Message
    {
        public string type { get; set; }
        public string textMessage { get; set; }
        public bool closeable { get; set; }

        public Message()
        {
            closeable = true;
            type = FlashMessage.ALERT;
        }
    }

    public static void AddFlashMessage(this HtmlHelper helper, Message message)
    {
        if (helper.ViewContext.Controller.TempData.ContainsKey(message.type))
        {
            List<Message> messages = null;
            if (helper.ViewContext.Controller.TempData[message.type].GetType() == typeof(List<Message>))
            {
                messages = (List<Message>)helper.ViewContext.Controller.TempData[message.type];
                messages.Add(message);
            }
            else
            {
                messages = new List<Message> { message };
            }

            helper.ViewContext.Controller.TempData[message.type] = messages;
        }
        else
        {
            helper.ViewContext.Controller.TempData[message.type] = new List<Message> { message };
        }
    }

    public static void AddFlashMessage(this Controller controller, Message message)
    {
        if (controller.TempData.ContainsKey(message.type))
        {
            List<Message> messages = null;
            if (controller.TempData[message.type].GetType() == typeof(List<Message>))
            {
                messages = (List<Message>)controller.TempData[message.type];
                messages.Add(message);
            }
            else
            {
                messages = new List<Message> { message };
            }

            controller.TempData[message.type] = messages;
        }
        else
        {
            controller.TempData[message.type] = new List<Message> { message };
        }
    }

    public static void AddFlashMessage(this HtmlHelper helper, string message, string type = FlashMessage.SUCCESS)
    {
        AddFlashMessage(helper, new Message
        {
            type = type,
            textMessage = message,
            closeable = true
        });
    }

    public static void AddFlashMessage(this Controller controller, string message, string type = FlashMessage.SUCCESS)
    {
        AddFlashMessage(controller, new Message
        {
            type = type,
            textMessage = message,
            closeable = true
        });
    }

    public static MvcHtmlString RenderFlashMessage(this HtmlHelper helper)
    {
        string cssClass, inner;
        string result = string.Empty;
        string format = @"<div class=""{0}"">{1}</div>";

        foreach (KeyValuePair<string, object> item in helper.ViewContext.Controller.TempData)
        {
            foreach (Message message in (List<Message>)item.Value)
            {
                cssClass = string.Empty;
                inner = string.Empty;

                if (message.closeable)
                {
                    inner += @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close"">×</button>";
                    cssClass += "print ";
                }

                if ((item.Key == message.type) && item.Key == FlashMessage.INFO)
                {
                    cssClass += "alert alert-info";
                }
                else if (item.Key == FlashMessage.ERROR)
                {
                    cssClass += "alert alert-danger";
                }
                else if (item.Key == FlashMessage.SUCCESS)
                {
                    cssClass += "alert alert-success";
                }
                else
                {
                    cssClass += "alert alert-warning";
                }

                inner += message.textMessage;
                result += string.Format(format, cssClass, inner);
            }
        }

        return MvcHtmlString.Create(result);
    }
}


public static class ViewExtensions
{
    public static MvcHtmlString CustomValidationSummary(this HtmlHelper html, bool closeable = true, bool hideProperties = true, string validationMessage = "", object htmlAttributes = null)
    {
        if (!html.ViewData.ModelState.IsValid)
        {

            TagBuilder div = new TagBuilder("div");
            string properties = string.Empty;

            // adiciona os atributos
            if (htmlAttributes != null)
            {
                var type = htmlAttributes.GetType();
                var props = type.GetProperties();

                foreach (var item in props)
                {
                    div.MergeAttribute(item.Name, item.GetValue(htmlAttributes, null).ToString());
                }
            }

            if (closeable)
            {
                div.InnerHtml += @"<button type=""button"" class=""close"" data-dismiss=""alert"" aria-label=""Close"">×</button>";
                div.AddCssClass("print");
            }

            // adiciona mensagem na div
            div.InnerHtml += validationMessage;

            if (!hideProperties)
            {
                foreach (var key in html.ViewData.ModelState.Keys)
                {
                    foreach (var err in html.ViewData.ModelState[key].Errors)
                    {
                        properties += "<p>" + html.Encode(err.ErrorMessage) + "</p>";
                    }
                }

                if (!string.IsNullOrEmpty(properties))
                {
                    div.InnerHtml += properties;
                }
            }

            return MvcHtmlString.Create(div.ToString());
        }

        return null;
    }

    public static MvcHtmlString CustomActionLink(this HtmlHelper html, string linkText, string actionName, object htmlAttributes, object icons = null, bool hideText = false)
    {
        return CustomActionLink(html, linkText, actionName, null, new { }, htmlAttributes, icons, hideText);
    }

    public static MvcHtmlString CustomActionLink(this HtmlHelper html, string linkText, string actionName, object routeValues, object htmlAttributes, object icons = null, bool hideText = false)
    {
        return CustomActionLink(html, linkText, actionName, null, routeValues, htmlAttributes, icons, hideText);
    }

    public static MvcHtmlString CustomActionLink(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes, object icons = null, bool hideText = false)
    {
        UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
        string iconLeft = string.Empty,
                iconRight = string.Empty,
                innerHtml = string.Empty;

        TagBuilder a = new TagBuilder("a");
        TagBuilder i;
        bool hasTitle = false;

        if (string.IsNullOrEmpty(controllerName))
        {
            a.Attributes.Add("href", actionName.StartsWith("#") ? actionName : urlHelper.Action(actionName, routeValues));
        }
        else
        {
            a.Attributes.Add("href", actionName.StartsWith("#") ? actionName : urlHelper.Action(actionName, controllerName, routeValues));
        }

        // adiciona os atributos
        var htmlAttributesDictionary = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

        foreach (var attribute in htmlAttributesDictionary)
        {
            if (!hasTitle)
            {
                hasTitle = attribute.Key.ToLower().Equals("title");
            }

            a.MergeAttribute(attribute.Key, Convert.ToString(attribute.Value));
        }

        // adiciona os icones
        if (icons != null)
        {
            var type = icons.GetType();
            var props = type.GetProperties();
            foreach (var item in props)
            {
                if (item.Name.ToLower().Equals("left"))
                {
                    iconLeft = item.GetValue(icons, null).ToString();
                }
                else if (item.Name.ToLower().Equals("right"))
                {
                    iconRight = item.GetValue(icons, null).ToString();
                }
            }
        }

        if (!string.IsNullOrEmpty(iconLeft))
        {
            i = new TagBuilder("i");
            i.AddCssClass(iconLeft);
            innerHtml += i.ToString() + " ";
        }

        if (!hideText)
        {
            innerHtml += linkText;
        }

        if (!hasTitle && hideText)
        {
            a.Attributes.Add("title", linkText);
        }

        if (!string.IsNullOrEmpty(iconRight))
        {
            i = new TagBuilder("i");
            i.AddCssClass(iconRight);
            innerHtml += " " + i.ToString();
        }

        a.InnerHtml = innerHtml;

        return MvcHtmlString.Create(a.ToString());
    }

    public static MvcHtmlString CustomButton(this HtmlHelper html, string buttonText, object htmlAttributes, object icons = null, bool hideText = false)
    {
        return CustomButton(html, buttonText, null, htmlAttributes, icons, hideText);
    }

    public static MvcHtmlString CustomButton(this HtmlHelper html, string buttonText, string buttonType, object htmlAttributes, object icons = null, bool hideText = false)
    {
        UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);
        string iconLeft = string.Empty,
                iconRight = string.Empty,
                innerHtml = string.Empty;

        TagBuilder button = new TagBuilder("button");
        TagBuilder i;
        bool hasTitle = false;

        if (string.IsNullOrEmpty(buttonType))
        {
            button.Attributes.Add("type", "button");
        }
        else
        {
            button.Attributes.Add("type", buttonType);
        }


        // adiciona os atributos
        if (htmlAttributes != null)
        {
            var type = htmlAttributes.GetType();
            var props = type.GetProperties();
            foreach (var item in props)
            {
                if (!hasTitle)
                {
                    hasTitle = item.Name.ToLower().Equals("title");
                }

                button.MergeAttribute(item.Name, item.GetValue(htmlAttributes, null).ToString());
            }
        }

        // adiciona os icones
        if (icons != null)
        {
            var type = icons.GetType();
            var props = type.GetProperties();
            foreach (var item in props)
            {
                if (item.Name.ToLower().Equals("left"))
                {
                    iconLeft = item.GetValue(icons, null).ToString();
                }
                else if (item.Name.ToLower().Equals("right"))
                {
                    iconRight = item.GetValue(icons, null).ToString();
                }
            }
        }

        if (!string.IsNullOrEmpty(iconLeft))
        {
            i = new TagBuilder("i");
            i.AddCssClass(iconLeft);
            innerHtml += i.ToString() + " ";
        }

        if (!hideText)
        {
            innerHtml += buttonText;
        }

        if (!hasTitle && hideText)
        {
            button.Attributes.Add("title", buttonText);
        }

        if (!string.IsNullOrEmpty(iconRight))
        {
            i = new TagBuilder("i");
            i.AddCssClass(iconRight);
            innerHtml += " " + i.ToString();
        }

        button.InnerHtml = innerHtml;

        return MvcHtmlString.Create(button.ToString());
    }

    public static MvcHtmlString CustomLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes = null)
    {
        return CustomLabelFor(html, expression, string.Empty, htmlAttributes);
    }

    public static MvcHtmlString CustomLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes = null)
    {
        TagBuilder lbl = new TagBuilder("label");
        var metadata = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);

        if (string.IsNullOrEmpty(labelText))
        {
            labelText = metadata.DisplayName;
        }

        lbl.Attributes.Add("for", html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldId(ExpressionHelper.GetExpressionText(expression)));
        lbl.SetInnerText(labelText);

        // adiciona os atributos
        if (htmlAttributes != null)
        {
            var type = htmlAttributes.GetType();
            var props = type.GetProperties();
            foreach (var item in props)
            {
                lbl.MergeAttribute(item.Name, item.GetValue(htmlAttributes, null).ToString());
            }
        }

        return MvcHtmlString.Create(lbl.ToString());
    }
}