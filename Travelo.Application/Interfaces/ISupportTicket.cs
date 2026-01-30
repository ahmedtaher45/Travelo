using Travelo.Application.DTOs.SupportTicket;
using Travelo.Domain.Models.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Application.Interfaces;

namespace Travelo.Application.Interfaces
{
    public interface ISupportTicket: IGenericRepository<SupportTicket>
    {
        Task CreateSupportTicketAsync(SupportTicketDTO supportTicketDTO , string userId);
        Task<List<GetSupportTicketDTO>> GetSupportTicketsAsync();
        Task AddSupportTicketReplyAsync(AddSupportTicketReplyDTO supportTicketReplyDTO);
        Task<SupportTicketReplyDTO?> GetLatestReplyByUserAsync(string userId);
    }
}
    