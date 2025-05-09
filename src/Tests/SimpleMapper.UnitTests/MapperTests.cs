using Moq;
using SimpleMapper.Abstractions;
using SimpleMapper.Exceptions;

namespace SimpleMapper.UnitTests
{
    public class MapperTests
    {
        public class Source { public string Name = "original"; }
        public class Destination { public string Name = string.Empty; }

        [Fact]
        public void Map_ValidSource_ReturnsMappedDestination()
        {
            // Arrange
            var source = new Source();
            var expected = new Destination { Name = "mapped" };

            var mapperMock = new Mock<ITypeMapper<Source, Destination>>();
            mapperMock.Setup(m => m.Map(source)).Returns(expected);

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ITypeMapper<Source, Destination>)))
                .Returns(mapperMock.Object);

            var mapper = new Mapper(serviceProviderMock.Object);

            // Act
            var result = mapper.Map<Destination>(source);

            // Assert
            Assert.Same(expected, result);
        }

        [Fact]
        public void Map_NullSourceNullableDestination_ReturnsNull()
        {
            // Arrange
            var serviceProvider = new Mock<IServiceProvider>().Object;
            var mapper = new Mapper(serviceProvider);

            // Act
            var result = mapper.Map<string?>(null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Map_NullSourceNonNullableDestination_ThrowsMappingException()
        {
            // Arrange
            var serviceProvider = new Mock<IServiceProvider>().Object;
            var mapper = new Mapper(serviceProvider);

            // Act & Assert
            Assert.Throws<MappingException>(() => mapper.Map<int>(null));
        }

        [Fact]
        public void Map_MapperNotRegistered_ThrowsMappingException()
        {
            // Arrange
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(ITypeMapper<Source, Destination>)))
                .Returns((object?)null);

            var mapper = new Mapper(serviceProviderMock.Object);

            var source = new Source();

            // Act & Assert
            Assert.Throws<MappingException>(() => mapper.Map<Destination>(source));
        }
    }
}
