using System;

namespace liteclerk_api.Utilities
{
    public class PaginationMetaData
    {
        public PaginationMetaData(int totalCount, int currentPage, int itemsPerPage)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage);
        }

        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}
