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
using Microsoft.Win32;
using PraktikaChat.ClassHelper;
using System.IO;

namespace PraktikaChat.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private bool isLogTbEmpty = true;
        private bool isPasTbEmpty = true;
        private bool isPasRepeatTbEmpty = true;
        private bool isModeRegister = true;
        private Uri imageSource = new Uri("Images/DefaultProfilePicture.png", UriKind.Relative);
        private bool isPasHidden = false;
        MainWindow mw;
        public LoginPage(MainWindow window)
        {
            InitializeComponent();
            mw = window;
        }

        private void BtnNewImage_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnNewImage.Opacity = 1;
        }

        private void BtnNewImage_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnNewImage.Opacity = 0.7;
        }

        private void TBLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isLogTbEmpty)
            {
                TBLogin.Foreground = Brushes.Black;
                TBLogin.Text = "";
            }
        }

        private void TBLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            isLogTbEmpty = TBLogin.Text == "";
            if (isLogTbEmpty)
            {
                TBLogin.Foreground = Brushes.LightGray;
                TBLogin.Text = "Login";
            }
        }

        private void TBPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isPasHidden)
            {
                if (!isPasTbEmpty || TBPassword.Foreground != Brushes.LightGray)
                {
                    PBPassword.Password = TBPassword.Text;
                }
                TBPassword.Visibility = Visibility.Hidden;
                PBPassword.Visibility = Visibility.Visible;
                PBPassword.Focus();
            }
            else
            {
                if (isPasTbEmpty || TBPassword.Foreground == Brushes.LightGray)
                {
                    TBPassword.Foreground = Brushes.Black;
                    TBPassword.Text = "";
                }
            }
        }

        private void TBPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!isPasHidden)
            {
                isPasTbEmpty = TBPassword.Text == "";
                if (isPasTbEmpty)
                {
                    TBPassword.Foreground = Brushes.LightGray;
                    TBPassword.Text = "Password";
                }
            }
        }

        private void TBPasswordRepeat_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isPasRepeatTbEmpty)
            {
                TBPasswordRepeat.Foreground = Brushes.Black;
                TBPasswordRepeat.Text = "";
            }
        }

        private void TBPasswordRepeat_LostFocus(object sender, RoutedEventArgs e)
        {
            isPasRepeatTbEmpty = TBPasswordRepeat.Text == "";
            if (isPasRepeatTbEmpty)
            {
                TBPasswordRepeat.Foreground = Brushes.LightGray;
                TBPasswordRepeat.Text = "Repeat Password";
            }
        }

        private void BtnLogin_MouseEnter(object sender, MouseEventArgs e)
        {
            BtnLogin.Background = Brushes.AliceBlue;
        }

        private void BtnLogin_MouseLeave(object sender, MouseEventArgs e)
        {
            BtnLogin.Background = Brushes.DarkTurquoise;
        }

        private void BtnNewImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imageSource = new Uri(openFileDialog.FileName);
                ImgUser.Source = new BitmapImage(imageSource);
            }
        }

        private void BtnChangeMode_Click(object sender, RoutedEventArgs e)
        {
            if (isModeRegister)
            {
                BtnLogin.Content = "Log in";
                BtnChangeMode.Content = "Register";
                isPasHidden = true;
                BtnPasVisibility.Visibility = Visibility.Visible;
                if (!isPasTbEmpty)
                {
                    PBPassword.Password = TBPassword.Text;
                    TBPassword.Visibility = Visibility.Hidden;
                    PBPassword.Visibility = Visibility.Visible;
                }
                ImgUser.Visibility = Visibility.Hidden;
                BtnNewImage.Visibility = Visibility.Hidden;
                TBPasswordRepeat.Visibility = Visibility.Hidden;
            }
            else
            {
                BtnLogin.Content = "Register";
                BtnChangeMode.Content = "Log in";
                BtnPasVisibility.Visibility = Visibility.Hidden;
                if (isPasHidden && PBPassword.Password != "")
                {
                    TBPassword.Text = PBPassword.Password;
                    TBPassword.Foreground = Brushes.Black;
                    isPasTbEmpty = false;
                    TBPassword.Text = PBPassword.Password;
                    PBPassword.Password = "";
                }
                else if (isPasHidden && PBPassword.Password == "")
                {
                    TBPassword.Foreground = Brushes.LightGray;
                    TBPassword.Text = "Password";
                }
                PBPassword.Visibility = Visibility.Hidden;
                TBPassword.Visibility = Visibility.Visible;
                isPasHidden = false;
                ImgUser.Visibility = Visibility.Visible;
                BtnNewImage.Visibility = Visibility.Visible;
                TBPasswordRepeat.Visibility = Visibility.Visible;
            }
            isModeRegister = !isModeRegister;
        }

        private void PBPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            //not used
        }

        private void PBPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PBPassword.Password == "")
            {
                TBPassword.Text = "Password";
                TBPassword.Foreground = Brushes.LightGray;
                PBPassword.Visibility = Visibility.Hidden;
                TBPassword.Visibility = Visibility.Visible;
            }
        }

        private void BtnPasVisibility_Click(object sender, RoutedEventArgs e)
        {
            if (isPasHidden)
            {
                if (PBPassword.Password != "")
                {
                    TBPassword.Text = PBPassword.Password;
                    TBPassword.Foreground = Brushes.Black;
                    isPasTbEmpty = false;
                    PBPassword.Password = "";
                }
                PBPassword.Visibility = Visibility.Hidden;
                TBPassword.Visibility = Visibility.Visible;
            }
            else
            {
                if (!isPasTbEmpty)
                {
                    PBPassword.Password = TBPassword.Text;
                }
                TBPassword.Visibility = Visibility.Hidden;
                PBPassword.Visibility = Visibility.Visible;
            }
            isPasHidden = !isPasHidden;
        }

        private bool checkIsLoginFree(string log)
        {
            return !LinkHandler.LoginExists(log);
        }
        private bool tryLogIn(string log, string pas)
        {
            return LinkHandler.TryLogIn(log, pas);
        }

        private string getLog
        {
            get
            {
                if (TBLogin.Foreground == Brushes.LightGray)
                {
                    return "";
                }
                return TBLogin.Text;
            }
        }
        private string getPas
        {
            get
            {
                if (isPasHidden)
                {
                    return PBPassword.Password;
                }
                else
                {
                    if (TBPassword.Foreground == Brushes.LightGray)
                    {
                        return "";
                    }
                    return TBPassword.Text;
                }
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            bool logCheck = false;
            bool pasCheck = false;
            if (isModeRegister)
            {
                if (checkIsLoginFree(getLog))
                {
                    if (getLog.Length > 16 || getLog.Length < 5)
                    {
                        TBLogin.BorderBrush = Brushes.Red;
                        TBLoginError.Text = "Login must be at least 5 characters long and not longer than 15";
                    }
                    else
                    {
                        TBLogin.BorderBrush = Brushes.SteelBlue;
                        TBLoginError.Text = "";
                        logCheck = true;
                    }
                }
                else
                {
                    TBLogin.BorderBrush = Brushes.Red;
                    TBLoginError.Text = "User with this login already exists";
                }
                int pasValidation = PasswordTestClass.Verify(getPas);
                switch (pasValidation)
                {
                    case 0:
                        if (getPas != TBPasswordRepeat.Text)
                        {
                            TBPassword.BorderBrush = Brushes.Red;
                            PBPassword.BorderBrush = Brushes.Red;
                            TBPasswordRepeat.BorderBrush = Brushes.Red;
                            TBPasswordError.Text = "Repeat your password";
                        }
                        else
                        {
                            TBPassword.BorderBrush = Brushes.SteelBlue;
                            PBPassword.BorderBrush = Brushes.SteelBlue;
                            TBPasswordRepeat.BorderBrush = Brushes.SteelBlue;
                            TBPasswordError.Text = "";
                            pasCheck = true;
                        }
                        break;
                    case 1:
                        TBPassword.BorderBrush = Brushes.Red;
                        PBPassword.BorderBrush = Brushes.Red;
                        TBPasswordError.Text = "Password length must be from 6 to 12 symbols";
                        break;
                    case 2:
                        TBPassword.BorderBrush = Brushes.Red;
                        PBPassword.BorderBrush = Brushes.Red;
                        TBPasswordError.Text = "Password can only use characters from English alphabet, numbers and special symbols: ! ? @ #";
                        break;
                    case 3:
                        TBPassword.BorderBrush = Brushes.Red;
                        PBPassword.BorderBrush = Brushes.Red;
                        TBPasswordError.Text = "Password must contain a lowercase letter, an upper case letter, a number and a special symbol (! ? @ #)";
                        break;
                }
            }
            else
            {
                if (!checkIsLoginFree(getLog))
                {
                    TBLogin.BorderBrush = Brushes.SteelBlue;
                    TBLoginError.Text = "";
                    logCheck = true;
                }
                else
                {
                    TBLogin.BorderBrush = Brushes.Red;
                    TBLoginError.Text = "User with this login does not exist";
                }
                if (tryLogIn(getLog, getPas))
                {
                    TBPassword.BorderBrush = Brushes.SteelBlue;
                    PBPassword.BorderBrush = Brushes.SteelBlue;
                    pasCheck = true;
                }
                else
                {
                    TBPassword.BorderBrush = Brushes.Red;
                    PBPassword.BorderBrush = Brushes.Red;
                    TBPasswordError.Text = "User with this login and password does not exist ";
                }
            }
            if (logCheck && pasCheck)
            {
                UserClass logInUser;
                if (isModeRegister)
                {
                    bool registerSuccess;
                    if (imageSource == new Uri("Images/DefaultProfilePicture.png", UriKind.Relative))
                    {
                        registerSuccess = LinkHandler.Register(getLog, getPas);
                    }
                    else
                    {
                        registerSuccess = LinkHandler.Register(getLog, getPas, File.ReadAllBytes(imageSource.AbsoluteUri.Substring(8)));
                    }
                    if (registerSuccess)
                    {
                        logInUser = LinkHandler.LogIn(getLog, getPas);
                        mw.curPage.Content = new ChatPage(logInUser, getPas);
                    }
                    else
                    {
                        TBLogin.BorderBrush = Brushes.Red;
                        TBPassword.BorderBrush = Brushes.Red;
                        PBPassword.BorderBrush = Brushes.Red;
                        TBPasswordRepeat.BorderBrush = Brushes.Red;
                        TBLoginError.Text = "Register Fail";
                    }
                }
                else
                {
                    logInUser = LinkHandler.LogIn(getLog, getPas);
                    //logInUser = AppData.context.User.First(i => i.Name == getLog);
                    mw.curPage.Content = new ChatPage(logInUser, getPas);
                }
            }
        }
    }
}
