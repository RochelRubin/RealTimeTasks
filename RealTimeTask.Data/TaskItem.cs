namespace RealTimeTask.Data
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int? HandledBy { get; set; }
       

        public User User { get; set; }
    }
}
