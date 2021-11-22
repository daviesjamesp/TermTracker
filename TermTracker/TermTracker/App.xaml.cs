using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TermTracker.Data;
using TermTracker.Models;
using System.IO;

namespace TermTracker
{
    public partial class App : Application
    {
        static ModelDB modelDB;

        public static ModelDB Database
        {
            get
            {
                if (modelDB is null)
                    modelDB = new ModelDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Notes.db3"));
                return modelDB;
            }
        }

        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new TermOverviewPage(Database));
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
