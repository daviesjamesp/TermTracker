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
        private readonly ModelDB database;
        private readonly Term term;

        public TermEditPage(ModelDB _database, Term _term)
        {
            InitializeComponent();
            database = _database;
            term = _term;
            InitializeFields();
        }

        private void InitializeFields()
        {
            DisplayAlert("", "fields initiate here", "ok");
        }
    }
}