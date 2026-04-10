private async void btnConn_Click(object sender, RoutedEventArgs e)
{
    try
    {
        string da = txtDA.Text.Trim();
        string password = txtPassword.Password.Trim();

        var auth = new AuthService();
        var user = await auth.LoginAsync(da, password);

        Window? nextWindow = null;

        if (user is Student s)
        {
            nextWindow = new ProfileView(s);
        }
        else if (user is Tutor t)
        {
            if (t.IsValidated)
            {
                nextWindow = new ProfileView(t);
            }
            else
            {
                nextWindow = new RequeteRoleTuteurView(t, new AppDbContext());
            }
        }
        else
        {
            MessageBox.Show("Erreur DA/Password");
            return;
        }

        if (nextWindow == null)
        {
            MessageBox.Show("Impossible de créer la fenêtre suivante.");
            return;
        }

        // Make the new window the application main window before closing the login window
        Application.Current.MainWindow = nextWindow;

        // Show and activate the new window; only close login when Show succeeds
        nextWindow.Show();
        nextWindow.Activate();

        // Close login window only after showing next window
        this.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show("Erreur lors de la connexion : " + ex.ToString());
    }
}