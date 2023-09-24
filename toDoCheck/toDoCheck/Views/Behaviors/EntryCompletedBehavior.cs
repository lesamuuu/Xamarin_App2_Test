using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace toDoCheck.Views.Behaviors
{
    public class EntryCompletedBehavior : Behavior<Entry>
	{
        public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EntryCompletedBehavior), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.Completed += OnEntryCompleted;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.Completed -= OnEntryCompleted;
        }

        private void OnEntryCompleted(object sender, EventArgs e)
        {
            if (Command != null && Command.CanExecute(null))
            {
                Command.Execute(null);
            }
        }
    }
}

