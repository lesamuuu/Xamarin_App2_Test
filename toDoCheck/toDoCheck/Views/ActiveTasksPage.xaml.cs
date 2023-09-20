using System;
using System.Collections.Generic;
using System.Linq;
using toDoCheck.Models;
using Xamarin.Forms;

namespace toDoCheck.Views
{	
	public partial class ActiveTasksPage : ContentPage
	{
     
		public ActiveTasksPage ()
		{
			InitializeComponent ();
		}

        protected override void OnAppearing()
        {
            LoadActiveItems();
            base.OnAppearing();
            
        }

        private async void LoadActiveItems()
        {
            var items = await App.Context.GetItemsAsync();
            ToDoItemsActive.ItemsSource = items.Where(x => x.StatusCompleted == false);
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

