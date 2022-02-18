namespace IceSync.Models
{
    public class Workflow  
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsRunning { get; set; }
        public MultiExecBehaviors MultiExecBehavior { get; set; }
    }
}
