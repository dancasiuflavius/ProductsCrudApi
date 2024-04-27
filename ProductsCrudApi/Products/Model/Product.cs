using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductsCrudApi.Products.Model
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }
        [Required]
        [Column("price")]

        public double Price { get; set; }
        [Required]
        [Column("name")]

        public string Name { get; set; }
        [Required]
        [Column("category")]

        public string Category { get; set; }
        [Required]
        [Column("date_of_fabrication")]

        public DateTime DateOfFabrication { get; set; }

        public override bool Equals(object? obj)
        {
            Product oProd = (Product)obj;

            return this.Id == oProd.Id && this.Category == oProd.Category;
        }

    }
}
