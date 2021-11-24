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

        public int GetCourseIDBySlot(int slot)
        {
            switch (slot)
            {
                case 0:
                    return Course0;
                case 1:
                    return Course1;
                case 2:
                    return Course2;
                case 3:
                    return Course3;
                case 4:
                    return Course4;
                case 5:
                    return Course5;
                default:
                    return -1;
            }
        }

        public void SetCourseIDBySlot(int slot, int courseID)
        {
            switch (slot)
            {
                case 0:
                    Course0 = courseID;
                    break;
                case 1:
                    Course1 = courseID;
                    break;
                case 2:
                    Course2 = courseID;
                    break;
                case 3:
                    Course3 = courseID;
                    break;
                case 4:
                    Course4 = courseID;
                    break;
                case 5:
                    Course5 = courseID;
                    break;
                default:
                    // do nothing
                    break;
            }
        }
    }
}
