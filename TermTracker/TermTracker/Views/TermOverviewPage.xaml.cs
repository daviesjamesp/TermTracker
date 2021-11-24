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

        public TermOverviewPage(ModelDB _database)
        {
            InitializeComponent();
            database = _database;
            AddTapSetup();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulateTerms();
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
