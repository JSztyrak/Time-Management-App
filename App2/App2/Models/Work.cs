using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace App2.Models
{
    public class Work : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string ActivityName { get; set; }

        public string Comment { get; set; }

        public string ImgSrc { get; set; }

        public int Duration { get; set; }

        private int _timeSpent;
        public int TimeSpent
        {
            get { return _timeSpent; }
            set
            {
                _timeSpent = value;
                OnPropertyChanged();
            }
        }

        private DateTime _date;
        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
            }
        }

        public bool isFinished { get; set; }

        private double _progress { get; set; }

        public double Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Progression));
            }
        }

        public string Progression
        {
            get { return $"Progress: {Progress} %"; }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
