namespace HotelsBookingSystem.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public required string BookingReference { get; set; }
        public required int RoomId { get; set; }
        
        public Room Room { get; set; }
        public Hotel Hotel { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
