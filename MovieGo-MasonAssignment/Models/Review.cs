﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGo_MasonAssignment.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        public string ReviewPara { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        [Range(0, 10, ErrorMessage = "Movies can only be rated from 0-10")]
        public double UserRating { get; set; }

        public int MovieId { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}