using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IAlgorithm<in T, in TConfig, out TResult>
    {
        TResult Execute(T input, TConfig config);
    }
}
