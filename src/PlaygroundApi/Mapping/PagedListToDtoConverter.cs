using System.Collections.Generic;
using AutoMapper;
using Playground.Dto;
using X.PagedList;

namespace PlaygroundApi.Mapping
{
    /// <summary>
    /// Converts a paged list to a paged list dto.
    /// </summary>
    /// <typeparam name="T1">Source type.</typeparam>
    /// <typeparam name="T2">DestinationDate</typeparam>
    public class PagedListToDtoConverter<T1, T2> : ITypeConverter<IPagedList<T1>, PagedListDto<T2>>
    {
        public PagedListDto<T2> Convert(IPagedList<T1> source, PagedListDto<T2> destination, ResolutionContext context)
        {
            var items = context.Mapper.Map<List<T2>>(source);

            return new PagedListDto<T2>
            {
                Items = items,
                FirstItemOnPage = source.FirstItemOnPage,
                HasNextPage = source.HasNextPage,
                HasPreviousPage = source.HasPreviousPage,
                IsFirstPage = source.IsFirstPage,
                IsLastPage = source.IsLastPage,
                LastItemOnPage = source.LastItemOnPage,
                PageCount = source.PageCount,
                PageNumber = source.PageNumber,
                PageSize = source.PageSize,
                TotalItemCount = source.TotalItemCount
            };
        }
    }
}
