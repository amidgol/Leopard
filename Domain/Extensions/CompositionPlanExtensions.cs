using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Domain.Extensions
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

        public static List<double> GetMask(this CompositionPlan compositionPlan)
        {
            Random random = new Random();

            List<double> maskList = new List<double>();

            for (var index = 0; index < compositionPlan.TaskServices.Count; index++)
            {
                maskList.Add((double) random.Next(0, 100) / 100);
            }

            return maskList;
        }
    }
}
