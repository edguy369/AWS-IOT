namespace Core.Models;
public class SlotModel
{
    public int id { get; set; }
    public SlotStatusModel status { get; set; } = default!;
    public DateTime last_update { get; set; }
}
