using System;
using System.Collections.Generic;
using System.Text;

namespace DatabaseCreation.Models
{
    public interface IUserOwned
    {
        string UserId { get; set; }
    }
}
