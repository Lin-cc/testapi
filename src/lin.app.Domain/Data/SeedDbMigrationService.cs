using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;

namespace lin.app.Domain.Data
{
    public class SeedDbMigrationService: ITransientDependency
    {
        public ILogger<SeedDbMigrationService> Logger { get; set; }

        private readonly IDataSeeder _dataSeeder;
        private readonly IEnumerable<ISeedDbMigrationService> _dbSchemaMigrators;

        //public SeedDbMigrationService(ILogger<SeedDbMigrationService> logger, IDataSeeder dataSeeder, IEnumerable<ISeedDbMigrationService> dbSchemaMigrators)
        //{
        //    Logger = NullLogger<SeedDbMigrationService>.Instance;
        //    _dataSeeder = dataSeeder;
        //    _dbSchemaMigrators = dbSchemaMigrators;
        //}

        protected IGuidGenerator GuidGenerator { get; }
        protected IIdentityRoleRepository RoleRepository { get; }
        protected IIdentityUserRepository UserRepository { get; }
        protected ILookupNormalizer LookupNormalizer { get; }
        protected IdentityUserManager UserManager { get; }
        protected IdentityRoleManager RoleManager { get; }
        protected ICurrentTenant CurrentTenant { get; }
        protected IOptions<IdentityOptions> IdentityOptions { get; }

        public SeedDbMigrationService(
            IGuidGenerator guidGenerator,
            IIdentityRoleRepository roleRepository,
            IIdentityUserRepository userRepository,
            ILookupNormalizer lookupNormalizer,
            IdentityUserManager userManager,
            IdentityRoleManager roleManager,
            ICurrentTenant currentTenant,
            IOptions<IdentityOptions> identityOptions)
        {
            GuidGenerator = guidGenerator;
            RoleRepository = roleRepository;
            UserRepository = userRepository;
            LookupNormalizer = lookupNormalizer;
            UserManager = userManager;
            RoleManager = roleManager;
            CurrentTenant = currentTenant;
            IdentityOptions = identityOptions;
        }


        public async Task MigrateAsync()
        {
            await SeedAsync();
            return ;
            //只需要基础数据，数据结构已经更新数据库里了

            //var initialMigrationAdded = AddInitialMigrationIfNotExist();

            //if (initialMigrationAdded)
            //{
            //    return;
            //}

            //Logger.LogInformation("Started database migrations...");

            //await MigrateDatabaseSchemaAsync();
            //await SeedDataAsync();

            //Logger.LogInformation($"Successfully completed host database migrations.");


            //var migratedDatabaseSchemas = new HashSet<string>();
         

            Logger.LogInformation("Successfully completed all database migrations.");
            Logger.LogInformation("You can safely end this process...");
        }

       
        #region IdentitySeed 用户模块种子数据生成

        [UnitOfWork]
        public virtual async Task<IdentityDataSeedResult> SeedAsync(
       string adminEmail= IdentityDataSeedContributor.AdminEmailDefaultValue,
       string adminPassword = IdentityDataSeedContributor.AdminPasswordDefaultValue,
       Guid? tenantId = null)
        {
            Check.NotNullOrWhiteSpace(adminEmail, nameof(adminEmail));
            Check.NotNullOrWhiteSpace(adminPassword, nameof(adminPassword));

            using (CurrentTenant.Change(tenantId))
            {
                await IdentityOptions.SetAsync();

                var result = new IdentityDataSeedResult();
                //"admin" user
                const string adminUserName = "admin";
                var adminUser = await UserRepository.FindByNormalizedUserNameAsync(
                    LookupNormalizer.NormalizeName(adminUserName)
                );

                if (adminUser != null)
                {
                    return result;
                }

                adminUser = new IdentityUser(
                    GuidGenerator.Create(),
                    adminUserName,
                    adminEmail,
                    tenantId
                )
                {
                    Name = adminUserName
                };

                (await UserManager.CreateAsync(adminUser, adminPassword, validatePassword: false)).CheckErrors();
                result.CreatedAdminUser = true;

                //"admin" role
                const string adminRoleName = "admin";
                var adminRole =
                    await RoleRepository.FindByNormalizedNameAsync(LookupNormalizer.NormalizeName(adminRoleName));
                if (adminRole == null)
                {
                    adminRole = new IdentityRole(
                        GuidGenerator.Create(),
                        adminRoleName,
                        tenantId
                    )
                    {
                        IsStatic = true,
                        IsPublic = true
                    };

                    (await RoleManager.CreateAsync(adminRole)).CheckErrors();
                    result.CreatedAdminRole = true;
                }

                (await UserManager.AddToRoleAsync(adminUser, adminRoleName)).CheckErrors();

                return result;
            }
        }
        #endregion

