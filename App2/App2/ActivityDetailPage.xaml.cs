using App2.Db;
using App2.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Entry = Microcharts.Entry;
using Microcharts;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ActivityDetailPage : ContentPage
	{
        private Activity _thisActivity;
        private SQLiteAsyncConnection _connection;
        public event EventHandler<Activity> ActivityDeleted;
        private ObservableCollection<Work> _works;

        public ActivityDetailPage (Activity activity)
		{
			InitializeComponent ();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _thisActivity = activity;
            BindingContext = activity;
		}

        protected override async void OnAppearing()
        {
            await LoadDataBase();
            DetailPageLabels();
            LoadChart();
            base.OnAppearing();
        }

        async Task LoadDataBase()
        {
            await _connection.CreateTableAsync<Work>();

            var works = await _connection.Table<Work>().ToListAsync();

            _works = new ObservableCollection<Work>(works);

        }

        private void LoadChart()
        {
            var otherActivities = _works.Count() - _works.Where(w => w.ActivityName == _thisActivity.Name).Count();
            var thisActivity = _works.Where(w => w.ActivityName == _thisActivity.Name).Count();

            List <Entry> entries = new List<Entry>
            {
                new Entry(otherActivities)
                {
                    Label = "Other activities",
                    ValueLabel = otherActivities.ToString(),
                    Color = SKColor.Parse("#f44144")
                },
                new Entry(thisActivity)
                {
                    Label = "This activity",
                    ValueLabel = thisActivity.ToString(),
                    Color = SKColor.Parse("#4277f4")
                }
            };


            Chart1.Chart = new DonutChart { Entries = entries };

        }

        private void DetailPageLabels()
        {
            TimeConverter t = new TimeConverter();
            var totalTime = 0;
            var works = _works.Where(w => w.ActivityName == _thisActivity.Name);
            var unFinishedActivities = _works.Where(w => w.isFinished == false).Where(w=> w.ActivityName == _thisActivity.Name).Count();
            var finishedActivities = _works.Where(w => w.isFinished == true).Where(w => w.ActivityName == _thisActivity.Name).Count();
            foreach (var a in works)
            {
                totalTime += a.TimeSpent; 
            }
            totalTimeSpent.Text = "Total time spent: " + t.SecondsToHms(totalTime);
            unFinishedWorks.Text = "Unfinished works: " + unFinishedActivities;
            finishedWorks.Text = "Finished works: " + finishedActivities;
        }

        async void OnDeleteActivity(object sender, System.EventArgs e)
        {
            if (await DisplayAlert("Removing activity", "Are you sure?", "Yes", "No"))
            {
                await _connection.DeleteAsync(_thisActivity);
                ActivityDeleted?.Invoke(this, _thisActivity);
                await Navigation.PopAsync();
            }
        }
    }
}