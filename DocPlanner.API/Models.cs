namespace DocPlanner.API
{
    public class WeeklyAvailability
    {
        public Facility Facility { get; set; }
        public int SlotDurationMinutes { get; set; }
        // Include other days as well
        public DailyAvailability Monday { get; set; }
        public DailyAvailability Tuesday { get; set; }
        public DailyAvailability Wednesday { get; set; }
        public DailyAvailability Thursday { get; set; }
        public DailyAvailability Friday { get; set; }
        public DailyAvailability Saturday { get; set; }
        public DailyAvailability Sunday { get; set; }
    }

    public class Facility
    {
        public string Name { get; set; }
        public string Address { get; set; }
    }

    public class DailyAvailability
    {
        public WorkPeriod WorkPeriod { get; set; }
        public List<BusySlot> BusySlots { get; set; }
    }

    public class WorkPeriod
    {
        public int StartHour { get; set; }
        public int LunchStartHour { get; set; }
        public int LunchEndHour { get; set; }
        public int EndHour { get; set; }
    }

    public class BusySlot
    {
        public string Start { get; set; }
        public string End { get; set; }
    }

    public class SlotBooking
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Comments { get; set; }
        public Patient Patient { get; set; }
    }

    public class Patient
    {
        public string Name { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }

    public class HttpRequestResult
    {
        public string StatusCode { get; set; }
        public string Content { get; set; }
    }

    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public ErrorResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
