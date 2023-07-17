using Swashbuckle.AspNetCore.Filters;

namespace EventsAPI.ResponseExamples;

public class OrganizationExample
{
    public class Organization : IExamplesProvider<EventsDTO.Organization>
    {
        public EventsDTO.Organization GetExamples()
        {
            return new EventsDTO.Organization
            {
                Id = 1,
                Name = "Public organization name",
                CorporateName = "Organization Inc.",
                WebSite = "https://organizationwebsite.com"
            };
        }
    }

    public class OrganizationResponse : IExamplesProvider<EventsDTO.OrganizationResponse>
    {
        public EventsDTO.OrganizationResponse GetExamples()
        {
            return new EventsDTO.OrganizationResponse
            {
                Id = 1,
                Name = "Public organization name",
                CorporateName = "Organization Inc.",
                WebSite = "https://organizationwebsite.com",
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
