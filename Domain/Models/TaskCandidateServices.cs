using System.Collections.Generic;

namespace Domain.Models
{
    public class TaskCandidateServices
    {
        public SingleTask Task { get; set; }
        public List<WebService> WebServices { get; set; }

    }
}