using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace RealTimeTask.Data
{
    public class TaskRepository
    {
        private readonly string _connectionString;
        public TaskRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddTask(TaskItem task)
        {
            using var ctx = new TaskItemsContext(_connectionString);
            ctx.TaskItems.Add(task);
            ctx.SaveChanges();
        }
        public List<TaskItem> GetTaskItems()
        {
            using var ctx = new TaskItemsContext(_connectionString);
            return ctx.TaskItems.Include(a => a.User).ToList();
        }
        public void SetDoingIt(int taskId, int userId)
        {
            using var ctx = new TaskItemsContext(_connectionString);
            ctx.Database.ExecuteSqlInterpolated($"update taskitems set handledby={userId} where id ={taskId}");
        }
        public void SetCompleted(int taskId)
        {
            using var ctx = new TaskItemsContext(_connectionString);
            ctx.Database.ExecuteSqlInterpolated($"delete from taskitems where id ={taskId}");
        }
        public TaskItem GetById(int id)
        {
            using var ctx = new TaskItemsContext(_connectionString);
            return ctx.TaskItems.Include(u => u.User).FirstOrDefault(u => u.Id == id);
        }

    }
}
