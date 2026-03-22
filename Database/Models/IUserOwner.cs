using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
    public interface IUserOwned
    {
        string UserId { get; set; }
    }
}
