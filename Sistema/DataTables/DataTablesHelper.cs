using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;

namespace Sistema.DataTables
{
    public static class DataTablesHelper
    {
        public static IQueryable<T> QueryCreator<T>(IQueryable<T> query, ColumnCollection columns)
        {
            string orderExpression = string.Empty;

            foreach (var column in columns.GetSortedColumns().OrderBy(p => p.OrderNumber))
            {
                if (column.SortDirection == Column.OrderDirection.Ascendant)
                    orderExpression += string.Format("{0} {1}, ", column.Data, "ascending");
                else
                    orderExpression += string.Format("{0} {1}, ", column.Data, "descending");
            }
            if (!string.IsNullOrEmpty(orderExpression))
            {
                query = query.OrderBy(orderExpression.TrimEnd(new char[] { ',', ' ' }));
            }
            else
            {
                var column = columns.Where(u => u.Orderable == true).FirstOrDefault();
                if (column == null)
                {
                    column = columns.FirstOrDefault();
                }
                //var expresionFixed = string.Format("{0} {1}, ", column.Data, "ascending");
                //query = query.OrderBy(expresionFixed.TrimEnd(new char[] { ',', ' ' }));
            }

            return query;
        }

        public static IQueryable<T> QueryCreator<T, TOrderBy>(IQueryable<T> query, Expression<Func<T, TOrderBy>> order)
        {
            query = query.OrderBy(order);

            return query;
        }

        public static IQueryable<T> OrderBy<T, TOrderBy>(this IQueryable<T> query, Expression<Func<T, TOrderBy>> order, int start, int length)
        {
            //string orderExpression = string.Empty;

            //foreach (var column in columns.GetSortedColumns().OrderBy(p => p.OrderNumber))
            //{
            //    if (column.SortDirection == Column.OrderDirection.Ascendant)
            //        orderExpression += string.Format("{0} {1}, ", column.Data, "ascending");
            //    else
            //        orderExpression += string.Format("{0} {1}, ", column.Data, "descending");
            //}
            //if (!string.IsNullOrEmpty(orderExpression))
            //{
            //    query = query.OrderBy(orderExpression.TrimEnd(new char[] { ',', ' ' }));
            //}
            //else
            //{
            //    var column = columns.Where(u => u.Orderable == true).FirstOrDefault();
            //    if (column == null)
            //    {
            //        column = columns.FirstOrDefault();
            //    }
            //    var expresionFixed = string.Format("{0} {1}, ", column.Data, "ascending");
            //    query = query.OrderBy(expresionFixed.TrimEnd(new char[] { ',', ' ' }));
            //}

            query = QueryCreator<T, TOrderBy>(query, order);

            //paginacao
            if(length <= 0)
            {
                length = query.Count();
            }
            return query.Skip(start).Take(length);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> query, ColumnCollection columns, int start, int length)
        {
            //string orderExpression = string.Empty;

            //foreach (var column in columns.GetSortedColumns().OrderBy(p => p.OrderNumber))
            //{
            //    if (column.SortDirection == Column.OrderDirection.Ascendant)
            //        orderExpression += string.Format("{0} {1}, ", column.Data, "ascending");
            //    else
            //        orderExpression += string.Format("{0} {1}, ", column.Data, "descending");
            //}
            //if (!string.IsNullOrEmpty(orderExpression))
            //{
            //    query = query.OrderBy(orderExpression.TrimEnd(new char[] { ',', ' ' }));
            //}
            //else
            //{
            //    var column = columns.Where(u => u.Orderable == true).FirstOrDefault();
            //    if (column == null)
            //    {
            //        column = columns.FirstOrDefault();
            //    }
            //    var expresionFixed = string.Format("{0} {1}, ", column.Data, "ascending");
            //    query = query.OrderBy(expresionFixed.TrimEnd(new char[] { ',', ' ' }));
            //}

            query = QueryCreator<T>(query, columns);

            //paginacao
            if (length <= 0)
            {
                length = query.Count();
            }
            return query.Skip(start).Take(length);
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> query, ColumnCollection columns, int start, int length)
        {
            string orderExpression = string.Empty;

            foreach (var column in columns.GetSortedColumns().OrderBy(p => p.OrderNumber))
            {
                if (column.SortDirection == Column.OrderDirection.Ascendant)
                    orderExpression += string.Format("{0} {1}, ", column.Data, "ascending");
                else
                    orderExpression += string.Format("{0} {1}, ", column.Data, "descending");
            }
            if (!string.IsNullOrEmpty(orderExpression))
            {
                query = query.OrderBy(orderExpression.TrimEnd(new char[] { ',', ' ' }));
            }
            else
            {
                var column = columns.Where(u => u.Orderable == true).FirstOrDefault();
                if (column == null)
                {
                    column = columns.FirstOrDefault();
                }
                var expresionFixed = string.Format("{0} {1}, ", column.Data, "ascending");
                query = query.OrderBy(expresionFixed.TrimEnd(new char[] { ',', ' ' }));
            }

            //paginacao
            return query.Skip(start).Take(length);
        }


        public static IList<T> OrderBy<T>(this IList<T> query, ColumnCollection columns, int start, int length)
        {
            string orderExpression = string.Empty;

            foreach (var column in columns.GetSortedColumns().OrderBy(p => p.OrderNumber))
            {
                if (column.SortDirection == Column.OrderDirection.Ascendant)
                    orderExpression += string.Format("{0} {1}, ", column.Data, "ascending");
                else
                    orderExpression += string.Format("{0} {1}, ", column.Data, "descending");
            }

            //paginacao
            return query;
        }
    }
}