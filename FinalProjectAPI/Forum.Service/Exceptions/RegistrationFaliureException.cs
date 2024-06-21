namespace Forum.Service.Exceptions
{
    public class RegistrationFaliureException : Exception
    {
        public RegistrationFaliureException(string message) : base("Registration was failed")
        {
        }
    }
}
