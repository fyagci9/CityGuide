using cityguide.Models;
using Microsoft.EntityFrameworkCore;

namespace cityguide.Data
{
    public class AppRepository : IAppRepository
    {
        private DataContext _context;

        public AppRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity)
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity)
        {
            _context.Remove(entity);
        }

        public List<City> GetCities()
        {
            var cities = _context.Cities.Include(c=>c.Photos).ToList(); 
            return cities;
        }

        public City GetCityById(int cityId)
        {
            var city = _context.Cities.Include(c => c.Photos).FirstOrDefault(c=>c.Id == cityId);
            return city;
        }

        public Photo GetPhoto(int id)
        {
            var photos= _context.Photos.FirstOrDefault(p=>p.Id == id);
            return photos;
        }

        public List<Photo> GetPhotosByCity(int id)
        {
            var photos = _context.Photos.Where(p=>p.CityId == id).ToList();
            return photos;
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
