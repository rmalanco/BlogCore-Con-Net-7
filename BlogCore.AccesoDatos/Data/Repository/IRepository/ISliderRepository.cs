using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogCore.Models;

namespace BlogCore.AccesoDatos.Data.Repository.IRepository
{
    public interface ISliderRepository : IRepository<Slider>
    {
        void Update(Slider slider);
        Slider ExisteSlider(string nombreSlider);
        Slider ExisteSliderSinEllaMisma(string nombreSlider, int id);
    }
}