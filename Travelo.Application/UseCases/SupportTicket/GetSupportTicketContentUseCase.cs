using Travelo.Application.DTOs.SupportTicket;
using Travelo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Travelo.Application.UseCases.SupportTicket
{
    public class GetSupportTicketContentUseCase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSupportTicketContentUseCase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TicketContentDTO> ExecuteAsync(int ticketId)
        {
            var ticket = await _unitOfWork.SupportTicket.GetById(ticketId);
            if (ticket == null)
            {
                return new TicketContentDTO
                { 
                    Success = false,
                    Message = "Ticket Id could be null"
                };

            }
            var dto = new TicketContentDTO()
            {
                Success = true,
                Subject = ticket.Subject,
                Details = ticket.Details
            };
            return dto;
        }
    }
}
