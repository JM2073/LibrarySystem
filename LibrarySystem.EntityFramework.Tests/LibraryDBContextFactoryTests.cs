using LibrarySystem.Domain.Models;

namespace LibrarySystem.EntityFramework.Tests;

[TestFixture]
public class Tests
{
    [Test]
    public async Task Test1()
    {
        await using var _db = new LibraryDBContextFactory().CreateDbContext();
        
            var value = await _db.Users.AddAsync(new User
            {
                Id = 1,
                LibraryCardNumber = "3537ec73-ef63-459d-9ab2-0e3ac48272b9",
                Name = "John Smith",
                PhoneNumber = "555-555-5555",
                Email = "johnsmith@email.com",
                AccountType = AccountType.Librarian,
                Books = null,
                Logs = null
            });
            
            await _db.SaveChangesAsync();
        
        
        Assert.Pass();
    }
}