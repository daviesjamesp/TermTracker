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
    public partial class CourseEditPage : ContentPage
    {
        private readonly ModelDB database;
        private readonly Course course;
        private string[] statuses = new string[] { "Not Started", "In Progress", "Completed" };

        private bool saved = false;

        public CourseEditPage(ModelDB _database, Course _course)
        {
            InitializeComponent();
            database = _database;
            course = _course;
            foreach (var s in statuses)
            {
                statusPicker.Items.Add(s);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeFields();
        }

        private void InitializeFields()
        {
            courseTitleLabel.Text = course.Name;
            startDatePicker.Date = course.StartDate;
            endDatePicker.Date = course.EndDate;

            if (course.InstructorID > 1)
            {
                var i = database.InstructorManager.GetAt(course.InstructorID);
                instructorNameEditor.Text = i.Name is null ? "" : i.Name;
                instructorEmailEditor.Text = i.Email is null ? "" : i.Email;
                instructorPhoneEditor.Text = i.Phone is null ? "" : i.Phone;
            }

            if (course.Status == "Completed")
            {
                statusPicker.SelectedIndex = 2;
            }
            else if (course.Status == "In Progress")
            {
                statusPicker.SelectedIndex = 1;
            }
            else
            {
                statusPicker.SelectedIndex = 0;
            }

            if (course.PerformanceID > 1)
            {
                var p = database.AssessmentManager.GetAt(course.PerformanceID);
                // finish initialization
            }
            else
            {
                // no assessment added
            }

            if (course.ObjectiveID > 1)
            {
                var o = database.AssessmentManager.GetAt(course.ObjectiveID);
                // finish initialization
            }
            else
            {
                // no assessment added
            }

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

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (courseNameEditor.Text != null && courseNameEditor.Text.Length > 0)
            {
                course.Name = courseNameEditor.Text;
                courseNameEditor.Text = "";
                courseTitleLabel.Text = course.Name;
            }

            course.StartDate = startDatePicker.Date;
            course.EndDate = endDatePicker.Date;

            // this is not done

            await database.CourseManager.UpdateAsync(course);
            saved = true;
        }
    }
}