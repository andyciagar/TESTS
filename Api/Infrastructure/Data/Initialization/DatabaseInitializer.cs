using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure.Data.Initialization;

public sealed class DatabaseInitializer(ApplicationDbContext dbContext)
{
    public async Task InitializeAsync()
    {
        await dbContext.Database.EnsureCreatedAsync();
    }
}