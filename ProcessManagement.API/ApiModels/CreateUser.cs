using ProcessManagement.Core.Entities;

namespace ProcessManagement.API.ApiModels
{
    public class CreateUser
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public User ToUser()
        {
            return new User()
            {
                Name = this.Name,
                Email = this.Email,
                Password = this.Password,
            };
        }
    }
}
