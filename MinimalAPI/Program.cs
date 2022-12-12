using Microsoft.EntityFrameworkCore;
using MinimalAPI.Data;
using MinimalAPI.Models;
using MiniValidation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinimalContentxDb>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/fornecedores", async (MinimalContentxDb context) =>
    await context.Forncedores.ToListAsync()
)
.WithName("GetFornecedores")
.WithTags("Fornecedor");


app.MapGet("/fornecedor/{id}", async (Guid id, MinimalContentxDb context) =>
    await context.Forncedores.FindAsync(id)
        is Fornecedor fornecedor ? Results.Ok(fornecedor) : Results.NotFound()
)
.Produces<Fornecedor>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.WithName("GetFornecedoresPorId")
.WithTags("Fornecedor");


app.MapPost("/fornecedor/", async (MinimalContentxDb context, Fornecedor fornecedor) => 
{
    if (!MiniValidator.TryValidate(fornecedor, out var errors))
        return Results.ValidationProblem(errors);

    context.Forncedores.Add(fornecedor);
    var result = await context.SaveChangesAsync();

    return result > 0
        //? Results.Created($"/fornecedor/{fornecedor.Id}", fornecedor })
        ? Results.CreatedAtRoute("GetFornecedorPorId", new { id = fornecedor.Id, fornecedor })
        : Results.BadRequest("Houve um problemaao salvar o registro");

})
.Produces<Fornecedor>(StatusCodes.Status201Created)
.Produces(StatusCodes.Status400BadRequest)
.WithName("PostFornecedores")
.WithTags("Fornecedor");
   

app.Run();