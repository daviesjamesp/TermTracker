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

        public TermOverviewPage(ModelDB _database)
        {
            InitializeComponent();
            database = _database;
        }
    }
}
