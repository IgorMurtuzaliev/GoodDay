using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoodDay.WebAPI.Hubs
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserDialogHub:Hub
    {
        
    }
}
