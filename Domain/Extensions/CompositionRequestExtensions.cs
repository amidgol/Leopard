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

            foreach (TaskCandidateService taskCandidateServices in request.TaskCandidateServices)
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


            for (int i = 0; i < listOfLists[0].Count; i++)
            {
                List<TaskService> taskServices = new List<TaskService>();

                foreach (List<TaskService> list in listOfLists)
                {
                    TaskService taskService = list[i];
                    taskServices.Add(taskService);
                }

                CompositionPlan country = new CompositionPlan
                {
                    Id = i,
                    TaskServices = taskServices
                };

                countries.Add(country);
            }

            return countries;
        }
    }
}
