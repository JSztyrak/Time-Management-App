using App2.Db;
using App2.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Entry = Microcharts.Entry;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microcharts;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkDetailPage : ContentPage
	{
        private Work _thisWork;
        private SQLiteAsyncConnection _connection;
        public event EventHandler<Work> WorkDeleted;
        private TimeConverter _timeConverter;

        public WorkDetailPage (Work work)
		{
            InitializeComponent ();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _timeConverter = new TimeConverter();
            _thisWork = work;
            BindingContext = _thisWork;
        }

        protected override void OnAppearing()
        {
            LoadData();

            base.OnAppearing();
        }

        async private void OnTimerPage(object senser, EventArgs e)
        {
            if (_thisWork.isFinished)
            {
                await DisplayAlert("Job Finished", "You already finished this job", "Ok");
                return;
            }

            var page = new TimerPage(_thisWork);
            

            await Navigation.PushModalAsync(page);
        }

        private void LoadData()
        {
            List<Entry> entries = new List<Entry>
            {
                new Entry(_thisWork.Duration - _thisWork.TimeSpent)
                {
                    Label = "Time Left",
                    Color = SKColor.Parse("#f44144")
                },
                new Entry(_thisWork.TimeSpent)
                {
                    Label = "Time Spent",
                    Color = SKColor.Parse("#4277f4")
                }

            };
            Chart1.Chart = new DonutChart { Entries = entries };

            timeSpentText.Text = "Time spent: " + _timeConverter.SecondsToHms(_thisWork.TimeSpent);
            durationTimeText.Text = "Duration: " + _timeConverter.SecondsToHms(_thisWork.Duration);
        }

        
        async void OnDeleteWork(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Removing work", "Are you sure?", "Yes", "No"))
            {
                await _connection.DeleteAsync(_thisWork);
                WorkDeleted?.Invoke(this, _thisWork);
                await Navigation.PopAsync();
            } 
        }
    }
}