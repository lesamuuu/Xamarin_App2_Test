using System;
using System.Threading.Tasks;

namespace toDoCheck.Services
{
	public interface IDialogService
	{
		Task DisplayCustomAlert(string title, string message, string cancel);
	}
}

