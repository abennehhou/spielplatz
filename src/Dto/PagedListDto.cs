using System.Collections.Generic;

namespace Playground.Dto
{
    /// <summary>
    /// List of items provided with pagination.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedListDto<T> : ResourceBase
    {
        /// <summary>
        /// List of items.
        /// </summary>
        public IList<T> Items { get; set; }

        /// <summary>
        /// One-based index of the first item in the paged subset.
        /// </summary>
        public int FirstItemOnPage { get; set; }

        /// <summary>
        /// Returns true if this is not the last subset within the superset.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Returns true if this is not the first subset within the superset.
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Returns true if this is the first subset within the superset.
        /// </summary>
        public bool IsFirstPage { get; set; }

        /// <summary>
        /// Returns true if this is the last subset within the superset.
        /// </summary>
        public bool IsLastPage { get; set; }

        /// <summary>
        /// One-based index of the last item in the paged subset.
        /// </summary>
        public int LastItemOnPage { get; set; }

        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        public int PageCount { get; set; }

        /// <summary>
        /// One-based index of this subset within the superset.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Maximum size any individual subset.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        public int TotalItemCount { get; set; }
    }
}
