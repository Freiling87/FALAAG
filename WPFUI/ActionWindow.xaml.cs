using FALAAG.Models;
using FALAAG.ViewModels;
using System.Windows;

namespace WPFUI
{
	/// <summary>
	/// Interaction logic for MovementActions.xaml
	/// TODO: Probably need an ActionManager (without AP for now) sooner rather than later, since you already need to queue them.
	/// </summary>
	public partial class ActionWindow : Window
    {
        public GameSession GameSession => DataContext as GameSession;
        public string Message { get; set; }
        public ActionCommand ResultAction { get; set; }

        public ActionWindow(ActionCommand resultAction)
		{
            InitializeComponent();
            ResultAction = resultAction;
            // _ = GameSession.CurrentActionCommands.Count;
		}

        private void OnClick_AttemptSelectedAction(object sender, RoutedEventArgs e)
        {
            if (GameSession.Player.SucceedsAt(GameSession.SelectedAction))
                ResultAction = GameSession.SelectedAction;
            else
                ResultAction = null;

            Terminate();
        }
        private void OnClick_SelectActionCommand(object sender, RoutedEventArgs e) =>
            GameSession.SelectedAction = ((FrameworkElement)sender).DataContext as ActionCommand;
        private void OnClick_CancelActionChoice(object sender, RoutedEventArgs e) =>
            Terminate();
        private void Terminate()
        {
            if (GameSession?.SelectedAction is object)
                GameSession.SelectedAction = null;

            if (GameSession?.CurrentActionCommands is object)
                GameSession.CurrentActionCommands.Clear();

            Close();
        }
    }
}
