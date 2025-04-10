using System.Windows.Input;

namespace PTANonCrown.Controls;

public partial class LabelWithInfoButton : ContentView
{
    public static readonly BindableProperty CommandParameterProperty =
    BindableProperty.Create(
        nameof(CommandParameter),
        typeof(object),
        typeof(LabelWithInfoButton),
        null);

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(
            nameof(Command),
            typeof(ICommand),
            typeof(LabelWithInfoButton),
            null);

    public static readonly BindableProperty LabelTextProperty =
        BindableProperty.Create(
            nameof(LabelText),
            typeof(string),
            typeof(LabelWithInfoButton),
            string.Empty);

    public LabelWithInfoButton()
    {
        InitializeComponent();
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    private void OnInfoClicked(object sender, EventArgs e)
    {
        if (Command?.CanExecute(CommandParameter) == true)
        {
            Command.Execute(CommandParameter);
        }
    }

}