using System;
using System.ComponentModel.DataAnnotations;

namespace BookShop.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название книги  не может быть пустым")]
        public string Name { get; set; }

        public string Author { get; set; }

        [Required(ErrorMessage = "укажите цену")]
        public decimal Price { get; set; }

        public string ImageURL { get; set; }

        public string OriginalPageURL { get; set; }

        public string Genre { get; set; }

        public string BookDescription { get; set; }

        public DateTime PublicationDate { get; set; }

    }
}
