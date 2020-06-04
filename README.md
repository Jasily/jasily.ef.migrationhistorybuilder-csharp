# Jasily.EF.MigrationHistoryBuilder

Allow you change `DbContext.Database.EnsureCreated()` to `DbContext.Database.Migrate()` after database created without drop data.

## Usage

### Programing

After you use `dotnet ef migrations add InitialCreate` create your migration class, than replace your csharp code `DbContext.Database.EnsureCreated()` to:

``` cs
if (DbContext.Database.IsCalledEnsureCreatedOnly())
{
    DbContext.Database.GetHistoryBuilder()
        .AddAsTrustedApplied<InitialCreate>(); // InitialCreate is the class you just created
}

DbContext.Database.Migrate();
```

Done.
