using Swashbuckle.AspNetCore.Filters;

namespace EventsAPI.ResponseExamples;

public class GuestExample
{
    public class Guest : IExamplesProvider<EventsDTO.Guest>
    {
        public EventsDTO.Guest GetExamples()
        {
            return new EventsDTO.Guest
            {
                Id = 1,
                FullName = "Jane Doe",
                Position = "Profession title",
                Bio = "Jane Doe's biography",
                Social = "https://twitter.com/JaneDoe",
                WebSite = "https://guestwebsite.com"
            };
        }
    }

    public class GuestResponse : IExamplesProvider<EventsDTO.GuestResponse>
    {
        public EventsDTO.GuestResponse GetExamples()
        {
            return new EventsDTO.GuestResponse
            {
                Id = 1,
                FullName = "Jane Doe",
                Position = "Profession title",
                Bio = "Jane Doe's biography",
                Social = "https://twitter.com/JaneDoe",
                WebSite = "https://guestwebsite.com",
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
