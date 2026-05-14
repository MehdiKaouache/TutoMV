using Projet_Session_Entreprise.Services;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Projet_Session_Entreprise
{
    /// <summary>
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var archiveService = new ArchiveService();
            archiveService.ArchiveOldAppointments(); 
        }
    }


}
