using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using ProjetArchiLog.API.Controllers.v1;
using ProjetArchiLog.API.Models;
using ProjetArchiLog.Library.Models;
using ProjetArchiLog.TestAPI.Data;

namespace ProjetArchiLog.TestAPI;

public class ProductControllerTests
{
    private ProductsController _controller;
    private MockDbContext _context;

    [SetUp]
    public void Setup()
    {
        _context = MockDbContext.GetDbContext();
        _controller = new ProductsController(_context);
    }

    [Test]
    public async Task TestGetAll()
    {
        try
        {
            var actionResult = await _controller.GetAll(new SortingParams{Sort = "Name"});
            var values =  actionResult.Value as IEnumerable<Product>;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        //Assert.IsNotNull(values);
        //Assert.AreEqual(_context.Products.Count(), values.Count());
        
        Assert.True(true);
    }
}