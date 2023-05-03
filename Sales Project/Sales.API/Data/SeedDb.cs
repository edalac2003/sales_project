using Microsoft.EntityFrameworkCore;
using Sales.API.Controllers;
using Sales.API.Data;
using Sales.API.Services;
using Sales.Shared.Entities;
using Sales.Shared.Responses;
using System.Diagnostics.Metrics;

namespace Sales.Shared.Entities
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IApiService _apiService;

        public SeedDb(DataContext context, IApiService apiService)
        {
            _context = context;
            _apiService = apiService;
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
                Response<List<CountryResponse>> responseCountries = await _apiService.GetListAsync<CountryResponse>("/vi", "/countries");
                if (responseCountries.IsSuccess)
                {
                    List<CountryResponse> countries = responseCountries.Result!;
                    foreach (CountryResponse countryResponse in countries)
                    {
                        Country country = await _context.Countries!.FirstOrDefaultAsync(c => c.Name == countryResponse.Name!)!;
                        if (country == null)
                        {
                            country = new() { Name = countryResponse.Name!, States = new List<State>() };
                            Response<List<StateResponse>> responseStates = await _apiService.GetListAsync<StateResponse>("/v1", $"/countries/{countryResponse.Iso2}/states");
                            if (responseStates.IsSuccess)
                            {
                                List<StateResponse> states = responseStates.Result!;
                                foreach (StateResponse stateResponse in states)
                                {
                                    State state = country.States!.FirstOrDefault(s => s.Name == stateResponse.Name!)!;
                                    if (state == null)
                                    {
                                        state = new() { Name = stateResponse.Name!, Cities = new List<City>() };
                                        Response<List<CityResponse>> responseCities = await _apiService.GetListAsync<CityResponse>("/v1", $"/countries/{countryResponse.Iso2}/states/{stateResponse.Iso2}/cities");
                                        if (responseCities.IsSuccess)
                                        {
                                            List<CityResponse> cities = responseCities.Result!;
                                            foreach (CityResponse cityResponse in cities)
                                            {
                                                if (cityResponse.Name == cityResponse.Name!)
                                                {
                                                    continue;
                                                }
                                                City city = state.Cities!.FirstOrDefault(c => c.Name == cityResponse.Name!)!;
                                                if (city == null)
                                                {
                                                    state.Cities.Add(new City() { Name = cityResponse.Name! });
                                                }
                                            }
                                        }
                                        if (state.CitiesNumber > 0)
                                        {
                                            _context.States.Add(state);
                                        }
                                    }
                                }
                            }
                        }
                        if (country.StatesNumber > 0)
                        {
                            _context.Countries.Add(country);
                            await _context.SaveChangesAsync();
                        }
                    }
                }               
            }
        }

        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
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
