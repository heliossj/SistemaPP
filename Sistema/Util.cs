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

    public class FormatSituacao
    {
        public static string Situacao(string flSituacao)
        {
            if (flSituacao == "A")
                return "ATIVA";
            if (flSituacao == "I")
                return "INATIVA";
            return flSituacao;
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