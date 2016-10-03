// Copyright(c) Daniel Veintimilla 2016.

namespace CSharpAssignment.Model
{
    /// <summary>Holds a User/Person information.</summary>
    public class UserModel : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public UserModel()
        {
            Id = -1;
            Name = string.Empty;
            LastName = string.Empty;
            Mail = string.Empty;
            Phone = string.Empty;
            UserName = string.Empty;
            Password = string.Empty;
        }

        public UserModel(
            int id, string name, string lastName, string mail,
            string phone, string userName, string password)
        {
            Id = id;
            Name = name;
            LastName = lastName;
            Mail = mail;
            Phone = phone;
            UserName = userName;
            Password = password;
        }}
}