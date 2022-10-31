using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using RealTimeTask.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealTimeTasks.Web
{
    public class TasksHub : Hub
    {
        private readonly string _connectionString;

        public TasksHub(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        public void NewTask(string title)
        {
            var taskRepo = new TaskRepository(_connectionString);
            var task = new TaskItem { Title = title};
            taskRepo.AddTask(task);
            SendTasks();
        }

        private void SendTasks()
        {
            var taskRepo = new TaskRepository(_connectionString);
            var tasks = taskRepo.GetTaskItems();

            Clients.All.SendAsync("WriteTasks", tasks.Select(t => new
            {
                Id = t.Id,
                Title = t.Title,
                HandledBy = t.HandledBy,
                UserDoingIt = t.User != null ? $"{t.User.FirstName} {t.User.LastName}" : null,
            }));
        }

        public void GetAll()
        {
            SendTasks();
        }

        public void SetDoing(int taskId)
        {
            var userRepo = new UserRepository(_connectionString);
            var user = userRepo.GetByEmail(Context.User.Identity.Name);
            var taskRepo = new TaskRepository(_connectionString);
            taskRepo.SetDoingIt(taskId, user.Id);
            SendTasks();
        }

        public void SetDone(int taskId)
        {
            var taskRepo = new TaskRepository(_connectionString);
            taskRepo.SetCompleted(taskId);
            SendTasks();
        }

    }
}
