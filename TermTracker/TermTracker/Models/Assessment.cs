using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace TermTracker.Models
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Notes { get; set; }
    }
}
