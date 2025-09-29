using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Common.Interfaces.RepositoryInterfaces;
using Application.DTOs;
using Application.FinancingLeads.Commands.SubmitFinancingLead;
using Domain.Entities;
using Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INotification = Application.Common.Interfaces.EventInterfaces.INotification;

namespace Application.FinancingLeads.Commands.ReviewFinancingLead
{
    public class ReviewFinancingLeadHandler : IRequestHandler<ReviewFinancingLeadCommand, ReviewDTO>
    {
        private readonly IFinancingLeadRepository _repository;
        private readonly INotification _notification;
        private readonly ILogger<ReviewFinancingLeadHandler> _logger;

        public ReviewFinancingLeadHandler(IFinancingLeadRepository repository, INotification notification, ILogger<ReviewFinancingLeadHandler> logger)
        {
            _repository = repository;
            _notification = notification;
            _logger = logger;
        }

        public async Task<ReviewDTO> Handle(ReviewFinancingLeadCommand request, CancellationToken cancellationToken)
        {
            FinancingLead financing = await _repository.GetByIdAsync(request.Request.ID);
            if (financing != null)
            {
                financing.Review(request.Request.Status, request.Request.Reason);

                try
                {
                    await _repository.SaveChangesAsync();

                    var acceptedEvent = financing.Events.OfType<LeadReviewedDomainEvent>().FirstOrDefault();
                   
                    if (acceptedEvent != null)
                    {
                        var topic = acceptedEvent.PhoneE164.NormalizeForTopic();
                        var title = "Lead Accepted";
                        var body = "Your financing lead was accepted.";

                        await _notification.SendNotificationAsync(topic, title,body);

                        _logger.LogInformation("Notification sent for accepted lead {LeadId} to topic {Topic}", acceptedEvent.LeadId, topic);
                    }

                    financing.ClearEvents();

                    return new ReviewDTO()
                    {
                        Decision = financing.Status,
                        Reason = financing.ReviewReason,
                    };
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogWarning(ex, "Concurrency conflict during review for lead {LeadId}", financing.Id);
                    throw new ConcurrencyException("The lead was modified by another user. Please retry the review.", ex);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Exception during review for lead {LeadId}", financing.Id);
                    throw new Exception("Please retry the review.", ex);
                }
            }
            return null;
        }

        
    }
}
