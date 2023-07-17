using Swashbuckle.AspNetCore.Filters;

namespace EventsAPI.ResponseExamples;

public class CategoryExample
{
    public class Category : IExamplesProvider<EventsDTO.Category>
    {
        public EventsDTO.Category GetExamples()
        {
            return new EventsDTO.Category()
            {
                Id = 1,
                Name = "Category name"
            };
        }
    }

    public class CategoryResponse : IExamplesProvider<EventsDTO.CategoryResponse>
    {
        public EventsDTO.CategoryResponse GetExamples()
        {
            return new EventsDTO.CategoryResponse()
            {
                Id = 1,
                Name = "Category name",
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
