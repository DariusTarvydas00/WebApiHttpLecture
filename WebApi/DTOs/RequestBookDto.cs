namespace WebApi.ServiceLayer.DTOs
{
    public class RequestBookDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
    }
}
