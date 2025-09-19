namespace SinglePageApplication.ApplicationServices.DTOs
{
    public class GetByIdProductDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal UnitPrice { get; set; }

    }
}
