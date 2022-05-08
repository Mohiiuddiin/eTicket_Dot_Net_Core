using eTicket.Data;
using eTicket.Data.Services;
using eTicket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Controllers
{
    public class MoviesController : Controller 
    {
        private readonly IMovieService _service;
        public MoviesController(IMovieService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            //var data = await _service.Movies.Include(x=>x.Cinema).OrderBy(x=>x.Name).ToListAsync();
            var data = await _service.GetAllAsync(n=> n.Cinema);
            return View(data);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }
            await _service.AddAsync(movie);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var producersDetails = await _service.GetByIdAsync(id);
            if (producersDetails == null)
            {
                return View("NotFound");

            }
            return View(producersDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var moviesDetails = await _service.GetByIdAsync(id);
            if (moviesDetails == null)
            {
                return View("NotFound");
            }
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var moviesDetails = await _service.GetByIdAsync(id);
            if (moviesDetails == null)
            {
                return View("NotFound");
            }
            return View(moviesDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, Movie movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }
            await _service.UpdateAsync(id, movie);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var moviesDetails = await _service.GetMovieByIdAsync(id);
            if (moviesDetails == null)
            {
                return View("NotFound");
            }
            return View(moviesDetails);
        }
    }
}
