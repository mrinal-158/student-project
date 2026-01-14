using Microsoft.EntityFrameworkCore;
using student_project.Data;
using student_project.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure PostgreSQL connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<StudentDb>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//CRUD Endpoints for Student entity
app.MapGet("/students", async (StudentDb db) =>
{
    var students = await db.Students.ToListAsync();
    return Results.Ok(students);
});

app.MapPost("/students", async (Student student, StudentDb db) =>
{
    db.Students.Add(student);
    await db.SaveChangesAsync();
    return Results.Created($"/students/{student.Id}", student);
});
    
app.MapDelete("/students/{id}", async (int id, StudentDb db) =>
{
    if(await db.Students.FindAsync(id) is Student student)
    {
        db.Students.Remove(student);
        await db.SaveChangesAsync();
        return Results.Ok(new
        {
            student = student,
            message = $"Student with ID {id} has been removed successfully!"
        });
    }
    return Results.NotFound("This record is not found!");
});


app.Run();

