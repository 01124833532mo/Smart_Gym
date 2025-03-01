using Microsoft.AspNetCore.Mvc;
using SmartGym.Apis.Controller;
using SmartGym.Apis.MiddleWares;
using SmartGym.Shared.Errors.Response;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().ConfigureApiBehaviorOptions((option) =>
{
    option.SuppressModelStateInvalidFilter = false;
    option.InvalidModelStateResponseFactory = (action) =>
    {
        var errors = action.ModelState.
        Where(p => p.Value!.Errors.Count > 0)
        .SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage);

        return new BadRequestObjectResult(new ApiValidationErrorResponse() { Erroes = errors });
    };
}).AddApplicationPart(typeof(AssemblyInformation).Assembly);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<CustomHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePagesWithReExecute("/Errors/{0}");
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
