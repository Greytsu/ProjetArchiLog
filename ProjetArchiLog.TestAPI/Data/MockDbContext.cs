using System;
using Microsoft.EntityFrameworkCore;
using ProjetArchiLog.API.Data;
using ProjetArchiLog.API.Models;

namespace ProjetArchiLog.TestAPI.Data;

public class MockDbContext : ArchiDbContext
{
    public MockDbContext(DbContextOptions options):base(options)
    {
    }

    public static MockDbContext GetDbContext(bool withData = true)
    {
        var options = new DbContextOptionsBuilder().UseInMemoryDatabase("dbTest").Options;
        var db = new MockDbContext(options);

        if (!withData) return db;
        
        db.Products.Add(new Product {Id = new Guid("894194aa-611c-4a35-7eac-08d9d75bf80f"), Name = "Toaster",Description = "Machine made to burn bread", Price = 12.99d, Type = "Machine"});
        db.Products.Add(new Product {Id = new Guid("894194aa-611c-4a35-7eac-07d9d75bf80f"), Name = "Keyboard",Description = "Machine that makes clicky sounds", Price = 59.99d, Type = "Machine", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false});
        db.Products.Add(new Product {Id = new Guid("894194aa-611c-4a35-7eac-06d9d75bf80f"), Name = "Cup",Description = "Rounded container", Price = 9.59d, Type = "Container", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false});
        db.Products.Add(new Product {Id = new Guid("894194aa-611c-4a35-7eac-05d9d75bf80f"), Name = "Bowl",Description = "Rounded container that is larger than a cup", Price = 11.99d, Type = "Container", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, IsDeleted = false});
        db.SaveChangesAsync();
        return db;
    }
}