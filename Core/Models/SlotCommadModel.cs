namespace Core.Models
{
    public class SlotCommadModel
    {
        public int slot_id { get; set; }
        public string slot_info { get; set; } = default!;
        public string order_number { get; set; } = default!;
        public DateTime timestamp { get; set; } 
    }
}
