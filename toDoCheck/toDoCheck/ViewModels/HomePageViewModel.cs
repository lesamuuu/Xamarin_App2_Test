using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using toDoCheck.Models;
using toDoCheck.Models.Converters.CheckBoxConverter;
using toDoCheck.Services;
using Xamarin.Forms;

namespace toDoCheck.ViewModels
{
	public class HomePageViewModel : INotifyPropertyChanged
	{

        public ICommand TaskDescriptionCompletedCommand { get; private set; }
        public ICommand TaskStatusChangedCommand { get; private set; }
        public ICommand ClearCompletedCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;


        private List<ToDoItem> _toDoItem_ListView;
        public List<ToDoItem> ToDoItem_ListView // Binded
        {
            get { return _toDoItem_ListView; }
            set
            {
                _toDoItem_ListView = value;
                OnPropertyChanged(nameof(ToDoItem_ListView));
            }
        }

        private string _itemCount_Label;
        public string ItemCount_Label // Binded
        {
            get { return _itemCount_Label; }
            set
            {
                _itemCount_Label = value;
                OnPropertyChanged(nameof(_itemCount_Label));
            }
        }

        private string _taskDescription;
        public string TaskDescription // Binded
        {
            get { return _taskDescription; }
            set
            {
                _taskDescription = value;
                OnPropertyChanged(nameof(TaskDescription));
            }
        }


        // Methods
        public HomePageViewModel()
		{
            TaskDescriptionCompletedCommand = new Command<string>(TaskDescription_Completed);
            TaskStatusChangedCommand = new Command<CheckBoxChangedData>(TaskStatusChanged);
            ClearCompletedCommand = new Command(ClearCompleted);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnAppearing()
        {
            LoadItems();
        }

        private async void LoadItems()
        {
            ToDoItem_ListView = await DependencyService.Get<ToDoItemDBService>().GetItemsAsync();
            
            ItemCount_Label = ToDoItem_ListView.Count.ToString() + " Items";
        }

        private async void TaskDescription_Completed(string description)
        {

            try
            {
                var toDoItem = new ToDoItem(description);

                var result = await DependencyService.Get<ToDoItemDBService>().InsertItemAsync(toDoItem);

                if (result == 1)
                {
                    LoadItems();
                    TaskDescription = "";
                }
                else
                {
                    await DependencyService.Get<DialogService>().DisplayCustomAlert("Error", "Could not save the task", "Accept");
                }
            }
            catch (Exception ex)
            {   
                await DependencyService.Get<DialogService>().DisplayCustomAlert("Error", ex.Message, "Accept");
            }
        }

        async void TaskStatusChanged(CheckBoxChangedData data)
        {

            //item.StatusCompleted = 

            ToDoItem item = data.Item;
            item.StatusCompleted = data.IsChecked;

            var result = await DependencyService.Get<ToDoItemDBService>().ModifyItemAsync(item);
            if (result != 1)
            {
                await DependencyService.Get<DialogService>().DisplayCustomAlert("Error", "Could not save the task", "Accept");
            }
        }


        async void ClearCompleted()
        {
            List<ToDoItem> items = ToDoItem_ListView;

            var checked_items = items.Where(x => x.StatusCompleted == true);

            bool reLoad = false;
            if (checked_items.Count() > 0)
            {
                // LINQ does not support await :(
                //checked_items.ForEach(x => App.Context.DeleteItemAsync(x));

                foreach (var item in checked_items)
                {
                    var deleteResult = await DependencyService.Get<ToDoItemDBService>().DeleteItemAsync(item);
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

