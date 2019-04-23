using App2.Db;
using App2.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddWork : ContentPage
	{
        public event EventHandler<Work> WorkAdded;
        private SQLiteAsyncConnection _connection;
        private string ActName;

        public AddWork()
        {
            InitializeComponent();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        async void OnActivityChoice(object sender, System.EventArgs e)
        {
            var page = new ActivitiesChoicePage();

            page.ActivitySelected += (source, selectedActivity) =>
            {
               ActName = selectedActivity.Name;
               activButton.Text = ActName;
            };
            await Navigation.PushAsync(page);
        }

        private void OnAddHour(object sender, System.EventArgs e)
        {
            int value;
            if (entryHours.Text == null)
                value = 0;
            else
                value = Int32.Parse(entryHours.Text);

            if(value <24)
                value++;

            entryHours.Text = value.ToString();
        }

        private void OnSubtractHour(object sender, System.EventArgs e)
        {
            int value;
            if (entryHours.Text == null)
                value = 0;
            else
                value = Int32.Parse(entryHours.Text);

            if (value > 0)
                value--;
            
            entryHours.Text = value.ToString();
        }

        private void OnAddMinute(object sender, System.EventArgs e)
        {
            int value;
            if (entryMinutes.Text == null)
                value = 0;
            else
                value = Int32.Parse(entryMinutes.Text);

            if(value<59)
                value++;

            entryMinutes.Text = value.ToString();
        }

        private void OnSubtractMinute(object sender, System.EventArgs e)
        {
            int value;
            if (entryMinutes.Text == null)
                value = 0;
            else
                value = Int32.Parse(entryMinutes.Text);

            if (value > 0)
                value--;

            entryMinutes.Text = value.ToString();
        }
        async void OnSaveNewWork(object sender, System.EventArgs e)
        {
            var comment = workComment.Text;
            var imgSrc = "Working.png";
            var entryValueH = Double.Parse(entryHours.Text);
            var entryValueM = Double.Parse(entryMinutes.Text);
            var durationH = (int)Math.Round(entryValueH) * 3600;
            var durationM = (int)Math.Round(entryValueM) *60;
            var duration = durationH + durationM;
            var date = workDate.Date;


            if ((entryValueH == 0 && entryValueM == 0) || ActName == null)
            {
                await DisplayAlert("Warning", "Fill the required fields", "Ok");
                return;
            }

            if (entryValueH > 24 || entryValueH < 0 || entryValueM > 59 || entryValueM <0)
            {
                await DisplayAlert("Warning", "Enter the correct time", "Ok");
                return;
            }

            var work = new Work
            {
                ActivityName = ActName,
                Comment = comment,
                ImgSrc = imgSrc,
                Duration = duration,
                Date = date
            };
            
            
            await _connection.InsertAsync(work);

            WorkAdded?.Invoke(this, work);

            await Navigation.PopAsync();
        }

    }
}