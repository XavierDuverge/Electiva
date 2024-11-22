

using LoadDWVentas.Data.Entities.DwVentas;
using Microsoft.EntityFrameworkCore;

namespace LoadDWVentas.Data.Context
{
    public class DbSalesContext : DbContext
    {
        public DbSalesContext(DbContextOptions<DbSalesContext> options) : base(options) 
        {
            
        }

        #region "Db Sets"
        public DbSet<DimEmployee> DimEmployees { get; set; }
        public DbSet<DimProductCategory> DimProductCategories { get; set; }
        public DbSet<DimCustomer> DimCustomers { get; set; }
        public DbSet<DimShipper> DimShippers { get; set; }
        #endregion
    }
}
