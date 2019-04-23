using App2.Db;
using App2.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorksPage : ContentPage
	{
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Work> _works;

        public WorksPage ()
		{
			InitializeComponent ();
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        protected override async void OnAppearing()
        {
            await LoadDataBase();
            OnLoadWorkList(new object(), new EventArgs());
            base.OnAppearing();
        }

        async Task LoadDataBase()
        {
            await _connection.CreateTableAsync<Work>();

            var works = await _connection.Table<Work>().ToListAsync();

            _works = new ObservableCollection<Work>(works);

        }

        void OnLoadWorkList(object sender, System.EventArgs e)
        {
            var selectedDayWorks = _works.Where(w => w.Date == workDate.Date);

            worksListView.ItemsSource = selectedDayWorks;
            worksToDo.Text = "Works left to do: " + selectedDayWorks.Where(w => w.isFinished == false).Count();
        }
        

        async void OnWorkSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (worksListView.SelectedItem == null)
                return;

            var selectedWork = e.SelectedItem as Work;

            worksListView.SelectedItem = null;
            var page = new WorkDetailPage(selectedWork);

            page.WorkDeleted += (source, work) =>
            {
                _works.Remove(work);
            };

            await Navigation.PushAsync(page);
        }

        //<ToolbarItem Text="DelAll" Clicked="OnDelWorks"/>
        //async void OnDelWorks(object sender, System.EventArgs e)
        //{
        //    if (await DisplayAlert("Removing all works", "Are you sure?", "Yes", "No"))
        //    {
        //        await _connection.DropTableAsync<Work>();
        //        OnAppearing();
        //    }
        //}


        async void OnAddWork(object sender, System.EventArgs e)
        {
            var page = new AddWork();

            page.WorkAdded += (source, work) =>
            {
                _works.Add(work);
            };

            await Navigation.PushAsync(page);
        }
    }
}