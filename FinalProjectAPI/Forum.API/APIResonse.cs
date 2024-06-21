namespace Forum.API
{
    public class APIResonse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public object Result { get; set; }
        public int StatusCode { get; set; }
    }
}
