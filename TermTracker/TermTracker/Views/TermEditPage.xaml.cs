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
        private bool saved = false;

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
            courseFrames = new List<Frame>() { c0Frame, c1Frame, c2Frame, c3Frame, c4Frame, c5Frame };
            TapSetup();
        }

        private void TapSetup()
        {
            for (int i = 0; i < 6; i++)
            {
                var targetFrame = FindByName($"c{i}Frame") as Frame;
                var newTGR = new TapGestureRecognizer();
                newTGR.Tapped += OpenCourseForEdit;
                targetFrame.GestureRecognizers.Add(newTGR);
            }
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
            }
        }

        private async void OpenCourseForEdit(object _, EventArgs e)
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
            if (courseToEdit is null)
            {       
                courseToEdit = new Course()
                {
                    Name = "NewCourse",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today
                };
                database.CourseManager.AddAsync(courseToEdit).Wait();
                term.SetCourseIDBySlot(courseSlot, courseToEdit.ID);
                database.TermManager.UpdateAsync(term).Wait();
            }
            await Navigation.PushAsync(new CourseEditPage(database, courseToEdit));
        }

        private Course GetCourseByTermSlot(int slot)
        {
            if (cachedCourseList is null)
            {
                var list_task = database.CourseManager.GetAllAsync();
                list_task.Wait();
                cachedCourseList = list_task.Result;
            }

            int targetCourseID = term.GetCourseIDBySlot(slot);

            if (targetCourseID < 1)
                return null;
            else
                return cachedCourseList.Where(c => c.ID == targetCourseID).FirstOrDefault();
        }

        private async void closeButton_Clicked(object sender, EventArgs e)
        {
            if (saved)
            {
                await Navigation.PopAsync();
            }
            else
            {
                var response = await DisplayAlert("", "Quit without saving?", "Yes", "No");
                if (response)
                {
                    await Navigation.PopAsync();
                }
            }
        }

        private void saveButton_Clicked(object sender, EventArgs e)
        {
            if (termNameEditor.Text.Length > 0)
            {
                term.Name = termNameEditor.Text;
                termNameEditor.Text = "";
                termTitleLabel.Text = term.Name;
            }

            term.StartDate = startDatePicker.Date;
            term.EndDate = endDatePicker.Date;

            database.TermManager.UpdateAsync(term).Wait();
            saved = true;
        }
    }
}