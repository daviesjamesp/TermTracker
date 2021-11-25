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
        public static NavigationPage navigationPage;

        public static ModelDB Database
        {
            get
            {
                if (modelDB is null)
                    modelDB = new ModelDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "termtracker_notes.db3"));
                return modelDB;
            }
        }

        public App()
        {
            InitializeComponent();

            navigationPage = new NavigationPage(new TermOverviewPage(Database));
            MainPage = navigationPage;

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
