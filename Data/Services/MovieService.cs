using eTicket.Data.Base;
using eTicket.Data.ViewModels;
using eTicket.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTicket.Data.Services
{
    public class MovieService : EntityBaseRepository<Movie>,IMovieService
    {       
        private readonly AppDbContext _context;
        public MovieService(AppDbContext context):base(context)
        {
            _context = context;
        }

        public async Task AddNewMovieAsync(MovieViewModel data)
        {
            var movie = new Movie()
            {
                Name = data.Name,
                Description = data.Description,
                Price = data.Price,
                ImageURL = data.ImageURL,
                CinemaId = data.CinemaId,
                StartDate = data.StartDate,
                EndDate = data.EndDate,
                MovieCategory = data.MovieCategory,
                ProducerId = data.ProducerId
            };
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();

            foreach (var actorId in data.ActorIds)
            {
                var newActorMovie = new Actor_Movie()
                {
                    MovieId = movie.Id,
                    ActorId = actorId
                };
                await _context.Actors_Movies.AddAsync(newActorMovie);
                await _context.SaveChangesAsync();
            
            }
        }

        public async Task<Movie> GetMovieByIdAsync(int id)
        {
            var movieDetails =await _context.Movies.Include(c=>c.Cinema)
                .Include(p=>p.Producer)
                .Include(am=>am.Actors_Movies)
                .ThenInclude(a=>a.Actor)
                .FirstOrDefaultAsync(n=>n.Id==id);
            return movieDetails;
        }

        public async Task<NewMovieDropdownsVM> GetNewMovieDropdownsValue()
        {
            var result = new NewMovieDropdownsVM()
            {
                Actors=await _context.Actors.OrderBy(x => x.FullName).ToListAsync(),
                Cinemas=await _context.Cinemas.OrderBy(x => x.Name).ToListAsync(),
                Producers=await _context.Producers.OrderBy(x => x.FullName).ToListAsync(),
            };
            return result;
        }
    }
}
