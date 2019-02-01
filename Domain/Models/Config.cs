using System.Collections.Generic;

namespace WebServiceComposition.Domain.Models
{
    public class Config
    {
        public List<SingleTask> Tasks { get; set; }
        public List<QualityAttributeWeight> QualityAttributeWeights { get; set; }
        public int CandidatesPerTask { get; set; } = 300;
        public string DataSetFilePath { get; set; } = @"C:\Users\Amid\Desktop\QWS_Dataset_v2";
        public int FileOffset { get; set; } = 20;
    }
}
