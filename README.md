
# SimpleMapper - Simple and lightweight object mapping for .NET

**SimpleMapper** is a lightweight, explicit, and high-performance object mapping library for .NET. It offers a clean, predictable mappeing library, giving you full control over how your data is transformed between types.

## 🚀 Features

- ✨ Explicit mapping via `ITypeMapper<TSource, TDestination>`
- 🔍 Automatic registration through assembly scanning
- 📦 DI integration using a runtime `IMapper` interface
- ⚡️ Optimized performance with reflection caching

## 📦 Installation

```bash
dotnet add package SimpleMapper
```

## 🛠️ Usage

#### 1. Implement a type mapper

```csharp
public class UserMapper : ITypeMapper<User, UserDto>
{
    public UserDto Map(User source)
    {
        return new UserDto
        {
            Id = source.Id,
            Name = source.Name,
            Email = source.Email
        };
    }
}
```

#### 2. Register the mapper in the DI container

```csharp
services.AddSimpleMapper()
        .RegisterTypeMapper<UserMapper>();
```

Or register all mappers from an assembly:

```csharp
services.RegisterTypeMappersFromAssembly(typeof(UserMapper).Assembly);
```

#### 3. Runtime Mapping

Once registered, you can use `IMapper` to map dynamically at runtime:

```csharp
var dto = _mapper.Map<UserDto>(sourceUser);
```

## 🤝 Contributing

Pull requests are welcome! You can help by:

- Reporting issues
- Proposing improvements
- Adding tests or documentation