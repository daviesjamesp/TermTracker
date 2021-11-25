using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TermTracker.Data;
using TermTracker.Models;

namespace TermTracker
{
    public partial class TermOverviewPage : ContentPage
    {
        private readonly ModelDB database;

        private List<Frame> termFrames = new List<Frame>();
        private bool notificationsShown = false;

        public TermOverviewPage(ModelDB _database)
        {
            InitializeComponent();
            database = _database;
            AddTapSetup();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            ShowNotifications();
            if (database.TermManager.Count == 0)
                await AddTestData();
            PopulateTerms();
        }

        private async Task<int> AddTestData()
        {
            var testObj = new Assessment()
            {
                Name = "Ex. Obj",
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today,
                Notes = "Obj assessment notes"
            };
            await database.AssessmentManager.AddAsync(testObj);

            var testPerf = new Assessment()
            {
                Name = "Ex. Perf",
                StartDate = DateTime.Today.AddDays(-2),
                EndDate = DateTime.Today.AddDays(1),
                Notes = "Perf assessment notes"
            };
            await database.AssessmentManager.AddAsync(testPerf);

            var testCourse = new Course()
            {
                Name = "Example Course",
                StartDate = DateTime.Today.AddDays(-3),
                EndDate = DateTime.Today.AddDays(3),
                Notes = "Ex. course notes",
                Status = "In Progress",
                InstructorName = "James Davies",
                InstructorEmail = "jda1553@wgu.edu",
                InstructorPhone = "3602712741",
                ObjectiveID = testObj.ID,
                PerformanceID = testPerf.ID
            };
            await database.CourseManager.AddAsync(testCourse);

            var testTerm = new Term()
            {
                Name = "Example Term",
                StartDate = DateTime.Today.AddDays(-5),
                EndDate = DateTime.Today.AddDays(5)
            };
            testTerm.SetCourseIDBySlot(0, testCourse.ID);
            
            return await database.TermManager.AddAsync(testTerm);
        }

        private async void ShowNotifications()
        {
            if (notificationsShown) return;

            var termList_task = database.TermManager.GetAllAsync();
            var courseList_task = database.CourseManager.GetAllAsync();
            var asstList_task = database.AssessmentManager.GetAllAsync();

            termList_task.Wait();
            courseList_task.Wait();
            asstList_task.Wait();

            var termList = termList_task.Result;
            var courseList = courseList_task.Result;
            var asstList = asstList_task.Result;

            foreach (var t in termList)
            {
                if (t.StartDate == DateTime.Today)
                    await DisplayAlert("", $"Term {t.Name} starts today.", "OK");
                if (t.EndDate == DateTime.Today)
                    await DisplayAlert("", $"Term {t.Name} ends today.", "OK");
            }

            foreach (var c in courseList)
            {
                if (c.StartDate == DateTime.Today)
                    await DisplayAlert("", $"Course {c.Name} starts today.", "OK");
                if (c.EndDate == DateTime.Today)
                    await DisplayAlert("", $"Course {c.Name} ends today.", "OK");
            }

            foreach (var a in asstList)
            {
                if (a.StartDate == DateTime.Today)
                    await DisplayAlert("", $"Assessment {a.Name} starts today.", "OK");
                if (a.EndDate == DateTime.Today)
                    await DisplayAlert("",$"Assessment {a.Name} is due today.","OK");
            }

            notificationsShown = true;
        }

        private void AddTapSetup()
        {
            var addTermLabel_tap = new TapGestureRecognizer();
            addTermLabel_tap.Tapped += AddTermLabel_Tapped;
            addTermLabel.GestureRecognizers.Add(addTermLabel_tap);
        }

        private void AddTermLabel_Tapped(object _, EventArgs e)
        {
            var newTerm = new Term()
            {
                Name = "New Term",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now
            };
            database.TermManager.AddAsync(newTerm).Wait();
            OpenTermPage(newTerm);
        }

        private void Term_Tapped(object _, EventArgs e)
        {
            var sendingFrame = _ as Frame;
            int termIndex = -1;
            for (int i = 0; i < termFrames.Count; i++)
            {
                if (termFrames[i] == sendingFrame)
                {
                    termIndex = i;
                    break;
                }
            }

            var termToEdit = database.TermManager.GetAt(termIndex);
            OpenTermPage(termToEdit);
        }

        async void OpenTermPage(Term term)
        {
            await Navigation.PushAsync(new TermEditPage(database, term));
        }

        private async void PopulateTerms()
        {
            termListStack.Children.Clear();
            termFrames.Clear();
            var terms = await database.TermManager.GetAllAsync();
            if (terms.Count == 0)
            {
                termListStack.Children.Add(new Label
                {
                    Text = "No terms have been added.",
                    TextColor = Color.DarkGray
                });
            } else
            {
                for (int i = 0; i < terms.Count; i++)
                {
                    var target = terms[i];
                    
                    var nameLabel = new Label()
                    {
                        Text = target.Name,
                        HorizontalOptions = LayoutOptions.Start
                    };
                    Grid.SetColumn(nameLabel, 0);

                    var dateLabel = new Label
                    {
                        Text = target.StartDate.ToString("M-d-yy") + " - " + target.EndDate.ToString("M-d-yy"),
                        HorizontalOptions = LayoutOptions.Center
                    };
                    Grid.SetColumn(dateLabel, 1);

                    var arrowsLabel = new Label
                    {
                        Text = ">>",
                        HorizontalOptions = LayoutOptions.End
                    };
                    Grid.SetColumn(arrowsLabel, 2);

                    var newTermFrame = new Frame
                    {
                        BackgroundColor = Color.LightGray,
                        Padding = 3,
                        CornerRadius = 3,
                        Content = new Grid
                        {
                            ColumnDefinitions =
                            {
                                new ColumnDefinition(),
                                new ColumnDefinition() { Width = GridLength.Auto },
                                new ColumnDefinition()
                            },
                            Children =
                            {
                                nameLabel,
                                dateLabel,
                                arrowsLabel
                            }
                        }
                    };
                    termListStack.Children.Add(newTermFrame);
                    var newTGR = new TapGestureRecognizer();
                    newTGR.Tapped += Term_Tapped;
                    newTermFrame.GestureRecognizers.Add(newTGR);

                    termFrames.Add(newTermFrame);
                }
            }
        }
    }
}
