using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MiniEshop.Domain.DTO
{
    public class CategoryDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<CategoryDTO> Children { get; set; }
    }
}
