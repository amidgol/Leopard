using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class CompositionPlan:BaseEntity
    {
        public List<TaskService> TaskServices { get; set; }
        public double Cost { get; set; }
        public double  Power { get; set; }
        public double  NormalizedPower { get; set; }

        public override string ToString()
        {
            string result = "";

            foreach (TaskService taskService in TaskServices)
            {
                result += $"{taskService.WebService.Title} - ";
            }

            return $"{result} Cost: {Cost}";
        }
    }
}
