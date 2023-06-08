namespace todoApp.Models
{
    public class Token
    {
        public Token()
        {
        }

        public string token { get; set; }

        public Guid userid { get; set; }
    }
    public class Result<T>
    {
        public Result()
        {
        }

        public bool Success { get; set; } = false;
        public T data { get; set; }
        public string? message { get; set; }
    }
}
