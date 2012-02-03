
namespace WebMonitor
{
    public class HealthResponse
    {
        public bool IsHealthy { get; set; }
        public string Response { get; set; }
        public int StatusCode { get; set; }
        public string Uri { get; set; }
    }
}
