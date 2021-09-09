using System.Collections.Generic;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace Disc_Store.Entities
{
    public class MyUser:IdentityUser
    {



        public string imgLink { get; set;}
         
    
        
    }
}