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
        public ActionOption ResultAction { get; set; }

        public ActionWindow(ActionOption resultAction)
		{
            InitializeComponent();
            ResultAction = resultAction;
            // _ = GameSession.CurrentActionOptions.Count;
		}

        private void OnClick_AttemptSelectedAction(object sender, RoutedEventArgs e)
        {
            OnClick_SelectActionOption(sender, e);

            if (GameSession.Player.SucceedsAt(GameSession.SelectedAction))
                ResultAction = GameSession.SelectedAction;
            else
                ResultAction = null;

            Terminate();
        }
        private void OnClick_SelectActionOption(object sender, RoutedEventArgs e) =>
            GameSession.SelectedAction = ((FrameworkElement)sender).DataContext as ActionOption;
        private void OnClick_CancelActionChoice(object sender, RoutedEventArgs e) =>
            Terminate();
        private void Terminate()
        {
            if (GameSession?.SelectedAction is object)
                GameSession.SelectedAction = null;

            if (GameSession?.CurrentActionOptions is object)
                GameSession.CurrentActionOptions.Clear();

            Close();
        }
    }
}
