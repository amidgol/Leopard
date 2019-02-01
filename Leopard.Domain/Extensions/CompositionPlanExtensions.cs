using System;
using System.Collections.Generic;
using Leopard.Domain.Models;

namespace Leopard.Domain.Extensions
{
    public static class CompositionPlanExtensions
    {
        public static double CalculateCost(this CompositionPlan compositionPlan,
            List<QualityAttributeWeight> qualityAttributeWeights)
        {
            double cost = 0;

            foreach (TaskService taskService in compositionPlan.TaskServices)
            {
                taskService.WebService.Cost = taskService.WebService
                    .CalculateCost(qualityAttributeWeights);

                cost += taskService.WebService.Cost;

            }

            return cost / compositionPlan.TaskServices.Count;
        }

        public static List<double> GetMask(this CompositionPlan compositionPlan,
            int min = 0, int max = 100)
        {
            Random random = new Random();

            List<double> maskList = new List<double>();

            for (int index = 0; index < compositionPlan.TaskServices.Count; index++)
            {
                maskList.Add((double) random.Next(min, max) / 100);
            }

            return maskList;
        }
    }
}
