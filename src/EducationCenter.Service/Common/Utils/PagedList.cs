﻿using Microsoft.EntityFrameworkCore;

namespace EducationCenter.Service.Common.Utils;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
    public PagedList(List<T> items, int count,
        int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        AddRange(items);
    }
    public async static Task<PagedList<T>> ToPagedListAsync(
        IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip(
            (pageNumber - 1) * pageSize).Take(pageSize)
            .ToListAsync();
        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
    public async static Task<PagedList<T>> ToPagedListAsync(
        IQueryable<T> source, PaginationParams @params)
    {
        var count = await source.CountAsync();
        var items = await source.Skip(
            (@params.PageNumber - 1) * @params.PageSize).Take(@params.PageSize)
            .ToListAsync();
        return new PagedList<T>(items, count, @params.PageNumber, @params.PageSize);
    }
}
