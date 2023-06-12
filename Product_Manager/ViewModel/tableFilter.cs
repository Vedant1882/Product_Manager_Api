namespace Product_Manager.ViewModel
{
    public class tableFilter
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }

        public string? searchValue { get; set; }

        public List<string>? displayedHeaders { get; set; } 
    }
}
