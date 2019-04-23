using App2.Db;
using App2.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TimerPage : ContentPage
	{
        private SQLiteAsyncConnection _connection;
        private Work thisWork;
        private bool isStopped = true;
        private TimeConverter _timeConverter;
        public TimerPage()
        {
            InitializeComponent();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

        }
        public TimerPage (Work work)
		{
			InitializeComponent ();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _timeConverter = new TimeConverter();
            BindingContext = work;
            thisWork = work;
            timertxt.Text = _timeConverter.SecondsToHms(thisWork.TimeSpent);
            workDuration.Text = _timeConverter.SecondsToHms(thisWork.Duration);
        }

        private void Timer(int duration, int timeSpent)
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                if (isStopped)
                    return false;

                timeSpent += 1;
                timertxt.Text = _timeConverter.SecondsToHms(timeSpent);
                UpdateData(timeSpent, duration);

                if (timeSpent == duration)
                {
                    DisplayAlert("DONE", "You finished your job successfully", "Ok");
                    isStopped = true;
                    timerButton.Text = "Finished";
                    return false;
                }

                return true;
            });
        }

        private void OnStartPauseTimer(object senser, EventArgs e)
        {
            if (thisWork.isFinished)
                return;

            if(isStopped == true)
            {
                timerButton.Text = "Stop";
                isStopped = false;
                int duration = thisWork.Duration;
                int timeSpent = thisWork.TimeSpent;
                Timer(duration, timeSpent);
            }
            else
            {
                isStopped = true;
                timerButton.Text = "Start";
            }
        }


        async void UpdateData(int timeSpent, int duration)
        {
            if (timeSpent == duration)
            {
                thisWork.isFinished = true;
                thisWork.ImgSrc = "Finished.png";
            }

            thisWork.TimeSpent = timeSpent;
            thisWork.Progress = Math.Round(((double)timeSpent / duration) * 100, 1);
            await _connection.UpdateAsync(thisWork);
        }


        async void OnGoBackButton(object sender, System.EventArgs e)
        {
            if (isStopped == false)
            {
                await DisplayAlert("STOP", "You have running work. Stop it first.", "Ok");
            }
            else
            {
                await Navigation.PopModalAsync();
            }
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}