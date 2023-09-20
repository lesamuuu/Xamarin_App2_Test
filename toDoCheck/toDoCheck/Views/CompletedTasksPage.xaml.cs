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
            base.OnAppearing();
            LoadCompletedItems();
        }

        private async void LoadCompletedItems()
        {
            var items = await App.Context.GetItemsAsync();
            ToDoItemsCompleted.ItemsSource = items.Where(x => x.StatusCompleted == true).ToList();
            ItemCountCompleted.Text = items.Count(x => x.StatusCompleted == true).ToString() + " Items";

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

        async void Button_ClearCompleted_Clicked(object sender, EventArgs e)
        {
            List<ToDoItem> items = (List <ToDoItem>) ToDoItemsCompleted.ItemsSource;
            var checked_items = items.Where(x => x.StatusCompleted == true);

            bool reLoad = false;
            if (checked_items.Count() > 0)
            {
                // LINQ does not support await :(
                //checked_items.ForEach(x => App.Context.DeleteItemAsync(x));

                foreach (var item in checked_items)
                {
                    var deleteResult = await App.Context.DeleteItemAsync(item);
                    if (deleteResult == 1)
                    {
                        // If at least one register is deleted, reload the page
                        reLoad = true;
                    }
                }
                if (reLoad)
                {
                    LoadCompletedItems();
                }
            }
        }
    }
}

