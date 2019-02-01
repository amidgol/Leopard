namespace Leopard.Domain.Models
{
    public  class TaskService
    {
        public SingleTask  Task { get; set; }
        public WebService WebService { get; set; }

        public override string ToString()
        {
            return $"{Task.Title}, WebService: {WebService.Title}";
        }
    }
}
