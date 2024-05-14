using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoAPI.Models.LoginModels
{
    public class LoginModel
    {
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
    }
}