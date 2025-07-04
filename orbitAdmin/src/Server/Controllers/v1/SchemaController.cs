using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolV01.Infrastructure.Services;
using SchoolV01.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace SchoolV01.Server.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchemaController : ControllerBase
    {
        private readonly EfSchemaService _schemaService;

        public SchemaController(EfSchemaService schemaService)
        {
            _schemaService = schemaService;
        }
        [Authorize(Policy = Permissions.Schema.View)]
        [HttpGet]
        public IActionResult Get()
        {
            var schema = _schemaService.GetSchema();
            return Ok(schema);
        }

        [Authorize(Policy = Permissions.Schema.View)]
        [HttpGet("GetInfoProduct")]
        public async Task<IActionResult> GetInfoProduct(string q)
        {
            var schema =await _schemaService.GetAllProductsAsync(q);
            return Ok(schema);
        }
    }
}
