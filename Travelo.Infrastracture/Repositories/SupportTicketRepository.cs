using Travelo.Application.DTOs.SupportTicket;
using Travelo.Application.Interfaces;
using Travelo.Domain.Models.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travelo.Infrastracture.Repositories;
using Travelo.Infrastracture.Contexts;
using Travelo.Domain.Models.Entities;

namespace Travelo.Infrastracture.Repositories
{
    public class SupportTicketRepository : GenericRepository<SupportTicket>, ISupportTicket
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public SupportTicketRepository(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(context)
        {
            _context = context;
            _userManager = userManager;
        }

       
        public async Task CreateSupportTicketAsync(SupportTicketDTO supportTicketDTO , string userId)
        {
            if (string.IsNullOrWhiteSpace(supportTicketDTO.Subject))
                throw new ArgumentNullException("Subject is required");

            if (string.IsNullOrWhiteSpace(supportTicketDTO.Details))
                throw new ArgumentNullException("Details is required");

            
           
            var ticket = new SupportTicket
            {
                userId = userId,
                Subject = supportTicketDTO.Subject,
                Details = supportTicketDTO.Details,
                Status = "New"
            };

            await _context.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GetSupportTicketDTO>> GetSupportTicketsAsync()
        {
            var tickets = await _context.SupportTickets
                .Include(t => t.User)
                    .ThenInclude(u => u.Doctor)
                .Include(t => t.User)
                    .ThenInclude(u => u.Patient)
               
                .Select(t => new GetSupportTicketDTO
                {
                    userName = t.User.UserName!,
                    Experience = t.User.Doctor != null
                        ? t.User.Doctor.ExperienceYears
                        : null,
                    Subject = t.Subject,
                    Status = t.Status
                })
                .ToListAsync();

            return tickets;
        }
        public async Task AddSupportTicketReplyAsync(AddSupportTicketReplyDTO supportTicketReplyDTO)
        {
            var ticket = await _context.SupportTickets.FirstOrDefaultAsync(t => t.Id == supportTicketReplyDTO.TicketId);

            if (ticket == null)
                throw new Exception("Ticket not found");

            ticket.Reply = supportTicketReplyDTO.Reply;
            ticket.Status = "Replied";

            await _context.SaveChangesAsync();
        }

        public async Task<SupportTicketReplyDTO?> GetLatestReplyByUserAsync(string userId)
        {
            var ticket = await _context.SupportTickets
                .Include(t => t.User)
                    .ThenInclude(u => u.Doctor)
                .Include(t => t.User)
                    .ThenInclude(u => u.Patient)
                .Where(t => t.userId == userId && !string.IsNullOrEmpty(t.Reply))
                .OrderByDescending(t => t.Id) 
                .FirstOrDefaultAsync();

            if (ticket == null) return null;

            return new SupportTicketReplyDTO
            {
                userId = userId,
                TicketId = ticket.Id,
                Subject = ticket.Subject,
                Details = ticket.Details,
                Reply = ticket.Reply
            };
        }
    }
    }
