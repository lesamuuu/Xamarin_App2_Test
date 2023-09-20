using System;
using System.Collections.Generic;
using toDoCheck.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace toDoCheck.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
	{
		public int ItemCount { get; set; }

		public HomePage()
		{
            InitializeComponent();
		}

        protected override void OnAppearing()
        {
            LoadItems();
            base.OnAppearing();
            
        }

        private async void LoadItems()
        {
            var items = await App.Context.GetItemsAsync();
            ToDoItems.ItemsSource = items;
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
    }
}

