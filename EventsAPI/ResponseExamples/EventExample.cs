using Swashbuckle.AspNetCore.Filters;

namespace EventsAPI.ResponseExamples;

public class EventExample
{
    public class Event : IExamplesProvider<EventsDTO.Event>
    {
        public EventsDTO.Event GetExamples()
        {
            return new EventsDTO.Event
            {
                Id = 1,
                Title = "Event title"
            };
        }
    }

    public class EventResponse : IExamplesProvider<EventsDTO.EventResponse>
    {
        public EventsDTO.EventResponse GetExamples()
        {
            return new EventsDTO.EventResponse
            {
                Id = 1,
                Title = "Event title",
                Talks = new List<EventsDTO.Talk>()
                {
                    new()
                    {
                        Id = 1,
                        Title = "Talk One"
                    }
                }
            };
        }
    }
}
