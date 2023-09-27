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
    public class ActiveTasksPageViewModel : INotifyPropertyChanged
    {
        public ICommand CheckBoxChangedCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;


        private List<ToDoItem> _toDoItemActive_ListView;
        public List<ToDoItem> ToDoItemActive_ListView // Binded
        {
            get { return _toDoItemActive_ListView; }
            set
            {
                _toDoItemActive_ListView = value;
                OnPropertyChanged(nameof(ToDoItemActive_ListView));
            }
        }

        private string _itemsCountActive_Label;
        public string ItemsCountActive_Label // Binded
        {
            get { return _itemsCountActive_Label; }
            set
            {
                _itemsCountActive_Label = value;
                OnPropertyChanged(nameof(ItemsCountActive_Label));
            }
        }


        // Methods
        public ActiveTasksPageViewModel()
        {
            CheckBoxChangedCommand = new Command<CheckBoxChangedEventArgs>(OnCheckBoxChanged);

            ItemsCountActive_Label = "PruebaConstructor";
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnAppearing()
        {
            LoadActiveItems();
        }

        private async void LoadActiveItems()
        {
            var items = await DependencyService.Get<ToDoItemDBService<ToDoItem>>().GetItemsAsync();
            ToDoItemActive_ListView = items.Where(x => x.StatusCompleted == false).ToList();
            ItemsCountActive_Label = ToDoItemActive_ListView.Count.ToString() + " Items";
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
        
    }
}

