using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Threading;
using Microsoft.Win32;
using PraktikaChat.ClassHelper;

namespace PraktikaChat.Pages
{
    /// <summary>
    /// Логика взаимодействия для ChatPage.xaml
    /// </summary>
    public partial class ChatPage : Page
    {
        private UserClass curUser; //current user
        private Uri imageSource = new Uri("Images/DefaultProfilePicture.png", UriKind.Relative);
        private bool isImageAttached = false;
        private List<UserClass> userSource;
        private List<MessageView> messageSource;
        private int idSelectedUser = -1;
        private byte[] attachedPhoto;
        private string password;
        private DispatcherTimer dispatcherTimer;
        private bool userSelectionChange = true;

        public ChatPage(UserClass user, string pas)
        {
            InitializeComponent();
            curUser = user;
            if (curUser.Photo != null)
            {
                ImgLogedInUser.Source = loadImage(curUser.Photo);
            }
            TBLogedInUser.Text = curUser.Name;
            password = pas;
            dispatcherTimerStart();
            GetUsers();
        }

        private void dispatcherTimerStart()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            GetMessages(idSelectedUser);
            GetUsers();
        }

        public BitmapImage loadImage(byte[] source)
        {
            using (var ms = new System.IO.MemoryStream(source))
            {
                BitmapImage res = new BitmapImage();

                res.BeginInit();
                res.CacheOption = BitmapCacheOption.OnLoad;
                res.StreamSource = ms;
                res.EndInit();

                return res;
            }
        } //https://stackoverflow.com/questions/14337071/convert-array-of-bytes-to-bitmapimage

        private void GetUsers()
        {
            int selectedindex = LVUsers.SelectedIndex;
            List<UserClass> uc = LinkHandler.GetUsers().Where(i => i.IdUser != curUser.IdUser).ToList();
            if (userSource != uc)
            {
                userSource = uc;
                LVUsers.ItemsSource = userSource;
                if (selectedindex != -1)
                {
                    LVUsers.SelectedIndex = selectedindex;
                    userSelectionChange = false;
                }
            }
        }

        private void GetMessages(int IdUser)
        {
            if (IdUser != -1)
            {
                messageSource = LinkHandler.GetMessages(curUser.Name, password, IdUser);
                LVMesages.ItemsSource = messageSource;
            }
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (isImageAttached)
            {
                LinkHandler.SendMessage(curUser.IdUser, idSelectedUser, TBMessage.Text, attachedPhoto);
            }
            else
            {
                LinkHandler.SendMessage(curUser.IdUser, idSelectedUser, TBMessage.Text);
            }
            GetMessages(idSelectedUser);
        }

        private void BtnImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                imageSource = new Uri(openFileDialog.FileName);
                ImgForSending.Source = new BitmapImage(imageSource);
                attachedPhoto = File.ReadAllBytes(openFileDialog.FileName);
                isImageAttached = true;
                SendingImageContainer.Visibility = Visibility.Visible;
            }
        }

        private void BtnCloseImage_Click(object sender, RoutedEventArgs e)
        {
            isImageAttached = false;
            SendingImageContainer.Visibility = Visibility.Hidden;
        }

        private void LVUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LVUsers.SelectedIndex != -1)
            {
                if (userSelectionChange)
                {
                    idSelectedUser = ((UserClass)LVUsers.SelectedItem).IdUser;
                    GetMessages(idSelectedUser);
                }
                else
                {
                    userSelectionChange = true;
                }
            }
        }
    }
}
