using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Leopard.Domain.Extensions;
using Leopard.Domain.Models;

namespace Leopard.Algorithms.Dpso.Extensions
{
        public static class ParticleExtensions
        {
        public static CompositionPlan GetGlobalBest(this List<CompositionPlan> particles)
        {
            return particles.OrderBy(p => p.Cost).First();
        }

        public static CompositionPlan Move(this CompositionPlan compositionPlan,
            CompositionPlan gBest, DpsoConfig DpsoConfig, List<TaskCandidateService> taskCandidateServices)
        {
            double mut = (double)1 / DpsoConfig.Tasks.Count;

            Random r = new Random();

            foreach (TaskService taskService in compositionPlan.TaskServices)
            {
                double rand1 = (double)r.Next(0, 100) / 100;

                if (rand1 < mut)
                {
                    List<WebService> webServices = taskCandidateServices
                        .First(t => t.Task.Equals(taskService.Task)).WebServices;

                    int randomIndex = r.Next(0, webServices.Count - 1);

                    //if (taskService.WebService != webServices[randomIndex])
                    //{
                    taskService.WebService = webServices[randomIndex];
                    //}
                }
                else
                if (rand1 < DpsoConfig.Omega)
                {
                    break;
                }
                else if (rand1 < DpsoConfig.C1 + DpsoConfig.Omega)
                {
                    taskService.WebService = compositionPlan.PBest.TaskServices
                        .First(t => t.Task.Equals(taskService.Task)).WebService;

                }
                else
                {
                    taskService.WebService = gBest.TaskServices
                        .First(t => t.Task.Equals(taskService.Task)).WebService;
                }
            }

            compositionPlan.UpdatePBest(DpsoConfig);

            return compositionPlan;
        }

        private static CompositionPlan UpdatePBest(this CompositionPlan compositionPlan,
            DpsoConfig DpsoConfig)
        {
            compositionPlan.Cost = compositionPlan.CalculateCost(DpsoConfig.QualityAttributeWeights);

            if (compositionPlan.Cost < compositionPlan.PBest.Cost)
                compositionPlan.PBest = compositionPlan;

            return compositionPlan;
        }

    }
}
