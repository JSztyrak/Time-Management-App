using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace App2.Models
{
    public class Activity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public string Category { get; set; }

        public string ImgSrc { get; set; }

    }
}
