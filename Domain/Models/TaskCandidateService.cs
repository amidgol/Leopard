using System.Collections.Generic;

namespace Domain.Models
{
    public class TaskCandidateService
    {
        public SingleTask Task { get; set; }
        public List<WebService> WebServices { get; set; }

        public override string ToString()
        {
            return $"{Task.Title}, Candidates: {WebServices.Count}";
        }
    }
}