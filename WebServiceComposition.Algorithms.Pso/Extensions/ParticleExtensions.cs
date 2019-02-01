using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServiceComposition.Domain.Extensions;
using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Algorithms.Pso.Extensions
{
    public static class ParticleExtensions
    {
        public static CompositionPlan GetGlobalBest(this List<CompositionPlan> particles)
        {
            return particles.OrderBy(p => p.Cost).First();
        }

        public static CompositionPlan Move(this CompositionPlan compositionPlan,
            CompositionPlan gBest, PsoConfig psoConfig)
        {
            int columnsToBeMaskedLocally = (int)(compositionPlan.TaskServices.Count * psoConfig.C1);
            int columnsToBeMaskedGlobally = compositionPlan.TaskServices.Count - columnsToBeMaskedLocally;

            Random r = new Random();
            int mod = DateTime.Now.Millisecond % 2;
            bool directionLtr = mod == 1;

            compositionPlan.MoveTowardParticle(compositionPlan.PBest, columnsToBeMaskedLocally, directionLtr);

            compositionPlan.MoveTowardParticle(gBest, columnsToBeMaskedGlobally, !directionLtr);

            compositionPlan.UpdatePBest(psoConfig);

            return compositionPlan;
        }

        private static CompositionPlan UpdatePBest(this CompositionPlan compositionPlan,
            PsoConfig psoConfig)
        {
            compositionPlan.Cost = compositionPlan.CalculateCost(psoConfig.QualityAttributeWeights);

            if (compositionPlan.Cost < compositionPlan.PBest.Cost)
                compositionPlan.PBest = compositionPlan;

            return compositionPlan;
        }

        private static CompositionPlan MoveTowardParticle(this CompositionPlan compositionPlan,
            CompositionPlan target,
            int columnsToBeMasked, bool maskLeftToRight)
        {
            var mask = compositionPlan.GetMask();

            if (maskLeftToRight)
            {
                for (int i = 0; i < columnsToBeMasked; i++)
                {
                    if (mask[i] > compositionPlan.TaskServices[i].WebService.Cost)
                    {
                        compositionPlan.TaskServices[i].WebService = target.TaskServices[i].WebService;
                    }
                }
            }
            else
            {
                for (int i = compositionPlan.TaskServices.Count - 1;
                    i > compositionPlan.TaskServices.Count - columnsToBeMasked; i--)
                {
                    if (mask[i] > compositionPlan.TaskServices[i].WebService.Cost)
                    {
                        compositionPlan.TaskServices[i].WebService = target.TaskServices[i].WebService;
                    }
                }
            }

            return compositionPlan;
        }
    }
}
