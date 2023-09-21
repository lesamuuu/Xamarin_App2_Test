using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace toDoCheck.Services
{
    public class DialogService : IDialogService
	{
		public DialogService()
		{
		}

        public async Task DisplayCustomAlert(string title, string message, string cancel)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}

