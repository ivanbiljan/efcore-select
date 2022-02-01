// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");

var options = new DbContextOptionsBuilder<Context>()
    .UseInMemoryDatabase(databaseName: "InMemoryTest")
    .Options;
var context = new Context(options);

var queryable = context.Entities.Select(e => new
{
    e.Id,
    e.Name
});

var queryableNoAnonSelect = context.Entities.AsQueryable();
var dbSet = context.Entities;

Console.WriteLine($"{nameof(queryable)} is IOrderedQueryable: {Check(queryable)}");
Console.WriteLine($"{nameof(queryableNoAnonSelect)} is IOrderedQueryable: {Check(queryableNoAnonSelect)}");
Console.WriteLine($"{nameof(dbSet)} is IOrderedQueryable: {Check(dbSet)}");

static bool Check<T>(IQueryable<T> source) => source is IOrderedQueryable<T>;

public class Entity
{
    public Entity(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;
}

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<Entity> Entities => Set<Entity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var entities = new Entity[]
        {
            new(1, "Entity1"),
            new(2, "Entity2"),
            new(3, "Entity3"),
            new(4, "Entity4")
        };

        modelBuilder.Entity<Entity>().HasData(entities);
    }
}