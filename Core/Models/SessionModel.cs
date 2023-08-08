namespace Core.Models;

public class SessionModel
{
    public string cabinet_uuid { get; set; } = default!;
    public int slot_id { get; set; }
    public DateTime start_time { get; set; }
}
