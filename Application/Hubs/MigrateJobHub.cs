using Application.Background;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Hubs
{
    public class MigrateJobHub : Hub
    {
        private readonly IBackgroundTaskQueue _taskQueue;

        public MigrateJobHub(IBackgroundTaskQueue taskQueue)
        {
            _taskQueue = taskQueue;
        }
        public async Task AssociateJob(string jobId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, jobId);
        }

        public async Task IsWorking()
        {
            var isWorking = this._taskQueue.GetIsWorking();
            await Clients.All.SendAsync("working", isWorking);
        }
    }
}
