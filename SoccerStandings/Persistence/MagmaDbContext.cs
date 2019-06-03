using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SoccerStandings.Persistence
{
    public abstract class MagmaDbContext : DbContext
    {
        public string SchemaName { get; protected set; }

        protected MagmaDbContext()
        {
        }

        protected MagmaDbContext(string schema) : this()
        {
            this.SchemaName = schema;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseLoggerFactory(this.GetLoggerFactory());

            // optionsBuilder.UseNpgsql("Host=localhost;Database=magmasystems;Username=magmasystems;Password=my_pw");
            optionsBuilder.UseNpgsql("Host=localhost;Database=magmasystems");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(this.SchemaName);
        }

        protected virtual ILoggerFactory GetLoggerFactory()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                   builder.AddConsole().AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information));
            return serviceCollection.BuildServiceProvider().GetService<ILoggerFactory>();
        }

        public bool DropTable(string name)
        {
            try
            {
                string command = $"DROP TABLE IF EXISTS {name};";
                this.Database.ExecuteSqlCommand(command);
                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }

        public bool RemoveAll(string name)
        {
            try
            {
                string command = $"DELETE * FROM {name};";
                this.Database.ExecuteSqlCommand(command);
                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }

        public bool RemoveAll<T>(DbSet<T> dbSet) where T : class
        {
            try
            {
                var entities = dbSet.Select(x => x).ToList();
                this.RemoveRange(entities);
                this.SaveChanges();
                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                return false;
            }
        }
    }
}
