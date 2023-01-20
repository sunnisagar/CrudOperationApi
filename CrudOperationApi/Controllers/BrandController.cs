using Azure;
using CrudOperationApi.Models;
using CrudOperationApi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CrudOperationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : Controller
    {
        private readonly ILogger<BrandController> _logger;
        private readonly BrandContext _dbcontext;
        private readonly BrandRepository _Repository;
        public BrandController(BrandContext dbcontext, ILogger<BrandController> logger)
        {
            _dbcontext = dbcontext;
            _logger = logger;
            if (_Repository == null) 
            {
                _Repository = new BrandRepository(_dbcontext);
            }
        }
        
        [HttpGet]
        [Route("GetAllBrand")]
        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            this._logger.LogInformation("Init GetBrands Method...");
            if (_Repository == null)
            {
                return NotFound();
            }
            return await _Repository.GetBrands();
        }

        [HttpGet]
        [Route("GetBrandById/{id}")]
        public async Task<ActionResult<Brand>> GetBrands(int id)
        {
            if (_dbcontext == null)
            {
                return NotFound();
            }
            var brand = await _dbcontext.brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return brand;
        }

        [HttpPost]
        [Route("AddBrand")]
        public async Task<ActionResult> PostBrand(Brand brand)
        {
            _dbcontext.Add(brand);
            await _dbcontext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBrands), new { id = brand.ID }, brand);
        }

        [HttpPut]
        [Route("UpdateBrand")]
        public async Task<ActionResult> PutBrand(int id, Brand brand)
        {
            if (id != brand.ID)
            {
                return BadRequest();
            }
            _dbcontext.Entry(brand).State = EntityState.Modified;

            try
            {
                await _dbcontext.SaveChangesAsync();
            }
            catch (DBConcurrencyException)
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

        private bool BrandAvailable(int id)
        {
            return (_dbcontext.brands?.Any(x => x.ID == id)).GetValueOrDefault();
        }

        [HttpDelete]
        [Route("DeleteBrandById/{id}")]
        public async Task<ActionResult> DeleteBrand(int id)
        {
            if (_dbcontext.brands == null)
            {
               return NotFound();
            }

            var brand = await _dbcontext.brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }

            _dbcontext.brands.Remove(brand);
            await _dbcontext.SaveChangesAsync();

            return Ok();
        }

        [HttpPatch]
        [Route("ReplaceBrandTitle/{id}")]
        public async Task<ActionResult> PatchBrand(int id, JsonPatchDocument<Brand> brandModel)
        {
            var brands = await _dbcontext.brands.FindAsync(id);

            if (brands != null)
            {
                brandModel.ApplyTo(brands);
                _dbcontext.SaveChanges();
            }
            else
            { 
                return BadRequest();
            }

            return Ok();
        }
    }
}
