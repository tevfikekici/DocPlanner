using System;
using System.Net.Http;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using System.Net;
using System.Threading;
using Newtonsoft.Json;
using Xunit;
using DocPlanner.API;
using DocPlanner.API.Controllers;
using Microsoft.AspNetCore.Mvc; // Replace with your actual namespace

namespace DocPlanner.Test
{
    public class SlotServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHandler;
        private readonly SlotService _slotService;

        public SlotServiceTests()
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_mockHandler.Object)
            {
                BaseAddress = new Uri("https://draliatest.azurewebsites.net/api/availability/")
            };

            _slotService = new SlotService(httpClient);
        }   

        [Fact]
        public async Task GetWeeklyAvailabilityAsync_ReturnsAvailability()
        {
            // Arrange
            var jsonContent = File.ReadAllText("GetResult.json"); 
            var expectedResponse = JsonConvert.DeserializeObject<WeeklyAvailability>(jsonContent);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonContent) 
            };

            _mockHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var testDate = new DateTime(2023, 11, 20); 

            // Act
            var result = await _slotService.GetWeeklyAvailabilityAsync(testDate);

            // Assert
            Assert.NotNull(result);
            // Assert facility details
            Assert.Equal(expectedResponse.Facility.Name, result.Facility.Name);
            Assert.Equal(expectedResponse.Facility.Address, result.Facility.Address);

            // Assert slot duration
            Assert.Equal(expectedResponse.SlotDurationMinutes, result.SlotDurationMinutes);

            // Assert details for specific days, e.g., Monday
            Assert.Equal(expectedResponse.Monday.WorkPeriod.StartHour, result.Monday.WorkPeriod.StartHour);
            // ... other asserts for WorkPeriod of rest of the week days

            // Assert busy slots for Monday
            Assert.Equal(expectedResponse.Monday.BusySlots.Count, result.Monday.BusySlots.Count);
            for (int i = 0; i < expectedResponse.Monday.BusySlots.Count; i++)
            {
                Assert.Equal(expectedResponse.Monday.BusySlots[i].Start, result.Monday.BusySlots[i].Start);
                Assert.Equal(expectedResponse.Monday.BusySlots[i].End, result.Monday.BusySlots[i].End);
            }

            // ... similar asserts for other days of the week

            // Assert null days (if applicable)
            Assert.Null(result.Tuesday);
            Assert.Null(result.Thursday);
            // ... similarly for other days that are expected to be null
        }

        [Fact]
        public async Task TakeSlot_ReturnsOkResult_WhenSlotBookingIsSuccessful()
        {
            // Arrange
            var mockService = new Mock<ISlotService>();
            var slotBooking = new SlotBooking
            {
                Start = "2023-11-30 11:00:00",
                End = "2023-11-30 12:00:00",
                Comments = "my arm hurts a lot",
                Patient = new Patient
                {
                    Name = "Mario",
                    SecondName = "Neta",
                    Email = "mario@myspace.es",
                    Phone = "555 44 33 22"
                }
            };

            mockService.Setup(s => s.TakeSlotAsync(It.Is<SlotBooking>(sb =>
                sb.Start == slotBooking.Start &&
                sb.End == slotBooking.End &&
                sb.Comments == slotBooking.Comments &&
                sb.Patient.Name == slotBooking.Patient.Name &&
                sb.Patient.SecondName == slotBooking.Patient.SecondName &&
                sb.Patient.Email == slotBooking.Patient.Email &&
                sb.Patient.Phone == slotBooking.Patient.Phone
            ))).ReturnsAsync(new HttpRequestResult
            {
                StatusCode = "OK",
                Content = "Slot booked successfully"
            });

            var controller = new AvailabilityController(mockService.Object);

            // Act
            var result = await controller.TakeSlot(slotBooking);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, okResult.StatusCode);
        }

        [Fact]
        public async Task TakeSlot_ReturnsBadRequest_WhenServiceReturnsError()
        {
            // Arrange
            var mockService = new Mock<ISlotService>();
            mockService
                .Setup(service => service.TakeSlotAsync(It.IsAny<SlotBooking>()))
                .ReturnsAsync(new HttpRequestResult
                {
                    StatusCode = "BadRequest",
                    Content = "Error message"
                });

            var controller = new AvailabilityController(mockService.Object);
            var slotBooking = new SlotBooking
            {
                Start = "2023-11-30 11:00:00",
                End = "2023-11-30 12:00:00",
                Comments = "my arm hurts a lot",
                Patient = new Patient
                {
                    Name = "Mario",
                    SecondName = "Neta",
                    Email = "mario@myspace.es",
                    Phone = "555 44 33 22"
                }
            };
            // Act
            var result = await controller.TakeSlot(slotBooking);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.Equal("BadRequest - Error message", badRequestResult.Value);
        }
    }
}

