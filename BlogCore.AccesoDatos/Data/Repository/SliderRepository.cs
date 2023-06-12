using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.AccesoDatos.Data.Repository.IRepository;
using BlogCore.Data;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository
{
    public class SliderRepository : Repository<Slider>, ISliderRepository
    {
        private readonly ApplicationDbContext _db;

        public SliderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Slider ExisteSlider(string nombreSlider)
        {
            return _db.Slider.FirstOrDefault(s => s.Nombre == nombreSlider);
        }

        public Slider ExisteSliderSinEllaMisma(string nombreSlider, int id)
        {
            return _db.Slider.FirstOrDefault(s => s.Nombre == nombreSlider && s.Id != id);
        }

        public void Update(Slider slider)
        {
            var objDesdeDb = _db.Slider.FirstOrDefault(s => s.Id == slider.Id);
            objDesdeDb.Nombre = slider.Nombre;
            objDesdeDb.Estado = slider.Estado;
            objDesdeDb.UrlImagen = slider.UrlImagen;
            _db.SaveChanges();
        }
    }
}