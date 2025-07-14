namespace DGPCE.Sigemad.Application.Specifications
{
    public abstract class SpecificationParams
    {
        public string? Sort { get; set; }

        private int _pageIndex = 0;
        public int PageIndex
        {
            get => _pageIndex;
            set => _pageIndex = (value < 0) ? 0 : value;
        }

        private const int MaxPageSize = 99999;
        private int _pageSize = 5;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Search { get; set; }
    }
}
