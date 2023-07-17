using Swashbuckle.AspNetCore.Filters;

namespace EventsAPI.ResponseExamples;

public class TalkExample
{
    public class Talk : IExamplesProvider<EventsDTO.Talk>
    {
        public EventsDTO.Talk GetExamples()
        {
            return new EventsDTO.Talk
            {
                Id = 1,
                Title = "Talk title",
                Summarize = "Brief description of the Talk's main purpose",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.UtcNow,
                CategoryId = 1,
                EventId = 1
            };
        }
    }

    public class TalkResponse : IExamplesProvider<EventsDTO.TalkResponse>
    {
        public EventsDTO.TalkResponse GetExamples()
        {
            return new EventsDTO.TalkResponse
            {
                Id = 1,
                Title = "Talk title",
                Summarize = "Brief description of the Talk's main purpose",
                StartTime = DateTimeOffset.Now,
                EndTime = DateTimeOffset.UtcNow,
                CategoryId = 1,
                EventId = 1,
                Guests = new List<EventsDTO.Guest>()
                {
                    new()
                    {
                        Id = 1,
                        FullName = "Jane Doe"
                    }
                },
                Organizations = new List<EventsDTO.Organization>()
                {
                    new()
                    {
                        Id = 1,
                        Name = "Public organization name"
                    }
                }
            };
        }
    }
}
