using System;
using System.Web;
using Playground.Domain;
using Playground.Dto;

namespace PlaygroundApi.Navigation
{
    /// <summary>
    /// Extensions for Paged list.
    /// </summary>
    public static class PagedListExtensions
    {
        /// <summary>
        /// Fills navigation for the paged list with the hyperlinks: self, previous and next, when applicable.
        /// </summary>
        public static void BuildNavigationLinks<T>(this PagedListDto<T> pagedList, Uri currentUri)
        {
            pagedList.Links[ResourceBase.RelationNameSelf] = currentUri.AbsoluteUri;
            var queryString = HttpUtility.ParseQueryString(currentUri.Query);
            SearchBase searchParam;
            var skipParameterName = nameof(searchParam.Skip);

            if (pagedList.HasNextPage)
            {
                var nbElementsToSkip = pagedList.LastItemOnPage;
                queryString.Set(skipParameterName, nbElementsToSkip.ToString());
                var newUri = new UriBuilder(currentUri) { Query = queryString.ToString() }.Uri;
                pagedList.Links[ResourceBase.RelationNameNext] = newUri.AbsoluteUri;
            }

            if (pagedList.HasPreviousPage)
            {
                var nbElementsToSkip = pagedList.FirstItemOnPage - 1 - pagedList.PageSize;
                queryString.Set(skipParameterName, nbElementsToSkip.ToString());
                var newUri = new UriBuilder(currentUri) { Query = queryString.ToString() }.Uri;
                pagedList.Links[ResourceBase.RelationNamePrevious] = newUri.AbsoluteUri;
            }
        }

        /// <summary>
        /// Fills navigation for the paged list with the hyperlinks: self, previous and next, when applicable.
        /// </summary>
        public static void BuildNavigationLinks<T>(this PagedListDto<T> pagedList, string currentUri)
        {
            pagedList.BuildNavigationLinks(new Uri(currentUri));
        }
    }
}
