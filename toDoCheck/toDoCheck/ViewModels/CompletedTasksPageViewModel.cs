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
	public class CompletedTasksPageViewModel : INotifyPropertyChanged
	{
        public ICommand CheckBoxChangedCommand { get; private set; }
        public ICommand ButtonClearCompletedCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private List<ToDoItem> _toDoItemsCompleted_ListView;
        public List<ToDoItem> ToDoItemsCompleted_ListView // Binded
        {
            get { return _toDoItemsCompleted_ListView; }
            set
            {
                _toDoItemsCompleted_ListView = value;
                OnPropertyChanged(nameof(ToDoItemsCompleted_ListView));
            }
        }

        private string _itemsCompletedCount_Label;
        public string ItemsCompletedCount_Label // Binded
        {
            get { return _itemsCompletedCount_Label; }
            set
            {
                _itemsCompletedCount_Label = value;
                OnPropertyChanged(nameof(ItemsCompletedCount_Label));
            }
        }

        // Methods
        public CompletedTasksPageViewModel()
        {
            CheckBoxChangedCommand = new Command<CheckBoxChangedEventArgs>(OnCheckBoxChanged);
            ButtonClearCompletedCommand = new Command(OnButtonClearCompletedCommand);
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnAppearing()
        {
            LoadCompletedItems();
        }

        private async void LoadCompletedItems()
        {
            var items = await DependencyService.Get<ToDoItemDBService>().GetItemsAsync();
            ToDoItemsCompleted_ListView = items.Where(x => x.StatusCompleted == true).ToList();
            ItemsCompletedCount_Label = ToDoItemsCompleted_ListView.Count.ToString() + " Items";
        }

        async void OnCheckBoxChanged(CheckBoxChangedEventArgs args)
        {
            ToDoItem item = args.Parameter as ToDoItem;
            item.StatusCompleted = args.IsChecked;

            var result = await DependencyService.Get<ToDoItemDBService>().ModifyItemAsync(item);
            if (result != 1)
            {
                await DependencyService.Get<DialogService>().DisplayCustomAlert("Error", "Could not save the task", "Accept");
            }
        }


        async void OnButtonClearCompletedCommand()
        {
            // Check the list is not empty
            if (ToDoItemsCompleted_ListView is null)
            {
                return;
            }

            List<ToDoItem> items = ToDoItemsCompleted_ListView;

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
                    LoadCompletedItems();
                }
            }
        }
    }
}

