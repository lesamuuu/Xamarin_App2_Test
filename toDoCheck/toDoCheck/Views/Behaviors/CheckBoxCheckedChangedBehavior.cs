using System;
using System.Windows.Input;
using toDoCheck.Models;
using Xamarin.Forms;

namespace toDoCheck.Views.Behaviors
{
    public class CheckBoxCheckedChangedBehavior : Behavior<CheckBox>
    {
        public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CheckBoxCheckedChangedBehavior), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        public static readonly BindableProperty CommandParameterProperty =
            BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckBoxCheckedChangedBehavior), null);

        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnAttachedTo(CheckBox bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.CheckedChanged += OnCheckBoxCheckedChanged;
        }

        protected override void OnDetachingFrom(CheckBox bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.CheckedChanged -= OnCheckBoxCheckedChanged;
        }

        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            var checkbox = (CheckBox)sender;

            if (checkbox.BindingContext is ToDoItem &&
                Command != null &&
                Command.CanExecute(CommandParameter))
            {
                //Command.Execute(new CheckBoxChangedEventArgs(e.Value, CommandParameter));
                Command.Execute(new CheckBoxChangedEventArgs(e.Value, checkbox.BindingContext));
            }
        }
    }

    public class CheckBoxChangedEventArgs
    {
        public bool IsChecked { get; }
        public object Parameter { get; }

        public CheckBoxChangedEventArgs(bool isChecked, object parameter)
        {
            IsChecked = isChecked;
            Parameter = parameter;
        }
    }
}