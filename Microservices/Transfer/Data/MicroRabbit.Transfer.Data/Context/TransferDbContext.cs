using System.IO;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MicroRabbit.Transfer.Data.Context {
    public class TransferDbContext : DbContext {
        public TransferDbContext (DbContextOptions options) : base (options) { }

        public DbSet<TransferLog> TransferLogs { get; set; }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TransferDbContext> {
            //.AddJsonFile(@Directory.GetCurrentDirectory() + "/../MyCookingMaster.API/appsettings.json")
            public TransferDbContext CreateDbContext (string[] args) {
                IConfigurationRoot configuration = new ConfigurationBuilder ()
                    .SetBasePath (Directory.GetCurrentDirectory ())
                    .AddJsonFile (@Directory.GetCurrentDirectory () + "/../../Api/MicroRabbit.Transfer.Api/appsettings.json")
                    .Build ();
                var builder = new DbContextOptionsBuilder<TransferDbContext> ();
                var connectionString = configuration.GetConnectionString ("TransferDbConnection");
                builder.UseSqlServer (connectionString);
                return new TransferDbContext (builder.Options);
            }
        }
    }
}