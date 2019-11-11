using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.Domain
{
    public class Good
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "decimal(38, 20)")]
        public decimal Price { get; set; }

        [Required]
        public int Qty { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Guid? ImageUrlId { get; set; }

        [ForeignKey("ImageUrlId")]
        public FileLink FileLink { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
