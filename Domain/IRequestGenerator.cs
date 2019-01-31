using WebServiceComposition.Domain.Models;

namespace WebServiceComposition.Domain
{
    public interface IRequestGenerator
    {
        CompositionRequest Generate(Config config);
    }
}
