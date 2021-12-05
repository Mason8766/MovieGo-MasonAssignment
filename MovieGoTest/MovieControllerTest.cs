using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieGo_MasonAssignment.Controllers;
using MovieGo_MasonAssignment.Data;
using MovieGo_MasonAssignment.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
        #region Create
        [TestMethod]
        public void MovieCreateSavesToDb()
        {
             var movie = new Movie
            {
                MovieId = 15,
                Name = "Rambo 2",
                Year = "1985",
                Rating = 7,
                GenreId = 1,

            };
            var results = controller.Create(movie);
            Assert.AreEqual(movie, _context.Movies.ToArray()[2]);
        }

        [TestMethod]
        public void MovieCreateReturnsPost()
        {

            var movieTest = new Movie { };
            //act
            controller.ModelState.AddModelError("Please enter a disciptive key", "Please enter a appropriate key");

            var result = controller.Create(movieTest);
            var viewResult = (ViewResult)result.Result;

            // assert         
            Assert.AreEqual("Create", viewResult.ViewName);
        }
        [TestMethod]
        public void MovieCreateViewDataNull()
        {
            var movie = new Movie { };

            //act
            controller.ModelState.AddModelError("Please enter a disciptive key", "Please enter a appropriate key");
            var result = controller.Create(movie);
            var viewResult = (ViewResult)result.Result;

            // assert            
            Assert.IsNotNull(viewResult.ViewData);
        }

        #endregion
        #region Edit
        //IF ID IS VALID, LOAD EDIT PAGE
        [TestMethod]
        public void MovieEditLoadsViewValidId()
        {
            var result = controller.Edit(5); //ACT
            var viewResult = (ViewResult)result.Result;

            //ASSERT PHASE
            Assert.AreEqual("Edit", viewResult.ViewName);
        }

        [TestMethod]
        public void MovieEditLoadsWithAValidModel()//LOADS EDIT PAGE, WITH THE SAME MODEL DATA AS ID
        {
            //ACT PHASE:
            var result = controller.Edit(5);
            var viewResult = (ViewResult)result.Result;
            Movie model = (Movie)viewResult.Model;


            Assert.AreEqual(_context.Movies.Find(5), model); //ASSERT, EDITING THE MOVIE WITH THE 5TH ID
        }
        //COMPARES VIEWDATA
        [TestMethod]
        public void MovieEditViewData()
        {
            //ACT
            var result = controller.Edit(5);
            var viewResult = (ViewResult)result.Result;
            var viewData = viewResult.ViewData;

            //ASSERT
            Assert.AreEqual(viewData, viewResult.ViewData);
        }

        [TestMethod] //MODEL FOUND AND MODEL GIVEN !=
        public void MovieEditErrorViewInvalidModel()
        {
            //ACT
            var result = controller.Edit(5);
            var viewResult = (ViewResult)result.Result;
            Movie model = (Movie)viewResult.Model;

            //ASSERT
            Assert.AreNotEqual(_context.Movies.FindAsync(10), model);
        }

        //IF EDIT CHANGES ARE SAVED
        [TestMethod]
        public void MovieEditSaveMovie()
        {
            //ACT:
            var movie = movies[0];
            movie.Year = "1999";
            var result = controller.Edit(movie.MovieId, movie);
            var redirectResult = (RedirectToActionResult)result.Result;
            // ASSERT PHASE:
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        //ERROR TEST:
       
        [TestMethod]
        public void MovieEditMovieGoDatabase()//movie not found in db
        {
            //act phase
            var movie = new Movie
            {
                MovieId = 18,
                Name = "Rambo 3",
                Year = "1989",
                Rating = 7,
                GenreId = 1,

            };

            var result = controller.Edit(1, movie);//edit movie with id of 1, passing in movie
            var viewResult = (ViewResult)result.Result;
            // ASSERT Phase 
            Assert.AreEqual("404", viewResult.ViewName);
        }
        //trying to pass a null
        [TestMethod]
        public void MovieEditNullViewError()
        {
            //act phase
            var result = controller.Edit(null);
            var viewResult = (ViewResult)result.Result;
            //ASSERT phase
            Assert.AreEqual("404", viewResult.ViewName);
        }
        //movieid does not match
        [TestMethod]
        public void MovieEditReturnsMovieId()
        {
            //act phase
            var result = controller.Edit(101, movies[0]);
            var viewResult = (ViewResult)result.Result;
            //assert phase
            Assert.AreEqual("404", viewResult.ViewName);
        }
     //passing a invalid id
        [TestMethod]
        public void MovieEditErrorViewInvalidId()
        {
            //act phsae
            var result = controller.Edit(33);
            var viewResult = (ViewResult)result.Result;

            //assert phase
            Assert.AreEqual("404", viewResult.ViewName);
        }
        #endregion
        #region Delete
        //trying to delete a movie, without a propper id
        [TestMethod]
        public void MovieDeleteMovieBasedOnId()
        {
            //act  phase
            var movieId = 1;
            var result = controller.Delete(movieId);
            var viewResult = (ViewResult)result.Result;
            Movie movie = (Movie)viewResult.Model;

            //assert phase
            Assert.AreEqual(movies[0], movie);
        }
        //Movie being dleted has right ID
        [TestMethod]
        public void MovieDeleteCorrectId()
        {
            //act phase
            var movieId = 1;
            var result = controller.Delete(movieId);
            var viewResult = (ViewResult)result.Result;
            //assert phase
            Assert.AreEqual("Delete", viewResult.ViewName);
        }
        // conformation delete
        [TestMethod]
        public void MovieDeleteConfromation()
        {
            //act phase
            var movieId = 1;
            var result = controller.DeleteConfirmed(movieId); 
            var movie = _context.Movies.Find(movieId);

            //assert phase
            Assert.AreEqual(movie, null);
        }
      
        //redirection when asked to confirm
        [TestMethod]
        public void MovieDeleteConfirmedationCausingRedirection()
        {
            //act  phase
            var id = 1;
            var result = controller.DeleteConfirmed(id);
            var actionResult = (RedirectToActionResult)result.Result;

            //assert phase
            Assert.AreEqual("Index", actionResult.ActionName);
        }
        //id passed not found - protect against injection
        [TestMethod]
        public void MovieDeletedIdDoesNotExists()
        {
            //act phase
            var result = controller.Delete(99);
            var viewResult = (ViewResult)result.Result;
            //assert phase
            Assert.AreEqual("404", viewResult.ViewName);
        }
        #endregion
    }
}
