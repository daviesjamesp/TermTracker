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

        private Assessment objective;
        private Assessment performance;
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
            TapSetup();
        }

        private void TapSetup()
        {
            var newTGR = new TapGestureRecognizer();
            newTGR.Tapped += OpenAssessmentForEdit;
            perfFrame.GestureRecognizers.Add(newTGR);
            objFrame.GestureRecognizers.Add(newTGR);
        }

        private async void OpenAssessmentForEdit(object sender, EventArgs e)
        {
            var sendingFrame = sender as Frame;
            if (sendingFrame == FindByName("perfFrame") as Frame)
            {
                if (performance is null)
                {
                    performance = new Assessment()
                    {
                        Name = "NewPerformanceAsst",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                    };
                    await database.AssessmentManager.AddAsync(performance);
                }
                await Navigation.PushAsync(new AssessmentEditPage(database, performance, true));
            }
            else
            {
                if (objective is null)
                {
                    objective = new Assessment()
                    {
                        Name = "NewObjectiveAsst",
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                    };
                    await database.AssessmentManager.AddAsync(objective);
                }
                await Navigation.PushAsync(new AssessmentEditPage(database, objective, false));
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

            notesEditor.Text = course.Notes;
            
            instructorNameEditor.Text = course.InstructorName is null ? "" : course.InstructorName;
            instructorEmailEditor.Text = course.InstructorEmail is null ? "" : course.InstructorEmail;
            instructorPhoneEditor.Text = course.InstructorPhone is null ? "" : course.InstructorPhone;

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
                perfName.Text = p.Name;
                perfName.TextColor = Color.Black;
                performance = p;
            }
            else
            {
                perfName.Text = "Add Performance";
                performance = null;
            }

            if (course.ObjectiveID > 1)
            {
                var o = database.AssessmentManager.GetAt(course.ObjectiveID);
                objName.Text = o.Name;
                objName.TextColor = Color.Black;
                objective = o;
            }
            else
            {
                objName.Text = "Add Objective";
                objective = null;
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

            course.Status = statuses[statusPicker.SelectedIndex];

            course.Notes = notesEditor.Text;

            course.InstructorName = instructorNameEditor.Text;
            course.InstructorEmail = instructorEmailEditor.Text;
            course.InstructorPhone = instructorPhoneEditor.Text;

            await database.CourseManager.UpdateAsync(course);
            saved = true;
        }
        
    }
}