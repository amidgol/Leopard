using System.Collections.Generic;

namespace Domain.Models
{
    public class TaskCandidateServices
    {
        public SingleTask Task { get; set; }
        public IEnumerable<WebService> WebServices { get; set; }

    }
}