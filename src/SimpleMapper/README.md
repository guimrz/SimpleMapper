# SimpleMapper

`SimpleMapper` is a lightweight and efficient object mapping library for .NET, designed to be simple, fast, and easy to integrate.

This project contains the implementation of the mapper system and internal caching for performance optimization.

## Features

- Dependency Injection support via `IServiceCollection`
- Fast caching via `ConcurrentDictionary`
- Clean separation of concerns using interfaces
- Zero runtime reflection after warm-up

## Key Components

### `Mapper`

Resolves type mappers and handles the conversion logic by leveraging the `ICachedMapperContainer`.

### `CachedMapperContainer`

Internal class responsible for resolving and caching method info to avoid runtime cost.

### `MappingException`

Custom exception used to represent mapping-specific failures.

### `ServiceCollectionExtensions`

Provides extension methods to register `SimpleMapper` in a DI container:

```csharp
services.AddSimpleMapper();
services.RegisterTypeMapper<MyCustomMapper>();
services.RegisterTypeMappersFromAssembly(Assembly.GetExecutingAssembly());
```

## Example

```csharp
public class PersonToDtoMapper : ITypeMapper<Person, PersonDto>
{
    public PersonDto Map(Person source) => new PersonDto { Name = source.Name };
}

// Registration
services.AddSimpleMapper()
        .RegisterTypeMapper<PersonToDtoMapper>();

// Usage
var mapper = serviceProvider.GetRequiredService<IMapper>();
var dto = mapper.Map<PersonDto>(new Person { Name = "José" });
```
