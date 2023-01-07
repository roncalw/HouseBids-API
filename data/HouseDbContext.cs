public class HouseDbContext : DbContext
{


    protected readonly IConfiguration Configuration;

    public HouseDbContext(DbContextOptions<HouseDbContext> options, IConfiguration configuration) : base(options) 
    { 
        Configuration = configuration; 
    }
    public DbSet<HouseEntity> Houses => Set<HouseEntity>();
    public DbSet<BidEntity> Bids => Set<BidEntity>();
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        //The following stores the database at the following location: C:\Users\Carlo\AppData\Local
        //var folder = Environment.SpecialFolder.LocalApplicationData;
        //var path = Environment.GetFolderPath(folder);
        //options.UseSqlite($"Data Source={Path.Join(path, "houses1.db")}");

        //The following uses the appsettings.json file to make it configurable
        options.UseSqlite(Configuration.GetConnectionString("WebApiDatabase"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        SeedData.Seed(modelBuilder);
    }

}