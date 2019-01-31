using System.Collections.Generic;

namespace Domain.Models
{
    public class CompositionRequest
    {
        public IEnumerable<TaskCandidateService> TaskCandidateServices { get; set; }
    }
}