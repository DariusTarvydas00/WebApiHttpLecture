namespace WebApi.ServiceLayer.DTOs
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}