        #region 其他项目拷过来的留作参考
        private async Task MigrateDatabaseSchemaAsync()
        {
            //Logger.LogInformation(
            //    $"Migrating schema for {(tenant == null ? "host" : tenant.Name + " tenant")} database...");

            foreach (var migrator in _dbSchemaMigrators)
            {
                await migrator.MigrateAsync();
            }
        }

        private async Task SeedDataAsync(object tenant = null)
        {
            //Logger.LogInformation($"Executing {(tenant == null ? "host" : tenant + " tenant")} database seed...");

            //await _dataSeeder.SeedAsync(new DataSeedContext(null)
            //    .WithProperty(IdentityDataSeedContributor.AdminEmailPropertyName, IdentityDataSeedContributor.AdminEmailDefaultValue)
            //    .WithProperty(IdentityDataSeedContributor.AdminPasswordPropertyName, IdentityDataSeedContributor.AdminPasswordDefaultValue)
            //);
        }

        private bool AddInitialMigrationIfNotExist()
        {
            try
            {
                if (!DbMigrationsProjectExists())
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                if (!MigrationsFolderExists())
                {
                    AddInitialMigration();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning("Couldn't determinate if any migrations exist : " + e.Message);
                return false;
            }
        }

        private bool DbMigrationsProjectExists()
        {
            var dbMigrationsProjectFolder = GetEntityFrameworkCoreProjectFolderPath();

            return dbMigrationsProjectFolder != null;
        }

        private bool MigrationsFolderExists()
        {
            var dbMigrationsProjectFolder = GetEntityFrameworkCoreProjectFolderPath();

            return Directory.Exists(Path.Combine(dbMigrationsProjectFolder, "Migrations"));
        }

        private void AddInitialMigration()
        {
            Logger.LogInformation("Creating initial migration...");

            string argumentPrefix;
            string fileName;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                argumentPrefix = "-c";
                fileName = "/bin/bash";
            }
            else
            {
                argumentPrefix = "/C";
                fileName = "cmd.exe";
            }

            var procStartInfo = new ProcessStartInfo(fileName,
                $"{argumentPrefix} \"abp create-migration-and-run-migrator \"{GetEntityFrameworkCoreProjectFolderPath()}\"\""
            );

            try
            {
                Process.Start(procStartInfo);
            }
            catch (Exception)
            {
                throw new Exception("Couldn't run ABP CLI...");
            }
        }

        private string GetEntityFrameworkCoreProjectFolderPath()
        {
            var slnDirectoryPath = GetSolutionDirectoryPath();

            if (slnDirectoryPath == null)
            {
                throw new Exception("Solution folder not found!");
            }

            var srcDirectoryPath = Path.Combine(slnDirectoryPath, "src");

            return Directory.GetDirectories(srcDirectoryPath)
                .FirstOrDefault(d => d.EndsWith(".EntityFrameworkCore"));
        }

        private string GetSolutionDirectoryPath()
        {
            var currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            while (Directory.GetParent(currentDirectory.FullName) != null)
            {
                currentDirectory = Directory.GetParent(currentDirectory.FullName);

                if (Directory.GetFiles(currentDirectory.FullName).FirstOrDefault(f => f.EndsWith(".sln")) != null)
                {
                    return currentDirectory.FullName;
                }
            }

            return null;
        }

        #endregion



    }
}
