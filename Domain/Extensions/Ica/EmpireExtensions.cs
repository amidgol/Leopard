using System;
using System.Collections.Generic;
using System.Text;
using Domain.ICA;
using Domain.Models;

namespace Domain.Extensions.Ica
{
    internal static class EmpireExtensions
    {
        public static Empire<CompositionPlan> Assimilate(this Empire<CompositionPlan> empire, 
            List<QualityAttributeWeight> attributeWeights)
        {
            foreach (CompositionPlan colony in empire.Colonies)
            {
                List<double> mask = colony.GetMask();

                for (var i = 0; i < colony.TaskServices.Count; i++)
                {
                    TaskService taskService = colony.TaskServices[i];

                    if (mask[i] > taskService.WebService.Cost)
                    {
                        colony.TaskServices[i].WebService = empire.Imperialist.TaskServices[i]
                            .WebService;
                        
                        //update colony's cost
                        colony.Cost = colony.CalculateCost(attributeWeights);
                    }
                }
            }

            return empire;
        } 
    }
}
