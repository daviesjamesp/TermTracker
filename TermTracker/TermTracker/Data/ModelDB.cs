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

        readonly DBObjectManager<Instructor> instructorManager;
        readonly DBObjectManager<Assessment> assessmentManager;
        readonly DBObjectManager<Course> courseManager;
        readonly DBObjectManager<Term> termManager;

        public ModelDB(string dbPath)
        {
            dbconnection = new SQLiteAsyncConnection(dbPath);

            instructorManager = new DBObjectManager<Instructor>(dbconnection);
            assessmentManager = new DBObjectManager<Assessment>(dbconnection);
            courseManager = new DBObjectManager<Course>(dbconnection);
            termManager = new DBObjectManager<Term>(dbconnection);
        }

    }
}
