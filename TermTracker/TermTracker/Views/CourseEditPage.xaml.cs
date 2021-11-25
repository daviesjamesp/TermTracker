using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermTracker.Data;
using TermTracker.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TermTracker
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseEditPage : ContentPage
    {
        private readonly ModelDB database;
        private readonly Term parent;
        private readonly int parentSlot;
        private readonly Course course;
        private string[] statuses = new string[] { "Not Started", "In Progress", "Completed" };

        private Assessment objective;
        private Assessment performance;

        public CourseEditPage(ModelDB _database, Term _parent, int _parentSlot, Course _course)
        {
            InitializeComponent();
            database = _database;
            parent = _parent;
            parentSlot = _parentSlot;
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

            newTGR = new TapGestureRecognizer();
            newTGR.Tapped += Share_Tapped;
            shareLabel.GestureRecognizers.Add(newTGR);
        }

        private async void Share_Tapped(object sender, EventArgs e)
        {
            if (notesEditor.Text is null)
            {
                await DisplayAlert("", "No notes to share!", "OK");
                return;
            }

            course.Notes = notesEditor.Text;
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = notesEditor.Text,
                Title = "Share Notes"
            });
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
                    course.PerformanceID = performance.ID;
                    await database.CourseManager.UpdateAsync(course);
                }
                await Navigation.PushAsync(new AssessmentEditPage(database, course, performance, true));
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
                    course.ObjectiveID = objective.ID;
                    await database.CourseManager.UpdateAsync(course);
                }
                await Navigation.PushAsync(new AssessmentEditPage(database, course, objective, false));
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

            if (course.PerformanceID > 0)
            {
                var p = GetAsstByID(course.PerformanceID);
                perfName.Text = p.Name;
                perfName.TextColor = Color.Black;
                performance = p;
            }
            else
            {
                perfName.Text = "Add Performance";
                performance = null;
            }

            if (course.ObjectiveID > 0)
            {
                var o = GetAsstByID(course.ObjectiveID);
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

        private Assessment GetAsstByID(int id)
        {
            var list_task = database.AssessmentManager.GetAllAsync();
            list_task.Wait();
            var asstList = list_task.Result;
            return asstList.Where(a => a.ID == id).FirstOrDefault();
        }

        private async void closeButton_Clicked(object sender, EventArgs e)
        {
            var response = await DisplayAlert("", "Are you sure you want to remove this course?", "Yes", "No");
            if (response)
            {
                await database.CourseManager.DeleteAsync(course);
                parent.SetCourseIDBySlot(parentSlot, 0);
                await database.TermManager.UpdateAsync(parent);
                await Navigation.PopAsync();
            }
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            if (instructorNameEditor.Text is null || instructorEmailEditor.Text is null || instructorPhoneEditor.Text is null || !instructorEmailEditor.Text.Contains("@"))
            {
                await DisplayAlert("", "Instructor fields are required.", "OK");
                return;
            }

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

            await Navigation.PopAsync();
        }
        
    }
}