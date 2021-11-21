using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace TermTracker.Models
{
    public class Instructor
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
