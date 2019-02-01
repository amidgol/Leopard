using System;
using Leopard.Domain;
using Leopard.Domain.Models;

namespace Leopard.RequestGeneration.RequestGenerators
{
    public class RandomRequestGenerator : IRequestGenerator
    {
        public CompositionRequest Generate(Config config)
        {
            //Random random = new Random();

            //List<TaskCandidateService> taskCandidateServices = new List<TaskCandidateService>();

            //foreach (SingleTask task in config.Tasks)
            //{
            //    TaskCandidateService taskCandidateService = new TaskCandidateService
            //    {
            //        Task = task,
            //        WebServices = new List<WebService>()
            //    };

            //    for (int i = 0; i < config.CandidatesPerTask; i++)
            //    {

            //        List<QualityAttributeValue> qualityAttributeValues
            //            = new List<QualityAttributeValue>();
            //        foreach (QualityAttribute attribute in qualityAttributes)
            //        {
            //            qualityAttributeValues.Add(new QualityAttributeValue
            //            {
            //                QualityAttribute = attribute,
            //                Value = (double)random.Next(5000, 10000) / 10000 //todo
            //        });
            //        }

            //        taskCandidateService.WebServices.Add(new WebService
            //        {
            //            Title = $"{task.Title}-{i}",
            //            QualityAttributeValues = qualityAttributeValues
            //        });
            //    }
            //}

            //CompositionRequest request = new CompositionRequest
            //{
            //    TaskCandidateServices = taskCandidateServices
            //};

            //return request;

            throw new NotImplementedException();
        }
        
    }
}
