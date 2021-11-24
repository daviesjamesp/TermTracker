using System;
using SQLite;
using System.Collections.Generic;
using System.Text;

namespace TermTracker.Models
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string InstructorName { get; set; }
        public string InstructorEmail { get; set; }
        public string InstructorPhone { get; set; }
        public int PerformanceID { get; private set; }
        public int ObjectiveID { get; private set; }

        public void SetPerformance(Assessment assessment)
        {
            if (assessment is null)
                PerformanceID = 0;
            else
                PerformanceID = assessment.ID;
        }

        public void SetObjective(Assessment assessment)
        {
            if (assessment is null)
                ObjectiveID = 0;
            else
                ObjectiveID = assessment.ID;
        }
    }
}
