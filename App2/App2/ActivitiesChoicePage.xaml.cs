using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;
using App2.Models;
using App2.Db;
using System.Collections.ObjectModel;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivitiesChoicePage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Activity> _activities;
        public event EventHandler<Activity> ActivitySelected;
        private bool _isDataLoaded;

        public ActivitiesChoicePage()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;

            await LoadDataBase();

            base.OnAppearing();
        }

        async Task LoadDataBase()
        {
            await _connection.CreateTableAsync<Activity>();

            var activities = await _connection.Table<Activity>().ToListAsync();

            _activities = new ObservableCollection<Activity>(activities);
            activitiesListView.ItemsSource = _activities;
        }

        async void OnSelectActivity(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (activitiesListView.SelectedItem == null)
                return;

            var selectedActivity = e.SelectedItem as Activity;

            activitiesListView.SelectedItem = null;
            ActivitySelected?.Invoke(this, selectedActivity);

            await Navigation.PopAsync();
        }

    }
}