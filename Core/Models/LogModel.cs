namespace Core.Models
{
    public class LogModel
    {
        public int id { get; set; }
        public string cabinet_uuid { get; set; } = default!;
        public string user_uuid { get; set; } = default!;
        public int slot_id { get; set; } 
        public string order_num { get; set; } = default!;
        public DateTime start_time { get; set; }
        public DateTime? end_time { get; set; }
    }
}
