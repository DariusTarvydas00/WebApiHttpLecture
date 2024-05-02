namespace WebApi.ServiceLayer.DTOs
{
    public class ReviewDto
    {
        //public int Id { get; set; }
        //public string UserId { get; set; }  // User identifier for the reviewer
        public string ISBN { get; set; }     // Associated book identifier
        public string UserName { get; set; } // Name of the reviewer
        public string BookTitle { get; set; } // Title of the book reviewed
        public string ReviewText { get; set; }  // Content of the review
        public int Rating { get; set; }      // Rating given in the review
    }
}
