namespace RestaurantAPI.Dtos.ChefDtos
{
    public class CreateChefDto
    {
        public string ChefNameSurname { get; set; }
        public string ChefTitle { get; set; }
        public string ChefDescription { get; set; }
        public byte[] ChefImage { get; set; }
        public bool ChefStatus { get; set; } = true;
    }
}