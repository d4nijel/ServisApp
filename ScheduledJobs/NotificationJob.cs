using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Quartz;
using ServisApp.Data;
using ServisApp.Models;
using ServisApp.SendEmails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServisApp.ScheduledJobs
{
    [DisallowConcurrentExecution]
    public class NotificationJob : IJob
    {
        private readonly EmailNotificationMetadata _notificationMetadata;
        private readonly IServiceScopeFactory scopeFactory;
        public NotificationJob(IServiceScopeFactory scopeFactory, EmailNotificationMetadata notificationMetadata)
        {
            this.scopeFactory = scopeFactory;
            _notificationMetadata = notificationMetadata;
        }

        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            { Text = message.Content };
            return mimeMessage;
        }

        public Task Execute(IJobExecutionContext context)
        {
            List<Ispitivanje> ispitivanja = null;
            List<KlijentskiRacun> klijentskiRacuni = null;

            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<MojContext>();

                ispitivanja = _context.Ispitivanja.Where(w => w.TipIspitivanja == "Redovno").Include(i => i.NazivIspitivanja).Include(n => n.RadniNalog).ThenInclude(t => t.Objekat).ToList();
                klijentskiRacuni = _context.KlijentskiRacuni.Where(w => w.EmailNotifikacija == true && w.KlijentskiRacunStatus == true).ToList();
            }

            if (ispitivanja != null && klijentskiRacuni != null)
            {
                foreach (var ispitivanje in ispitivanja)
                {
                    foreach (var klijentskiRacun in klijentskiRacuni)
                    {
                        if (ispitivanje.DatumNarednogIspitivanja.Date.Subtract(DateTime.Now.Date).Days == klijentskiRacun.BrojDanaPrijeIsteka)
                        {
                            var ImePrezimeKlijenta = klijentskiRacun.Ime + " " + klijentskiRacun.Prezime;
                            var NazivPregleda = ispitivanje.NazivIspitivanja.Naziv;
                            var Objekat = ispitivanje.RadniNalog.Objekat.Naziv;
                            var DatumPosljednjegRedovnogPregleda = ispitivanje.DatumIspitivanja.Date.ToString("dd.MM.yyyy");
                            var BrojDanaDoIsteka = klijentskiRacun.BrojDanaPrijeIsteka;
                            var EmailKlijenta = klijentskiRacun.Email;

                            EmailMessage message = new EmailMessage()
                            {
                                Sender = new MailboxAddress("ServisApp", _notificationMetadata.Sender),
                                Reciever = new MailboxAddress(EmailKlijenta),
                                Subject = "Obavijest o planiranju izvršenja zadatka: \"" + NazivPregleda + "\" u objektu " + Objekat,
                                Content = "Poštovani " + ImePrezimeKlijenta + "," +
                                "<br>" +
                                "Obavještavamo Vas da je redovni/redovno:" +
                                "<br>" +
                                "\"" + "<strong>" + NazivPregleda + "</strong>" + "\" u Vašem objektu: " + "<strong>" + Objekat + "</strong>" + ", posljednji put izvršen(o) " + "<strong>" + DatumPosljednjegRedovnogPregleda + "</strong>" + "." +
                                "<br>" +
                                "U narednih " + "<strong>" + BrojDanaDoIsteka + "</strong>" + " dana potrebno je izvršiti naredni redovan pregled/ispitivanje." +
                                "<br>" + "Lijep pozdrav," + "<br>" + "ServisApp Tim"
                            };

                            var mimeMessage = CreateMimeMessageFromEmailMessage(message);

                            using SmtpClient smtpClient = new SmtpClient();
                            smtpClient.Connect(_notificationMetadata.SmtpServer, _notificationMetadata.Port, false);
                            smtpClient.Authenticate(_notificationMetadata.UserName, _notificationMetadata.Password);
                            smtpClient.Send(mimeMessage);
                            smtpClient.Disconnect(true);
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
