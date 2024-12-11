using Logistics.Data;
using Logistics.Data.Requests.Models;
using Microsoft.EntityFrameworkCore;

namespace Logistics.Services
{
    public class RequestArchiverService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private Timer _timer;

        public RequestArchiverService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(archive, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private async void archive(object state)
        {  
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
                _context.Requests.Where(x => (x.status == RequestStatus.Active || x.status == RequestStatus.Delayed) && x.sendingTime < DateTime.UtcNow).ExecuteUpdate(upd => upd.SetProperty(a => a.status, RequestStatus.ArchivedNotAccepted));
                _context.SaveChanges();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
