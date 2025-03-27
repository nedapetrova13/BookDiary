﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookDiary.Models
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Името е заядължително")]
        public string Name { get; set; } 
        public ICollection<BookTag> BookTags { get; set; }  
    }
}
