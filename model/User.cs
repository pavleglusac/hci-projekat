using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCIProjekat.model
{
    internal class User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }

        public User(string Name, string Surname, string Username, string Password, UserType type)
        {
            this.Name = Name;
            this.Surname = Surname;
            this.Username = Username;
            this.Password = Password;
            this.Type = type;
        }
    }

    internal enum UserType
    {
        CUSTOMER,
        MANAGER
    }
}
