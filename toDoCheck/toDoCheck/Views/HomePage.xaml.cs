using toDoCheck.ViewModels;
using Xamarin.Forms;

namespace toDoCheck.Views
{
    public partial class HomePage : ContentPage
	{
		public HomePage()
		{
            InitializeComponent();
            BindingContext = new HomePageViewModel();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as HomePageViewModel;
            viewModel?.OnAppearing();
        }
    }
}

