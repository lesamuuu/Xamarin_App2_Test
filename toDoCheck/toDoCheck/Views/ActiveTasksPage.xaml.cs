using toDoCheck.ViewModels;
using Xamarin.Forms;

namespace toDoCheck.Views
{	
	public partial class ActiveTasksPage : ContentPage
	{
        public ActiveTasksPage ()
		{
			InitializeComponent ();
            BindingContext = new ActiveTasksPageViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as ActiveTasksPageViewModel;
            viewModel?.OnAppearing();
        }
    }
}

