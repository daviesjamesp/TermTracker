using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace TermTracker.Models
{
    public class Term
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Course0 { get; set; }
        public int Course1 { get; set; }
        public int Course2 { get; set; }
        public int Course3 { get; set; }
        public int Course4 { get; set; }
        public int Course5 { get; set; }
    }
}
