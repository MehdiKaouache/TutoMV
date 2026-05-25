using Projet_Session_Entreprise.Core.Models;
using System;
using Xunit;

namespace Projet_Session_Entreprise.Tests.Models
{
    public class AppointmentTests
    {
        [Fact]
        public void Appointment_Initialisation_StatutParDefautEnAttente()
        {
            var appointment = new Appointment();
            Assert.Equal("En attente", appointment.Status);
        }

        [Fact]
        public void Appointment_ChangementStatut_ValeurMiseAJour()
        {
            var appointment = new Appointment { Status = "En attente" };
            appointment.Status = "Accepté";
            Assert.Equal("Accepté", appointment.Status);
        }
    }
}