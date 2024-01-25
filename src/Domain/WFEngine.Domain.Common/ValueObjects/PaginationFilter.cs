using WFEngine.Domain.Common.Enums;

namespace WFEngine.Domain.Common.ValueObjects
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public EnumSortDirection SortDirection { get; set; } = EnumSortDirection.Ascending;

        public PaginationFilter()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
