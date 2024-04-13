using CrudOperationNetCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationNetCore.Controllers
{
 [Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly BrandContext _dbContext;

    public BrandController(BrandContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
    {
        if (_dbContext.Brands == null)
        {
            return NotFound();
        }
        return await _dbContext.Brands.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Brand>> GetBrand(int id)
    {
        if (_dbContext.Brands == null)
        {
            return NotFound();
        }
        var brand = await _dbContext.Brands.FindAsync(id);

        if (brand == null)
        {
            return NotFound();
        }
        return brand;
    }
        [HttpPost]
        public async Task<ActionResult<Brand>> PostBrand(Brand brand)
        {
            _dbContext.Brands.Add(brand);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrand),new {id=brand.ID},  brand);
        }
        [HttpPut]
        public async Task<IActionResult> PutBrand(int id,Brand brand)
        {
            if (id == brand.ID)
            {
                return BadRequest();
            }
            _dbContext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrandAvailable(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (_dbContext.Brands == null)
            {
                return NotFound();
            }
            var brand =await _dbContext.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _dbContext.Brands.Remove( brand);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Brand>>> SearchBrands(string keyword)
        {
            var brands = await _dbContext.Brands
                                        .Where(b => b.Name.Contains(keyword))
                                        .ToListAsync();

            if (brands.Count == 0)
            {
                return NotFound();
            }

            return Ok(brands);
        }
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<Brand>>> FilterBrands(string propertyName, string propertyValue)
        {
            var brands = await _dbContext.Brands
                                        .Where(b => EF.Property<string>(b, propertyName) == propertyValue)
                                        .ToListAsync();

            if (brands.Count == 0)
            {
                return NotFound();
            }

            return Ok(brands);
        }

        [HttpGet("{id}/Brand")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrandProducts(int id)
        {
            var brand = await _dbContext.Brands.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            var products = await _dbContext.Brands
                                            .Where(p => p.ID == id)
                                            .ToListAsync();

            return Ok(products);
        }


        //[HttpGet("sorted")]
        //public async Task<ActionResult<IEnumerable<Brand>>> GetSortedBrands(string sortBy)
        //{
        //    IQueryable<Brand> query = _dbContext.Brands;

        //    switch (sortBy.ToLower())
        //    {
        //        case "name":
        //            query = query.OrderBy(b => b.Name);
        //            break;
        //        case "createdAt":
        //            query = query.OrderBy(b => b.CreatedAt);
        //            break;
        //        // Add more cases for other properties if needed
        //        default:
        //            return BadRequest("Invalid sort property");
        //    }

        //    var brands = await query.ToListAsync();
        //    return Ok(brands);
        //}



        private bool BrandAvailable(int id)
        {
            return (_dbContext.Brands?.Any(x => x.ID == id)).GetValueOrDefault();

        }
}

}
