using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Empire<CompositionPlan> UpdateAfterAssimilation(this Empire<CompositionPlan> empire)
        {
            CompositionPlan bestColony = empire.Colonies.OrderBy(x => x.Cost).First();

            if (bestColony.Cost < empire.Imperialist.Cost)
            {
                CompositionPlan formerImperialist = empire.Imperialist;
                empire.Imperialist = bestColony;
                empire.Colonies[0] = formerImperialist;
            }

            return empire;
        }

        public static Empire<CompositionPlan> CalculateCost(this Empire<CompositionPlan> empire, double zeta)
        {
            double totalCost = empire.Imperialist.Cost + zeta * (empire.Colonies.Select(x => x.Cost).Average());

            empire.TotalCost = totalCost;

            return empire;
        }
    }
}
