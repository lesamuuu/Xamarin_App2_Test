using System;
using System.Collections.Generic;
using System.Linq;
using toDoCheck.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace toDoCheck.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
	{
		public HomePage()
		{
            InitializeComponent();
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadItems();
        }

        private async void LoadItems()
        {
            var items = await App.Context.GetItemsAsync();
            ToDoItems.ItemsSource = items;
            ItemCount.Text = items.Count.ToString() + " Items";
        }

        private async void TaskDescription_Completed(object sender, EventArgs e)
        {

            try
            {
                var toToDoItem = new ToDoItem(TaskDescription.Text);


                var result = await App.Context.InsertItemAsync(toToDoItem);

                if (result == 1)
                {
                    LoadItems();
                    TaskDescription.Text = "";
                }
                else
                {
                    await DisplayAlert("Error", "Could not save the task", "Accept");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Accept");
            }
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
            List<ToDoItem> items = (List <ToDoItem>) ToDoItems.ItemsSource;
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
                    LoadItems();
                }
            }
        }
    }
}

