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
            DependencyService.Register<ToDoItemDBService>();
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

