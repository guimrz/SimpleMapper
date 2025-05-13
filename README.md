# SimpleMapper
> 🛍️ A lightweight and fast object-to-object mapper for .NET, designed with simplicity and performance in mind.

![Build](https://github.com/guimrz/SimpleMapper/workflows/Build/badge.svg)
[![NuGet](http://img.shields.io/nuget/vpre/Guimrz.SimpleMapper.svg?label=NuGet)](https://www.nuget.org/packages/Guimrz.SimpleMapper/)


## ✨ Overview

**Guimrz.SimpleMapper** is a strongly-typed, dependency-injection-friendly mapping library. It emphasizes low overhead and minimal configuration, ideal for clean architecture or microservice-based systems.

This library uses simple, explicit `ITypeMapper<TSource, TDestination>` implementations and leverages a runtime cache to ensure high performance — without relying on reflection-heavy solutions like AutoMapper.


## 🚀 Features

- ✨ Explicit mapping via `ITypeMapper<TSource, TDestination>`
- 🔍 Automatic registration through assembly scanning
- 📦 DI integration using a runtime `IMapper` interface
- ⚡️ Optimized performance with reflection caching


## 📦 Installation

Install via [NuGet](https://www.nuget.org):

```bash
dotnet add package Guimrz.SimpleMapper
```

## 🚀 Getting Started

### 1. Define your type mapper:

```csharp
public class UserToUserDtoMapper : ITypeMapper<User, UserDto>
{
    public UserDto Map(User source) => new()
    {
        Id = source.Id,
        Name = source.Name
    };
}
```

### 2. Register the mapper in `Startup.cs` or Program.cs:

```csharp
services.AddSimpleMapper()
        .RegisterTypeMapper<UserToUserDtoMapper>();
```

Or scan the whole assembly:

```csharp
services.AddSimpleMapper()
        .RegisterTypeMappersFromAssembly(typeof(UserToUserDtoMapper).Assembly);
```

### 3. Use the `IMapper` service:

```csharp
public class UserService
{
    private readonly IMapper _mapper;

    public UserService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public UserDto GetDto(User user)
    {
        return _mapper.Map<UserDto>(user);
    }
}
```


## 📄 License
This project is licensed under the MIT License.

## 👤 Author
Developed with care by [José Guimarães](https://github.com/guimrz).

## 🤝 Contributing
Feel free to submit issues, suggest improvements, or open pull requests.
Let’s make mapping more predictable and efficient together.
