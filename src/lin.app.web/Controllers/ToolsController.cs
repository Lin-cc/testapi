using lin.app.Domain.Data;
using Microsoft.AspNetCore.Mvc;

namespace lin.app.web.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ToolsController : ControllerBase
    {
        private readonly ILogger<ToolsController> _logger;
        public ToolsController(ILogger<ToolsController> logger)
        {
            _logger = logger;
        }
        [HttpPost]
        [ActionName("dataseed")]
        public async Task DataSeed([FromServices] SeedDbMigrationService dataSeed)
        {
            _logger.LogInformation("Data Seed Start");
            await dataSeed.MigrateAsync();
            _logger.LogInformation("Data Seed End");
        }
    }
}
