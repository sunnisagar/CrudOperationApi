using CrudOperationApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationApi.Repository
{
    public class BrandRepository : IBrand
    {
        private readonly BrandContext _dbcontext;

        public BrandRepository(BrandContext dbcontext)
        {
            this._dbcontext = dbcontext;
        }

        public async Task<ActionResult<IEnumerable<Brand>>> GetBrands()
        {
            return await this._dbcontext.brands.ToListAsync();
        }

    }
}
