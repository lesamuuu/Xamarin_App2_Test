using System;
using toDoCheck.Models;
using toDoCheck.Repositories;
using toDoCheck.Services;
using toDoCheck.Views;
using Xamarin.Forms;

namespace toDoCheck
{
    public partial class App : Application
    {

        public App ()
        {
            InitializeComponent();

            // Set up Repository dependency
            var dbPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ToDoItems.db3");
            var repository = new SQLiteRepository<ToDoItem>(dbPath);
            var toDoItemDBService = new ToDoItemDBService<ToDoItem>(repository);

            // Repository dependency
            DependencyService.RegisterSingleton(toDoItemDBService);


            DependencyService.Register<ToDoItemDBService<ToDoItem>>();
            MainPage = new TaskTabs();
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

