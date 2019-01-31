namespace WebServiceComposition.Domain
{
    public interface IFitnessCalculator<in T> where T:class 
    {
        double FitnessFunction(T input);
    }
}
