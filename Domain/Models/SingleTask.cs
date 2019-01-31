namespace WebServiceComposition.Domain.Models
{
    public class SingleTask
    {
        public string Title { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}