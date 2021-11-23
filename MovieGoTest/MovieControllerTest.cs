using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieGo_MasonAssignment.Controllers;
using MovieGo_MasonAssignment.Data;
using MovieGo_MasonAssignment.Models;
using System;
using System.Collections.Generic;

namespace MovieGoTest
{
    [TestClass]
    public class MovieControllerTest
    {

        private ApplicationDbContext _context; //db connection
        private MoviesController controller;
        List<Movie> movies = new List<Movie>();
        [TestInitialize]
        public void TestInitialize()
        {
            //sets up the db (in memory)
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);

            //movie
            
            var Genre = new Genre
            {
                GenreId = 1,
                GenreName = "Action"
            };
            var movie = new Movie
            {
                MovieId = 1,
                Name = "Rambo",
                Year = "1971",
                Rating = 7,
                GenreId = 1,
                Genre = Genre
            };
            var movieTwo = new Movie
            {
                MovieId = 5,
                Name = "Predator",
                Year = "1981",
                Rating = 6,
                GenreId = 1,
                Genre = Genre
            };
            movies.Add(movie);
            movies.Add(movieTwo);
            controller = new MoviesController(_context);

            foreach(var x in movies)
            {
                _context.Movies.Add(x);
            }
            _context.SaveChanges();
        }
        #region index

        //INDEX TESTING
        [TestMethod]
        public void IndexLoadViewCorrect()
        {
            //act
            var results = (ViewResult)controller.Index().Result;
            //assert
            Assert.AreEqual("Index", results.ViewName);
        }
        [TestMethod]
        public void IndexLoadMovies()
        {
            //act
            var results = (ViewResult)controller.Index().Result;
            List<Movie> model = (List<Movie>)results.Model;
            //assert
            CollectionAssert.AreEqual(movies, model);
        }
        #endregion

        //Details test
        #region Details
        [TestMethod]
        public void DetailsNoId()
        {
            //act
            var results = (ViewResult)controller.Details(null).Result;
            //assert
            Assert.AreEqual("404", results.ViewName);
        }
        [TestMethod]
        public void DetailsInvalidId()
        {
            //act
            var results = (ViewResult)controller.Details(1000).Result;
            //assert
            Assert.AreEqual("404", results.ViewName);
        }
        [TestMethod]
        public void DetailsValidId()
        {
            //act
            var results = (ViewResult)controller.Details(1).Result;
            Movie movie = (Movie)results.Model;
            //assert
            Assert.AreEqual(movies[0], movie);
        }
        [TestMethod]
        public void DetailsValidIdView()
        {
            //act
            var results = (ViewResult)controller.Details(1).Result;
         
            //assert
            Assert.AreEqual("details", results.ViewName);
        }
        #endregion

    }
}
