using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Models
{
    public static class FilterOptions
    {
            public static string NameofBook { get; set; }
            public static decimal Price { get; set; } = 0;
            public static string Author { get; set; }
            public static string Genere { get; set; }
    }
}
