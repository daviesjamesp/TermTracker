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
        public int InstructorID { get; private set; }
        public int PerformanceID { get; private set; }
        public int ObjectiveID { get; private set; }

        public void SetInstructor(Instructor instructor)
        {
            if (instructor is null)
                InstructorID = 0;
            else
                InstructorID = instructor.ID;

        }

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
