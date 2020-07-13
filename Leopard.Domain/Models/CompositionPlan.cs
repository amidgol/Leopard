using System.Collections.Generic;

namespace Leopard.Domain.Models {
    public class CompositionPlan : BaseEntity {
        public List<TaskService> TaskServices { get; set; }
        public double Cost { get; set; }
        public double Power { get; set; }
        public double NormalizedPower { get; set; }
        public CompositionPlan PBest { get; set; } //TODO: this is redundant for ICA
        
        public override string ToString () {
            return $"Cost: {Cost}";
        }
    }
}