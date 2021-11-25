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
    public partial class AssessmentEditPage : ContentPage
    {
        private readonly ModelDB database;
        private readonly Assessment assessment;
        private readonly Course parent;
        private readonly bool isPerformance;

        public AssessmentEditPage(ModelDB _database, Course _parent, Assessment _assessment, bool _isPerformance)
        {
            InitializeComponent();
            database = _database;
            assessment = _assessment;
            isPerformance = _isPerformance;
            parent = _parent;
            TapSetup();
        }
        
        private void TapSetup()
        {
            var newTGR = new TapGestureRecognizer();
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

            assessment.Notes = notesEditor.Text;
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = notesEditor.Text,
                Title = "Share Notes"
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeFields();
        }

        private void InitializeFields()
        {
            asstTitleLabel.Text = (isPerformance ? "P - " : "O - ") + assessment.Name;

            notesEditor.Text = assessment.Notes;

            startDatePicker.Date = assessment.StartDate;
            endDatePicker.Date = assessment.EndDate;
        }

        private async void closeButton_Clicked(object _, EventArgs e)
        {
            var response = await DisplayAlert("", "Are you sure you want to remove this assessment?", "Yes", "No");
            if (response)
            {
                await database.AssessmentManager.DeleteAsync(assessment);
                if (isPerformance)
                    parent.PerformanceID = 0;
                else
                    parent.ObjectiveID = 0;
                await database.CourseManager.UpdateAsync(parent);
                await Navigation.PopAsync();
            }
        }

        private async void saveButton_Clicked(object _, EventArgs e)
        {
            if (asstNameEditor.Text != null && asstNameEditor.Text.Length > 0)
            {
                assessment.Name = asstNameEditor.Text;
                asstNameEditor.Text = "";
                asstTitleLabel.Text = assessment.Name;
            }

            if (notesEditor.Text != null)
                assessment.Notes = notesEditor.Text;

            assessment.StartDate = startDatePicker.Date;
            assessment.EndDate = endDatePicker.Date;

            await database.AssessmentManager.UpdateAsync(assessment);

            await Navigation.PopAsync();
        }
    }
}