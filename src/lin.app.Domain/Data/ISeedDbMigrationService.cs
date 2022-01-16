using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lin.app.Domain.Data
{
    public interface ISeedDbMigrationService
    {
        Task MigrateAsync();
    }
}
