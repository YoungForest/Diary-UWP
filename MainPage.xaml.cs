using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Diary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public bool flag = false;
        public Library lib = new Library();

        public MainPage()
        {
            this.InitializeComponent();
            update();
            Current = this;
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            if (flag == true)
            {
                // 如果日记有修改
                ShowMessageDialog();
            }
            else
            {
                DiaryTitle.Text = string.Empty;
                DiaryContent.Text = string.Empty;
                flag = false;
            }
        }
        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            if (DiaryTitle.Text.Equals(string.Empty))
            {
                Windows.UI.Popups.MessageDialog messageDialog =
        new Windows.UI.Popups.MessageDialog("Empty title! Input your title please.");
                await messageDialog.ShowAsync();
            }
            else
            {
                flag = false;
                lib.saveInformation(DiaryTitle.Text, DiaryContent.Text);
                update();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!DiaryTitle.Text.Equals(string.Empty))
            {
                lib.deleteInformation(DiaryTitle.Text);
                update();
                flag = false;
                New_Click(null, null);
            }
        }

        private async void Help_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.Popups.MessageDialog messageDialog =
        new Windows.UI.Popups.MessageDialog("This application is a lightweight diaries managing tool!\n\nNew to open a new diary page;\nSave to save the current diary;\nDelete to Delete the current diary;\nHelp to popup this.\n\nIf you find any bugs or want to have any improvements, email to yangsen758@foxmail.com.");
            await messageDialog.ShowAsync();
        }
        private async void botton_click(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;
            string s = b.Name.ToString();
            if (!DiaryTitle.Text.Equals(string.Empty))
                Save_Click(null, null);
            DiaryTitle.Text = s;
            DiaryContent.Text = ApplicationData.Current.LocalSettings.Values[s].ToString();
            await System.Threading.Tasks.Task.Delay(20);
            flag = false;
        }
        private void DiaryTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
        }

        private void DiaryContent_TextChanged(object sender, TextChangedEventArgs e)
        {
            flag = true;
        }

        private void update()
        {
            var items = DiaryList.Items;
            items.Clear();
            var ite = lib.getKeys();
            string[] diary = new string[ite.Count];
            int count = 0;
            foreach (string s in ite)
            {
                if (s.Contains("_time"))
                    continue;
                else
                {
                    diary[count++] = s;
                }
            }

            for (int i = 0; i < count; i++)
            {
                for (int j = i; j < count; j++)
                {
                    DateTime di = DateTime.Parse(ApplicationData.Current.LocalSettings.Values[diary[i] + "_time"].ToString());
                    DateTime dj = DateTime.Parse(ApplicationData.Current.LocalSettings.Values[diary[j] + "_time"].ToString());
                    if (di.CompareTo(dj) < 0)
                    {
                        string temp = diary[i];
                        diary[i] = diary[j];
                        diary[j] = temp;
                    }
                }
            }

            for (int i = 0; i < count; i++)
            {
                string s = diary[i];
                Button b = new Button();
                string t;
                if (ApplicationData.Current.LocalSettings.Values.ContainsKey(s + "_time") && ApplicationData.Current.LocalSettings.Values[s + "_time"] != null)
                    t = ApplicationData.Current.LocalSettings.Values[s + "_time"].ToString();
                else
                    t = string.Empty;
                b.Content = s + "\t" + t;
                b.Name = s;
                b.HorizontalAlignment = HorizontalAlignment.Stretch;
                b.VerticalAlignment = VerticalAlignment.Stretch;
                b.Click += new RoutedEventHandler(botton_click);
                items.Add(b);
            }
        }

        private async void ShowMessageDialog()
        {
            Windows.UI.Popups.MessageDialog messageDialog =
       new Windows.UI.Popups.MessageDialog("Current diary which was changed has not been saved yet.\nDo you want to save it?");
            messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes", uiCommand => { Save_Click(null, null); flag = false; New_Click(null, null);}));
            messageDialog.Commands.Add(new Windows.UI.Popups.UICommand("No", uiCommand => { flag = false; New_Click(null, null); }));
            await messageDialog.ShowAsync();
        }
        //async public void OnSuspending(object sender)
        //{
        //    await this.SaveAsync();
        //    deferral.Complete();
        //}
    }
}
