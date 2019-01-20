using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface ICompositionConfig
    {
         int Tasks { get; set; }
         int CandidatesPerTask { get; set; }


    }
}
