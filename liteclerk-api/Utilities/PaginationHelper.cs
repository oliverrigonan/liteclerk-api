using System.Collections;

namespace liteclerk_api.Utilities
{
    public class PaginationHelper
    {

        private const int _maxItemsPerPage = 50;
        private int itemsPerPage;

        public int PageNumber { get; set; } = 1;

        public int ItemsPerPage
        {
            get => itemsPerPage;

            set => itemsPerPage = value > _maxItemsPerPage ? _maxItemsPerPage : value;
        }
    }

    public class PaginationData
    {
        public IEnumerable Data { get; set; }
        public PaginationMetaData MetaData { get; set; }
    }
}
