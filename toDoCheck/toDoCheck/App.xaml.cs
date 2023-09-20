using System;
using toDoCheck.Data;
using toDoCheck.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace toDoCheck
{
    public partial class App : Application
    {
        public static DatabaseContext Context { get; set; }

        public App ()
        {
            InitializeComponent();
            InitializeDatabase();
            MainPage = new TaskTabs();
        }

        private void InitializeDatabase()
        {
            var folderApp = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = System.IO.Path.Combine(folderApp, "ToDoItems.db3");
            Context = new DatabaseContext(dbPath);
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }

        
    }
}

