namespace TestAPI.Models
{
    public class TodoItemDTO2
    {


        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        public IFormFile? Image { get; set; }


    }
}
