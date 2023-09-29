using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using toDoCheck.Models;
using toDoCheck.Services;
using toDoCheck.Views.Behaviors;
using Xamarin.Forms;

namespace toDoCheck.ViewModels
{
	public class HomePageViewModel : INotifyPropertyChanged
	{

        public ICommand EntryCompletedCommand { get; private set; }
        public ICommand CheckBoxChangedCommand { get; private set; }
        public ICommand ButtonClearCompletedCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;


        private List<ToDoItem> _toDoItems_ListView;
        public List<ToDoItem> ToDoItems_ListView // Binded
        {
            get { return _toDoItems_ListView; }
            set
            {
                _toDoItems_ListView = value;
                OnPropertyChanged(nameof(ToDoItems_ListView));
            }
        }

        private string _itemsCount_Label;
        public string ItemsCount_Label // Binded
        {
            get { return _itemsCount_Label; }
            set
            {
                _itemsCount_Label = value;
                OnPropertyChanged(nameof(ItemsCount_Label));
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

        private string _text_SearchBar;
        public string Text_SearchBar // Binded
        {
            get { return _text_SearchBar; }
            set
            {
                _text_SearchBar = value;
                OnPropertyChanged(nameof(Text_SearchBar));

                SearchItem();
            }
        }


        // Methods
        public HomePageViewModel()
		{
            EntryCompletedCommand = new Command(OnEntryCompleted);
            CheckBoxChangedCommand = new Command<CheckBoxChangedEventArgs>(OnCheckBoxChanged);
            ButtonClearCompletedCommand = new Command(OnButtonClearCompletedCommand);
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
            ToDoItems_ListView = await DependencyService.Get<ToDoItemDBService<ToDoItem>>().GetItemsAsync();

            ItemsCount_Label = ToDoItems_ListView.Count.ToString() + " Items";
        }

        private async void OnEntryCompleted()
        {

            try
            {
                // Check the entry is not empty
                if (TaskDescription is null)
                {
                    return;
                }
                var toDoItem = new ToDoItem(TaskDescription);

                var result = await DependencyService.Get<ToDoItemDBService<ToDoItem>>().InsertItemAsync(toDoItem);

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



        async void OnCheckBoxChanged(CheckBoxChangedEventArgs args)
        {
            ToDoItem item = args.Parameter as ToDoItem;
            item.StatusCompleted = args.IsChecked;

            var result = await DependencyService.Get<ToDoItemDBService<ToDoItem>>().UpdateItemAsync(item);
            if (result != 1)
            {
                await DependencyService.Get<DialogService>().DisplayCustomAlert("Error", "Could not save the task", "Accept");
            }
        }

        async void SearchItem()
        {
            ToDoItems_ListView = await DependencyService.Get<ToDoItemDBService<ToDoItem>>().Search(Text_SearchBar);
        }

        async void OnButtonClearCompletedCommand()
        {
            // Check the list is not empty
            if (ToDoItems_ListView is null)
            {
                return;
            }

            List<ToDoItem> items = ToDoItems_ListView;

            var checked_items = items.Where(x => x.StatusCompleted == true);

            bool reLoad = false;
            if (checked_items.Count() > 0)
            {
                // LINQ does not support await :(
                //checked_items.ForEach(x => App.Context.DeleteItemAsync(x));

                foreach (var item in checked_items)
                {
                    var deleteResult = await DependencyService.Get<ToDoItemDBService<ToDoItem>>().DeleteItemAsync(item);
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