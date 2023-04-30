using Sales.API.Data;
using Sales.Shared.Entities;

namespace Sales.Shared.Entities
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync();
            await CheckCountriesAsync();
            await CheckCategoriesAsync();
        }
        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country { Name = "Colombia" });
                _context.Countries.Add(new Country { Name = "Estados Unidos" });
            }
            await _context.SaveChangesAsync();
        }

        private async Task CheckCategoriesAsync()
        {
            if(!_context.Categories.Any())
            {
                _context.Categories.Add(new Category { Name = "Licores" });
                _context.Categories.Add(new Category { Name = "Carnes" });
                _context.Categories.Add(new Category { Name = "Aseo Personal" });
                _context.Categories.Add(new Category { Name = "Aseo Hogar" });
                _context.Categories.Add(new Category { Name = "Electrodomesticos" });
                _context.Categories.Add(new Category { Name = "Tecnología" });
                _context.Categories.Add(new Category { Name = "Moda / Vestuario" });
                _context.Categories.Add(new Category { Name = "Hogar" });
                _context.Categories.Add(new Category { Name = "Ferreteria" });
                _context.Categories.Add(new Category { Name = "Decoración" });
                _context.Categories.Add(new Category { Name = "Panadería" });
                _context.Categories.Add(new Category { Name = "Frutas / Verduras" });
            }
            await _context.SaveChangesAsync();
        }
    }
}
