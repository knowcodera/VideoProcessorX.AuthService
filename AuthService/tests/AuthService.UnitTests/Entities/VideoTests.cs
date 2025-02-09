using AuthService.Domain.Entities;

namespace AuthService.UnitTests.Entities
{
    public class VideoTests
    {
        [Fact]
        public void Video_ShouldInitializeWithDefaultValues()
        {
            // Act
            var video = new Video();

            // Assert
            Assert.NotNull(video);
            Assert.Equal(DateTime.UtcNow.Date, video.CreatedAt.Date);
            Assert.Null(video.ProcessedAt);
        }
    }
}
