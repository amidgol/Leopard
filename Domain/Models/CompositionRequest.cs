using System.Collections.Generic;

namespace Domain.Models
{
    public class CompositionRequest
    {
        public IEnumerable<SingleTask> SingleTasks { get; set; }
        public IEnumerable<WebService> CandidateWebServices { get; set; }
    }
}