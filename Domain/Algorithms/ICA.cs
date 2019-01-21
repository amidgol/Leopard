using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.ICA;
using Domain.Models;

namespace Domain.Algorithms
{
    public class Ica : IAlgorithm<CompositionPlan, CompositionRequest>
    {
        public CompositionRequest Execute(CompositionPlan input)
        {
            throw new NotImplementedException();
        }

        private IEnumerable<CompositionPlan> CreateInitialCountries(CompositionRequest request)
        {
            List<CompositionPlan> countries = new List<CompositionPlan>();
            double id = 1;

            foreach (var requestCandidateWebService in request.CandidateWebServices)
            {
                CompositionPlan country = new CompositionPlan();

                foreach (SingleTask item in request.SingleTasks)
                {
                    Random random = new Random();

                    double randomNumber = random.Next(1, request.CandidateWebServices.Count());

                    SingleTaskService singleTaskService = new SingleTaskService
                    {
                        SingleTask = item,
                        WebService = request.CandidateWebServices.First(x => x.Id == randomNumber)
                    };
                    
                    country.TaskServices.Add(singleTaskService);

                    id++;
                }

                countries.Add(country);
            }

            return countries;
        }

        private IEnumerable<Empire<CompositionPlan>> CreateInitialEmpires(List<CompositionPlan> compositionPlans)
        {
            throw new NotImplementedException();
        }
    }
}
