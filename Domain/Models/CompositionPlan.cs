using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class CompositionPlan:BaseEntity
    {
        public List<SingleTaskService> TaskServices { get; set; }
    }
}
