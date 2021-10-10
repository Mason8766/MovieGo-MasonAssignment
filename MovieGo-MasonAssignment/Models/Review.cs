using System;
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

        [Display(Name = "Review")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        public string ReviewPara { get; set; }

        [Display(Name = "Rating")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        [Range(0, 10, ErrorMessage = "Movies can only be rated from 0-10")]
        public double UserRating { get; set; }

        [Display(Name = "Movie")]
        public int MovieId { get; set; }

        [Display(Name = "User")]
        public int UserId { get; set; }

        public User User { get; set; }
        public Movie Movie { get; set; }
    }
}