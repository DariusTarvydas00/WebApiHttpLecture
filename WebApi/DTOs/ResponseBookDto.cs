namespace WebApi.ServiceLayer.DTOs
{
    public class ResponseBookDto
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string? Publisher { get; set; }
        public string? Image_URL_S { get; set; }
        public string? Image_URL_M { get; set; }
        public string? Image_URL_L { get; set; }
    }
}
