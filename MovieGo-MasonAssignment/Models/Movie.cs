using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGo_MasonAssignment.Models
{
    public class Movie
    {
        public int MovieId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        public string Year { get; set; }

       
        [Required(AllowEmptyStrings = false, ErrorMessage = "Field required")]
        [Range(0, 10, ErrorMessage = "Movies can only be rated from 0-10")]
        public double Rating { get; set; }

        [Display(Name = "Genre")]
        public int GenreId { get; set; }

        public Genre Genre { get; set; } //linked with genre
        public List<Review> Reviews { get; set; }//linked with reviews
    }
}