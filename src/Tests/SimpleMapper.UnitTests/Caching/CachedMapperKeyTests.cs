using SimpleMapper.Caching;

namespace SimpleMapper.UnitTests.Caching
{
    public class CachedMapperKeyTests
    {
        [Fact]
        public void Constructor_SetsPropertiesCorrectly()
        {
            // Arrange
            var sourceType = typeof(string);
            var destinationType = typeof(int);

            // Act
            var key = new CachedMapperKey(sourceType, destinationType);

            // Assert
            Assert.Equal(sourceType, key.SourceType);
            Assert.Equal(destinationType, key.DestinationType);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenSourceTypeIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new CachedMapperKey(null!, typeof(int)));
            Assert.Equal("sourceType", ex.ParamName);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenDestinationTypeIsNull()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new CachedMapperKey(typeof(string), null!));
            Assert.Equal("destinationType", ex.ParamName);
        }

        [Fact]
        public void Equals_ReturnsTrue_WhenKeysAreEqual()
        {
            // Arrange
            var key1 = new CachedMapperKey(typeof(string), typeof(int));
            var key2 = new CachedMapperKey(typeof(string), typeof(int));

            // Act & Assert
            Assert.True(key1.Equals(key2));
            Assert.True(key1.Equals((object)key2));
        }

        [Fact]
        public void Equals_ReturnsFalse_WhenKeysDiffer()
        {
            // Arrange
            var key1 = new CachedMapperKey(typeof(string), typeof(int));
            var key2 = new CachedMapperKey(typeof(int), typeof(string));

            // Act & Assert
            Assert.False(key1.Equals(key2));
            Assert.False(key1.Equals((object)key2));
        }

        [Fact]
        public void GetHashCode_IsEqual_ForEqualKeys()
        {
            // Arrange
            var key1 = new CachedMapperKey(typeof(string), typeof(int));
            var key2 = new CachedMapperKey(typeof(string), typeof(int));

            // Act & Assert
            Assert.Equal(key1.GetHashCode(), key2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_Differs_ForDifferentKeys()
        {
            // Arrange
            var key1 = new CachedMapperKey(typeof(string), typeof(int));
            var key2 = new CachedMapperKey(typeof(int), typeof(string));

            // Act & Assert
            Assert.NotEqual(key1.GetHashCode(), key2.GetHashCode());
        }
    }
}
