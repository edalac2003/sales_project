using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sales.API.Data;

namespace Sales.API.Controllers
{
    [ApiController]
    [Route("/api/products")]
    public class ProductsController: ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAsync()
        {
            return Ok(await _context.Products
                .ToListAsync());
        }
    }
}
