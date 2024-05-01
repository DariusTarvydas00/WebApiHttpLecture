namespace WebApi.ServiceLayer.DTOs
{
    public class RequestBookDto
    {
        //ISBN should be part of request
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
    }
}
