using System.Collections.Generic;

namespace Domain.Models
{
    public class CompositionRequest
    {
        public IEnumerable<TaskCandidateServices> TaskCandidateServices { get; set; }
    }
}