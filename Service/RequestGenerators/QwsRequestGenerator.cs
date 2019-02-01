using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebServiceComposition.Domain;
using WebServiceComposition.Domain.Extensions;
using WebServiceComposition.Domain.Models;

namespace WebServiceCompositionService.RequestGenerators
{
    public class QwsRequestGenerator : IRequestGenerator
    {
        public CompositionRequest Generate(Config config)
        {

            int counter = 0;
            string line;

            // Read the file and display it line by line.  
            StreamReader file =
                new StreamReader(config.DataSetFilePath);

            List<WebService> webServices = new List<WebService>();

            while ((line = file.ReadLine()) != null)
            {
                if (counter > config.FileOffset)
                {

                    string[] parsedLine = line.Split(',');
                    WebService webService = new WebService
                    {
                        Title = parsedLine[9],
                        QualityAttributeValues = new List<QualityAttributeValue>
                    {
                        new QualityAttributeValue
                        {
                            QualityAttribute = config.QualityAttributeWeights
                                .Where(q => q.QualityAttribute.Title.Trim()
                                .Equals("ResponseTime",StringComparison.OrdinalIgnoreCase))
                                .Select(q=>q.QualityAttribute).First(),

                            Value = Convert.ToDouble(parsedLine[0])
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = config.QualityAttributeWeights
                                .Where(q => q.QualityAttribute.Title.Trim()
                                    .Equals("Availability",StringComparison.OrdinalIgnoreCase))
                                .Select(q=>q.QualityAttribute).First(),

                            Value = Convert.ToDouble(parsedLine[1])/100
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = config.QualityAttributeWeights
                                .Where(q => q.QualityAttribute.Title.Trim()
                                    .Equals("Throughput",StringComparison.OrdinalIgnoreCase))
                                .Select(q=>q.QualityAttribute).First(),

                            Value = Convert.ToDouble(parsedLine[2])
                        },
                        new QualityAttributeValue
                        {
                            QualityAttribute = config.QualityAttributeWeights
                                .Where(q => q.QualityAttribute.Title.Trim()
                                    .Equals("Reliability",StringComparison.OrdinalIgnoreCase))
                                .Select(q=>q.QualityAttribute).First(),

                            Value = Convert.ToDouble(parsedLine[4])/100
                        }
                    }
                    };

                    webServices.Add(webService);
                }

                counter++;
            }

            file.Close();

            int servicesPerTask = Math.Min(config.CandidatesPerTask,
                webServices.Count / config.Tasks.Count);

            int offsetToSkip = 0;

            List<TaskCandidateService> taskCandidateServices = new List<TaskCandidateService>();
            foreach (SingleTask task in config.Tasks)
            {
                TaskCandidateService taskCandidateService = new TaskCandidateService
                {
                    Task = task,
                    WebServices = webServices.Skip(offsetToSkip).Take(servicesPerTask).ToList()
                };

                offsetToSkip += servicesPerTask;

                taskCandidateServices.Add(taskCandidateService);
            }

            foreach (QualityAttributeWeight attributeWeight in config.QualityAttributeWeights)
            {
                attributeWeight.QualityAttribute.CalculateMinAndMax(webServices);
            }

            return new CompositionRequest
            {
                TaskCandidateServices = taskCandidateServices
            };
        }


       

    }
}
