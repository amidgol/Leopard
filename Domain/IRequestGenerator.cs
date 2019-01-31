using System;
using System.Collections.Generic;
using System.Text;
using Domain.Models;

namespace Domain
{
    public interface IRequestGenerator
    {
        CompositionRequest Generate(Config config);
    }
}
