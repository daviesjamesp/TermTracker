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
    public partial class AssessmentEditPage : ContentPage
    {
        private readonly ModelDB database;
        private readonly Assessment assessment;
        private readonly bool isPerformance;

        public AssessmentEditPage(ModelDB _database, Assessment _assessment, bool _isPerformance)
        {
            InitializeComponent();
            database = _database;
            assessment = _assessment;
            isPerformance = _isPerformance;
        }
    }
}