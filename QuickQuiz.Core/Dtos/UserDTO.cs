namespace QuickQuiz.Core.Dtos
{
    public record struct UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? PictureUrl { get; set; }
        public string City { get; set; }
        public string Github { get; set; }
        public string Linkedln { get; set; }
        public string Twitter { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
    }
}