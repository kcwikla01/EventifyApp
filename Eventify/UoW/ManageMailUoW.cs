using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Eventify.UoW.Base;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;

namespace Eventify.UoW
{
    public class ManageMailUoW : IManageMailUoW
    {
        private readonly IManageUsersUoW _manageUsersUoW;
        private readonly IManageEventsUoW _manageEventsUoW;
        private readonly IConfiguration _configuration;

        public ManageMailUoW(IManageUsersUoW manageUsersUoW, IManageEventsUoW manageEventsUoW, IConfiguration configuration)
        {
            _manageUsersUoW = manageUsersUoW;
            _manageEventsUoW = manageEventsUoW;
            _configuration = configuration;
        }
        public async Task<bool> SendEventParticipantEmail(int userId, int eventId)
        {
            var findEvent = await _manageEventsUoW.GetEventById(eventId);
            var findUser = await _manageUsersUoW.GetUserById(userId);

            if (findEvent != null && findUser != null)
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_configuration["MailConfiguration:Login"], _configuration["MailConfiguration:Email"]));
                message.To.Add(new MailboxAddress(findUser.Name, findUser.Email));
                message.Subject = "EventifyApp - Event Participation";
                message.Body = new TextPart("plain")
                {
                    Text = $"Hello {findUser.Name},\n\nYou have successfully registered for the event: {findEvent.Name}.\n\nThank you for using EventifyApp!"
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    try
                    {
                        await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        await client.AuthenticateAsync(_configuration["MailConfiguration:Email"], _configuration["MailConfiguration:AppPassword"]);
                        await client.SendAsync(message);
                        await client.DisconnectAsync(true);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending email: {ex.Message}");
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
