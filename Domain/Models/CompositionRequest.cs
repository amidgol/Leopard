using System.Collections.Generic;

namespace WebServiceComposition.Domain.Models
{
    public class CompositionRequest
    {
        public IEnumerable<TaskCandidateService> TaskCandidateServices { get; set; }
    }
}