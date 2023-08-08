namespace IotCoreDemo.Requests
{
    public class CabinetRequest
    {
        public string uuid { get; set; } = default!;
        public string mac_addr { get; set; } = default!;
        public string name { get; set; } = default!;
        public string location { get; set; } = default!;
        public int slot_number { get; set; } = default!;
    }
}
