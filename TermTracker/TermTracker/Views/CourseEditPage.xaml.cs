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

        private bool saved = false;

        public CourseEditPage(ModelDB _database, Course _course)
        {
            InitializeComponent();
            database = _database;
            course = _course;
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
            if (courseNameEditor.Text.Length > 0)
            {
                course.Name = courseNameEditor.Text;
                courseNameEditor.Text = "";
                courseTitleLabel.Text = course.Name;
            }

            course.StartDate = startDatePicker.Date;
            course.EndDate = endDatePicker.Date;

            // this is not done

            database.CourseManager.UpdateAsync(course).Wait();
            saved = true;
        }
    }
}