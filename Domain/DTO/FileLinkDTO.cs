﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiniEshop.Domain.DTO
{
    public class FileLinkDTO
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FileUrl { get; set; }
    }
}
