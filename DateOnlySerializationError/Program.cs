using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<Order>("Orders");

builder.Services
    .AddControllers()
    .AddOData(options => options.EnableQueryFeatures().AddRouteComponents(modelBuilder.GetEdmModel()));

var app = builder.Build();

app.MapControllers();

app.Run();

public class Order
{
    public int Id { get; set; }
    public DateOnly Created { get; set; }
}

public class OrdersController : ODataController
{
    public IEnumerable<Order> Get()
    {
        return [new Order { Id = 1, Created = DateOnly.FromDateTime(DateTime.Now) }];
    }

    public ActionResult<Order> Post([FromBody] Order newOrder)
    {
        if (ModelState.IsValid is false)
        {
            return BadRequest(ModelState);
        }
        return newOrder;
    }
}