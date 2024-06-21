namespace Forum.Service.Exceptions
{
    public class TopicNotFoundException : Exception
    {
        public TopicNotFoundException() : base("Topic not found in the database")
        {
        }
    }
}
