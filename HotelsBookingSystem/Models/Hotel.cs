namespace HotelsBookingSystem.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        public ICollection<Room> Rooms { get; set; }
    }
}
