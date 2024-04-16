using APBD_API_v1.Classes;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

List<Animal> animals = new List<Animal>();
List<Visit> visits = new List<Visit>();

int GetNextAnimalId() => animals.Any() ? animals.Max(a => a.Id) + 1 : 1;
int GetNextVisitId() => visits.Any() ? visits.Max(v => v.Id) + 1 : 1;

app.MapGet("/animals", () => animals)
    .WithName("GetAllAnimals");

app.MapGet("/animals/{id}", (int id) => animals.FirstOrDefault(a => a.Id == id))
    .WithName("GetAnimalById");

app.MapPost("/animals", (Animal animal) => {
        animal.Id = GetNextAnimalId();
        animals.Add(animal);
        return Results.Created($"/animals/{animal.Id}", animal);
    })
    .WithName("AddAnimal");

app.MapPut("/animals/{id}", (int id, Animal updatedAnimal) => {
        var animal = animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return Results.NotFound();
        animal.Name = updatedAnimal.Name;
        animal.Category = updatedAnimal.Category;
        animal.Weight = updatedAnimal.Weight;
        animal.FurColor = updatedAnimal.FurColor;
        return Results.NoContent();
    })
    .WithName("UpdateAnimal");

app.MapDelete("/animals/{id}", (int id) => {
        var animal = animals.FirstOrDefault(a => a.Id == id);
        if (animal == null) return Results.NotFound();
        animals.Remove(animal);
        return Results.NoContent();
    })
    .WithName("DeleteAnimal");

// Visits
app.MapGet("/animals/{animalId}/visits", (int animalId) => visits.Where(v => v.AnimalId == animalId).ToList())
    .WithName("GetVisitsForAnimal");

app.MapPost("/visits", (Visit visit) => {
        visit.Id = GetNextVisitId();
        visits.Add(visit);
        return Results.Created($"/visits/{visit.Id}", visit);
    })
    .WithName("AddVisit");

app.Run();