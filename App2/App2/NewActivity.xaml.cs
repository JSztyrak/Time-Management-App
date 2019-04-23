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
    public partial class NewActivity : ContentPage
    {
        public event EventHandler<Activity> ActivityAdded;
        private SQLiteAsyncConnection _connection;
        public List<String> Categories = new List<string> { "Home", "Work", "Hobby", "Sport", "Education", "Family" };

        public NewActivity()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

            categoryList.ItemsSource = Categories;
        }

        async void OnSaveNewActivity(object sender, System.EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(activityName.Text) || categoryList.SelectedItem == null)
            {
                await DisplayAlert("Warning", "Fill the required fields", "OK");
                return;
            }

            var activity = new Activity
            {
                Name = activityName.Text,
                Category = categoryList.SelectedItem.ToString(),
                ImgSrc = categoryList.SelectedItem.ToString() + ".png"
            };

            await _connection.InsertAsync(activity);
            ActivityAdded?.Invoke(this, activity);
            await Navigation.PopAsync();
        }
    }
}