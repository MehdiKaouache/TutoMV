using System;
using System.Windows;

namespace Projet_Session_Entreprise.Views
{
    public partial class RoleSelectionView : Window
    {
        public RoleSelectionView()
        {
            InitializeComponent();
            btnStudent.Click += BtnStudent_Click;
            btnTeacher.Click += BtnTeacher_Click;
        }

        private void BtnStudent_Click(object sender, RoutedEventArgs e)
        {
            var studentReg = new RegisterView();
            studentReg.Owner = this.Owner;
            studentReg.Show();
            this.Close();
        }

        private void BtnTeacher_Click(object sender, RoutedEventArgs e)
        {
            var teacherReg = new TeacherRegisterView();
            teacherReg.Owner = this.Owner;
            teacherReg.Show();
            this.Close();
        }
    }
}