using Microsoft.EntityFrameworkCore;

namespace LocadoraDeVeiculos.Infraestrutura.Orm.Compartilhado;

public static class AppDbContextFactory
{
    public static AppDbContext CriarDbContext(string connectionString)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString, opt => opt.EnableRetryOnFailure(3))
            .Options;

        var dbContext = new AppDbContext(options);

        return dbContext;
    }
}