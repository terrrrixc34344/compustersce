using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ComputerShop.AppDataFiles;
using ComputerShop.Pages;
using MySql;
using MySql.Data.MySqlClient;
using System.Data;
using System.IO;
using System.Windows.Threading;
using Tulpep.NotificationWindow;
using FluentValidation;

namespace ComputerShop
{
    /// <summary>
    /// Логика взаимодействия для AutorizationWind.xaml
    /// </summary>
    

    public partial class AutorizationWind : Window
    {
        DispatcherTimer timer;
        bool finded = false;
        public AutorizationWind()
        {
            ConnectionBD.connect = new ComputerShopEntities5();
            InitializeComponent();
            ImgShowHide.MouseDown += ImgShowHide_PreviewMouseDown;
            ImgShowHide.MouseUp += ImgShowHide_PreviewMouseUp;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 3, 0);
            timer.Tick += Timer_Tick;
        }
        public class UserValidator : AbstractValidator<UserValid>
        {

            public UserValidator()
            {
                RuleFor(x => x.Login).NotEmpty().WithMessage("Логин не может быть пустым");
                RuleFor(x => x.Password).NotEmpty().WithMessage("Пароль не может быть пустым");
                RuleFor(x => x.Count).NotEmpty().WithMessage("Такого пользователя не существует");
            }

        }
        public class UserValid
        {
            
            public string Login { get; set; }
            public string Password { get; set; }
            public int Count { get; set; }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
           
        }
        void ShowPassword()
        {
            CopyPass.Text = Pass_TB.Password;
            CopyPass.Visibility = Visibility.Visible;
            Pass_TB.Visibility = Visibility.Hidden;
        }
        void HidePassword()
        {
            CopyPass.Visibility = Visibility.Hidden;
            Pass_TB.Visibility = Visibility.Visible;
        }

        private void ImgShowHide_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ShowPassword();
        }

        private void ImgShowHide_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            HidePassword();
        }

      
        public void Update()
        {
            try
            {
                    foreach (var us in ConnectionBD.connect.Users)
                    {
                        if (us.USLogin == Email_TB.Text && us.Pass == Pass_TB.Password)
                        {
                            PersonsInfo.IdToSale = us.ID;
                            UsersAutoriz.ID = us.USType;
                            foreach(var person in ConnectionBD.connect.InfoUsers)
                            {
                                if(person.ID == PersonsInfo.IdToSale)
                                {
                                    PersonsInfo.Name = person.USName;
                                    PersonsInfo.Surname = person.Surname;
                                    PersonsInfo.Lastname = person.Middlename;
                                }
                            }
                            menu men = new menu();
                            men.Show();
                            this.Hide();
                        }
                    }     
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Chekc_BT_Click(object sender, RoutedEventArgs e)
        {
            var validator = new UserValidator();
            var us = new UserValid { Login = Email_TB.Text, Password = Pass_TB.Password,Count = ConnectionBD.connect.Users.Where(x => x.USLogin == Email_TB.Text && x.Pass == Pass_TB.Password).ToList().Count};
            var ValidationResults = validator.Validate(us);
            foreach(var validationResult in ValidationResults.Errors)
            {
                MessageBox.Show(validationResult.ToString());
            }
            Update();
        }

        private void Email_TB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Email_TB.Clear();
        }

        private void Pass_TB_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Pass_TB.Clear();
            CopyPass.Clear();
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            MainWindow wind = new MainWindow();
            wind.Show();
        }

    }
}
