using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Models;

namespace Domain.Extensions
{
    public static class CompositionRequestExtensions
    {
        public static IEnumerable<CompositionPlan> CreateInitialCountries(this CompositionRequest request)
        {
            List<CompositionPlan> countries = new List<CompositionPlan>();

            Random random = new Random();

            foreach (TaskCandidateServices taskCandidateServices in request.TaskCandidateServices)
            {
                SingleTask task = taskCandidateServices.Task;
                List<TaskService> taskServices = new List<TaskService>();
                CompositionPlan compositionPlan = new CompositionPlan();

                foreach (WebService webService in taskCandidateServices.WebServices)
                {
                    int randomNumber = random.Next(0, taskCandidateServices.WebServices.Count() - 1);

                    TaskService taskService = new TaskService
                    {
                        Task = task,
                        WebService = taskCandidateServices.WebServices.ToList()[randomNumber]
                    };

                    taskServices.Add(taskService);
                }

                compositionPlan.TaskServices = taskServices;

                countries.Add(compositionPlan);
            }

            return countries;
        }
    }
}
