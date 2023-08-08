namespace Core.Models
{
    public class CabinetModel
    {
        public string uuid { get; set; } = default!;
        public string mac_addr { get; set; } = default!;
        public string name { get; set; } = default!;
        public string gps_point { get; set; } = default!;
        public int slot_count { get; set; } 
        public int available_slots { get; set; }
        public List<SlotModel> slots { get; set; } = default!;
    }
}
