namespace DotNetOptions.Demo.Api.Options
{
    public record ApplicationOptions
    {
        public const string Name = "Application";

        public string? CompanyName { get; set; }
        public string? City { get; set; }
    }
}
