using toDoCheck.ViewModels;
using Xamarin.Forms;

namespace toDoCheck.Views
{	
	public partial class CompletedTasksPage : ContentPage
	{

        public CompletedTasksPage ()
		{
			InitializeComponent ();
            BindingContext = new CompletedTasksPageViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as CompletedTasksPageViewModel;
            viewModel?.OnAppearing();
        }
    }
}

