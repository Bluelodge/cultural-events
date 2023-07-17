using Swashbuckle.AspNetCore.Filters;

namespace EventsAPI.ResponseExamples;

public class AttendeeExample
{
    public class Attendee : IExamplesProvider<EventsDTO.Attendee>
    {
        public EventsDTO.Attendee GetExamples()
        {
            return new EventsDTO.Attendee()
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                UserName = "JaneDoe_01",
                EmailAddress = "janedoe@email.com",
                PhoneNumber = "5551234"
            };
        }
    }

    public class AttendeeResponse : IExamplesProvider<EventsDTO.AttendeeResponse>
    {
        public EventsDTO.AttendeeResponse GetExamples()
        {
            return new EventsDTO.AttendeeResponse()
            {
                Id = 1,
                FirstName = "Jane",
                LastName = "Doe",
                UserName = "JaneDoe_01",
                EmailAddress = "janedoe@email.com",
                PhoneNumber = "5551234"
            };
        }
    }
}