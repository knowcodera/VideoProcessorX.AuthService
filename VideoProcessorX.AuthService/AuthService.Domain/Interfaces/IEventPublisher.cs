namespace AuthService.Domain.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync(string message);
    }
}
