using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace LibrarySystem.EntityFramework;

public class LibraryDBContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args = null)
    {
        return new LibraryDbContext(new DbContextOptionsBuilder().UseSqlite("Data Source=Data.db").Options);
    }
}