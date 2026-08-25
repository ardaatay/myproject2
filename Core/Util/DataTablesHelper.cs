using System.Linq.Dynamic.Core;
using Dto.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace Core.Util;

public static class DataTablesHelper
{
    public static async Task<DataTablesResponse<T>> ProcessAsync<T>(
        IQueryable<T> query,
        DataTablesRequest request) where T : class
    {
        var totalRecords = await query.CountAsync();
        var dataQuery = query;

        // Global Arama
        if (!string.IsNullOrEmpty(request.Searchs?.Value))
        {
            var search = request.Searchs.Value.ToLower();
            var stringProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType == typeof(string))
                .ToList();

            if (stringProperties.Any())
            {
                var searchPredicate = string.Join(" || ", stringProperties.Select(p => $"{p.Name} != null && {p.Name}.ToLower().Contains(@0)"));
                dataQuery = dataQuery.Where(searchPredicate, search);
            }
        }

        // Kolon Bazlı Arama
        if (request.Columns != null)
        {
            foreach (var column in request.Columns.Where(c => c.Searchable && !string.IsNullOrEmpty(c.Search?.Value)))
            {
                var val = column.Search?.Value?.ToLower();
                if (string.IsNullOrEmpty(val)) continue;

                var propName = column.Data;
                var prop = typeof(T).GetProperties()
                    .FirstOrDefault(p => string.Equals(p.Name, propName, StringComparison.OrdinalIgnoreCase));

                if (prop != null)
                {
                    if (prop.PropertyType == typeof(string))
                    {
                        dataQuery = dataQuery.Where($"{prop.Name} != null && {prop.Name}.ToLower().Contains(@0)", val);
                    }
                    else
                    {
                         // Diğer tipler (int, DateTime vs) için ToString() ile arama
                        dataQuery = dataQuery.Where($"{prop.Name} != null && {prop.Name}.ToString().ToLower().Contains(@0)", val);
                    }
                }
            }
        }

        var filteredRecords = await dataQuery.CountAsync();

        // Sıralama
        if (request.Orders != null && request.Orders.Any() && request.Columns != null)
        {
            var order = request.Orders.First();
            if (order.Column < request.Columns.Count)
            {
                var columnData = request.Columns[order.Column].Data;
                var direction = (order.Dir ?? "asc").ToLower();

                if (!string.IsNullOrEmpty(columnData))
                {
                    var prop = typeof(T).GetProperties()
                        .FirstOrDefault(p => string.Equals(p.Name, columnData, StringComparison.OrdinalIgnoreCase));

                    if (prop != null)
                    {
                        dataQuery = dataQuery.OrderBy($"{prop.Name} {direction}");
                    }
                }
            }
        }

        // Sayfalama
        var data = await dataQuery
            .Skip(request.Start)
            .Take(request.Length)
            .ToListAsync();

        return new DataTablesResponse<T>
        {
            Draw = request.Draw,
            RecordsTotal = totalRecords,
            RecordsFiltered = filteredRecords,
            Data = data
        };
    }
}
