using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermTracker.Data;
using TermTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermEditPage : ContentPage
    {
        public bool Saved { get; set; }

        private readonly ModelDB database;
        private readonly Term term;

        private List<Course> cachedCourseList = null;
        private List<Frame> courseFrames = new List<Frame>();
        private List<Course> termCourses = new List<Course>();
        public TermEditPage(ModelDB _database, Term _term)
        {
            InitializeComponent();
            database = _database;
            term = _term;
            Saved = false;
            courseFrames = new List<Frame>() { c0Frame, c1Frame, c2Frame, c3Frame, c4Frame, c5Frame };
            // same as overview page, moved to onappearing
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeFields();
        }

        private void InitializeFields()
        {
            termCourses.Clear();
            termTitleLabel.Text = term.Name;
            startDatePicker.Date = term.StartDate != null ? term.StartDate : DateTime.Today;
            endDatePicker.Date =  term.EndDate != null ? term.EndDate : DateTime.Today;

            for (int i = 0; i < 6; i++)
            {
                var course = GetCourseByTermSlot(i);
                termCourses.Add(course);

                var targetFrame = FindByName($"c{i}Frame") as Frame;
                var targetName = FindByName($"c{i}Name") as Label;
                var targetDates = FindByName($"c{i}Dates") as Label;

                if (course is null)
                {
                    targetName.Text = "Add Course...";
                    targetDates.Text = "";
                }
                else
                {
                    targetName.Text = course.Name;
                    targetDates.Text = course.StartDate.ToString("d") + " - " + course.EndDate.ToString("d");
                }

                var newTGR = new TapGestureRecognizer();
                newTGR.Tapped += OpenCourseForEdit;
                targetFrame.GestureRecognizers.Add(newTGR);
            }
        }

        private void OpenCourseForEdit(object _, EventArgs e)
        {
            var sendingFrame = _ as Frame;
            int courseSlot = -1;
            for (int i = 0; i < courseFrames.Count; i++)
            {
                if (courseFrames[i] == sendingFrame)
                {
                    courseSlot = i;
                    break;
                }
            }

            var courseToEdit = GetCourseByTermSlot(courseSlot);
            
            // open course edit page here
        }

        private Course GetCourseByTermSlot(int slot)
        {
            if (cachedCourseList is null)
            {
                var list_task = database.CourseManager.GetAllAsync();
                list_task.Wait();
                cachedCourseList = list_task.Result;
            }

            int targetCourseID;
            switch (slot)
            {
                // Please forgive me for this abomination
                case 0:
                    targetCourseID = term.Course0;
                    break;
                case 1:
                    targetCourseID = term.Course1;
                    break;
                case 2:
                    targetCourseID = term.Course2;
                    break;
                case 3:
                    targetCourseID = term.Course3;
                    break;
                case 4:
                    targetCourseID = term.Course4;
                    break;
                case 5:
                    targetCourseID = term.Course5;
                    break;
                default:
                    throw new Exception("How could this possibly have happened?");
            }

            if (targetCourseID == 0)
                return null;
            else
                return cachedCourseList.Where(c => c.ID == targetCourseID).FirstOrDefault();
        }

        private void closeButton_Clicked(object sender, EventArgs e)
        {

        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {

        }
    }
}