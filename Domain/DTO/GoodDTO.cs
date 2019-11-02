using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.Domain.DTO
{
    public class GoodDTO
    {
        public Guid? Id { get; set; }

        [Required]
        [StringLength(Int32.MaxValue, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Qty { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
