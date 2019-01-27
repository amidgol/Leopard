using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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

            List<List<TaskService>> listOfLists = new List<List<TaskService>>();

            foreach (TaskCandidateServices taskCandidateServices in request.TaskCandidateServices)
            {
                SingleTask task = taskCandidateServices.Task;
                List<TaskService> taskServices = new List<TaskService>();


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

                listOfLists.Add(taskServices);
            }

            for (int i = 0; i < Math.Min(listOfLists[0].Count, listOfLists[1].Count); i++)
            {
                CompositionPlan country = new CompositionPlan
                {
                    Id = i,
                    TaskServices = new List<TaskService>
                    {
                        listOfLists[0][i],
                        listOfLists[1][i]
                    }
                };

                countries.Add(country);
                
            }

            return countries;
        }
    }
}
