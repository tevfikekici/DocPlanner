using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DocPlanner.API
{
    public interface ISlotService
    {
        Task<WeeklyAvailability> GetWeeklyAvailabilityAsync(DateTime date);
        Task<HttpRequestResult> TakeSlotAsync(SlotBooking slotBooking);
    }

    public class SlotService : ISlotService
    {
        private readonly HttpClient _httpClient;
        public SlotService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeeklyAvailability> GetWeeklyAvailabilityAsync(DateTime date)
        {
            var response = await _httpClient.GetAsync($"GetWeeklyAvailability/{date:yyyyMMdd}");
            response.EnsureSuccessStatusCode();

            var availability = await response.Content.ReadFromJsonAsync<WeeklyAvailability>();
            return availability ?? throw new InvalidOperationException("No availability data was returned.");
        }

        public async Task<HttpRequestResult> TakeSlotAsync(SlotBooking slotBooking)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("TakeSlot", slotBooking);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new HttpRequestResult
                    {
                        StatusCode = response.StatusCode.ToString(),
                        Content = content,
                    };
                }

                return new HttpRequestResult
                {
                    StatusCode = response.StatusCode.ToString(),
                    Content = content,
                };
            }
            catch (HttpRequestException e)
            {
                // Log the exception details here
                return new HttpRequestResult
                {
                    StatusCode = "503",
                    Content = e.Message
                };
            }
        }
    }
}
