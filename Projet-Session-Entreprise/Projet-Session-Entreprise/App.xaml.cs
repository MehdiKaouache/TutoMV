using Projet_Session_Entreprise.Data;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Projet_Session_Entreprise
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            using var context = new AppDbContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            DbInitialSetup.SetupDonnees(context);
        }
    }

}
