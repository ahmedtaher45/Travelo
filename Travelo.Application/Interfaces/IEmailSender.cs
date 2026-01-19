using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Services.Auth;

namespace Travelo.Application.Interfaces
{
    public interface IEmailSender
    {
       Task SendEmailAsync(Message message);
    }
}
