namespace IotCoreDemo.Requests
{
    public class CommandRequest
    {
        public int commad { get; set; }
        public string user_uuid { get; set; } = default!;
    }
}
