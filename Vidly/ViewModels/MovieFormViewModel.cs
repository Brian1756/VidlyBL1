using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    public class MovieFormViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }

        public Movie Movie {get; set;}

        public string ViewTitle
        { get
            {
                if (Movie == null || Movie.Id == 0)
                {
                    return "New Movie";
                }
                else
                {
                    return "Edit Movie";

                }
            }
        }
       
    }
}