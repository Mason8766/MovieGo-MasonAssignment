using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieGo_MasonAssignment.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public List<Review> Reviews { get; set; }//linked with review

    }
}