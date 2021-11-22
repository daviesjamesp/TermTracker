using System;
using SQLite;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TermTracker.Models;

namespace TermTracker.Data
{
    public class ModelDB
    {
        readonly SQLiteAsyncConnection dbconnection;

        public DBObjectManager<Instructor> InstructorManager { get; }
        public DBObjectManager<Assessment> AssessmentManager { get; }
        public DBObjectManager<Course> CourseManager { get; }
        public DBObjectManager<Term> TermManager { get; }

        public ModelDB(string dbPath)
        {
            dbconnection = new SQLiteAsyncConnection(dbPath);

            InstructorManager = new DBObjectManager<Instructor>(dbconnection);
            AssessmentManager = new DBObjectManager<Assessment>(dbconnection);
            CourseManager = new DBObjectManager<Course>(dbconnection);
            TermManager = new DBObjectManager<Term>(dbconnection);
        }

    }
}
