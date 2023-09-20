using System;
using System.Collections.Generic;
using System.Linq;
using toDoCheck.Models;
using Xamarin.Forms;

namespace toDoCheck.Views
{	
	public partial class CompletedTasksPage : ContentPage
	{	
		public CompletedTasksPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            LoadCompletedItems();
            base.OnAppearing();
        }

        private async void LoadCompletedItems()
        {
            var items = await App.Context.GetItemsAsync();
            ToDoItemsCompleted.ItemsSource = items.Where(x => x.StatusCompleted == true); ;
        }

        async void CheckBox_TaskStatus_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            if (checkbox.BindingContext is ToDoItem item)
            {
                item.StatusCompleted = e.Value;
                var result = await App.Context.ModifyItemAsync(item);
                if (result != 1)
                {
                    await DisplayAlert("Error", "Could not update the Task' Status", "Accept");
                }
            }
        }
    }
}

