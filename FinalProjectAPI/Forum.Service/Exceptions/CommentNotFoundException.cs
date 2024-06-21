namespace Forum.Service.Exceptions
{
    public class CommentNotFoundException : Exception
    {
        public CommentNotFoundException() : base("Comment not found in the database")
        {
        }
    }
}
