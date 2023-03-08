namespace TestAPI.Models
{
    public class EmailOptions
    {
        public const string EmailConf = "EmailConf";
        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string Host { get; set; } = string.Empty;

        public int Port { get; set; } = 0;

        public bool SSL { get; set; } = false;
    }
}
