using System.ComponentModel.DataAnnotations;

namespace CaseAPI.Dtos.Product
{
    public class ResultProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }

    }
}
