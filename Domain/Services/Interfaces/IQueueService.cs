namespace Domain.Services.Interfaces
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IQueueService
    {
        Task Push<T>(CancellationToken stoppingToken, T message);
        Task Subscribe<T>(CancellationToken stoppingToken, Func<T, CancellationToken, Task> handler) where T : class;
    }
}