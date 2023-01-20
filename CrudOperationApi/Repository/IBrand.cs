using CrudOperationApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CrudOperationApi.Repository
{
    public interface IBrand
    {
        public Task<ActionResult<IEnumerable<Brand>>> GetBrands();
    }
}
