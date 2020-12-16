namespace BookShop.Models
{
    public  class FilterOptions
    {
        public string NameOfBook { get; set; }

        public decimal Price { get; set; } = 0;

        public string Author { get; set; }

        public string Genere { get; set; }
    }
}
