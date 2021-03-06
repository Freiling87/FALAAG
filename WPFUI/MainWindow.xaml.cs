using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using FALAAG.Models;
using FALAAG.Services;
using FALAAG.ViewModels;
using Microsoft.Win32;
using WPFUI.Windows;
using FALAAG.Core;

namespace WPFUI
{

	public partial class MainWindow : Window
    {
        #region Header
        private const string SAVE_GAME_FILE_EXTENSION = "falaag";
        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();
        private Point? _dragStart;
        private GameSession _gameSession;
        public MainWindow(Player player, int x = 0, int y = 0, int z = 0)
        {
            InitializeComponent();
            InitializeUserInputActions();
            SetActiveGameSessionTo(new GameSession(player, x, y, z));

            foreach (UIElement element in GameCanvas.Children)
            {
                if (element is Canvas)
                {
                    element.MouseDown += GameCanvas_OnMouseDown;
                    element.MouseMove += GameCanvas_OnMouseMove;
                    element.MouseUp += GameCanvas_OnMouseUp;
                }
            }
        }
        #endregion
        #region Input
        private void InitializeUserInputActions()
        {
            // To pass arguments with these action delegates, see https://soscsrpg.com/build-a-c-wpf-rpg/lesson-13-1-add-keyboard-input-for-actions-using-delegates/

            _userInputActions.Add(Key.X, () => _gameSession.AttackCurrentNPC());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));

            _userInputActions.Add(Key.I, () => _gameSession.InventoryDetails.IsVisible = !_gameSession.InventoryDetails.IsVisible);
            _userInputActions.Add(Key.J, () => _gameSession.JobDetails.IsVisible = !_gameSession.JobDetails.IsVisible);
            _userInputActions.Add(Key.P, () => _gameSession.PlayerDetails.IsVisible = !_gameSession.PlayerDetails.IsVisible);
            _userInputActions.Add(Key.R, () => _gameSession.RecipesDetails.IsVisible = !_gameSession.RecipesDetails.IsVisible);
        }
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
            {
                _userInputActions[e.Key].Invoke();
                e.Handled = true;
            }
        }
        private void CloseInventoryWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.InventoryDetails.IsVisible = false;
        }
        private void CloseJobsWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.JobDetails.IsVisible = false;
        }
        private void ClosePlayerDetailsWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.PlayerDetails.IsVisible = false;
        }
        private void CloseRecipesWindow_OnClick(object sender, RoutedEventArgs e)
        {
            _gameSession.RecipesDetails.IsVisible = false;
        }
        private void GameCanvas_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.Left)
                return;

            UIElement movingElement = (UIElement)sender;
            _dragStart = e.GetPosition(movingElement);
            movingElement.CaptureMouse();
            e.Handled = true;
        }
        private void GameCanvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_dragStart == null || e.LeftButton != MouseButtonState.Pressed)
                return;

            Point mousePosition = e.GetPosition(GameCanvas);
            UIElement movingElement = (UIElement)sender;

            if (mousePosition.X < _dragStart.Value.X ||
                mousePosition.Y < _dragStart.Value.Y ||
                mousePosition.X > GameCanvas.ActualWidth - ((Canvas)movingElement).ActualWidth + _dragStart.Value.X ||
                mousePosition.Y > GameCanvas.ActualHeight - ((Canvas)movingElement).ActualHeight + _dragStart.Value.Y)
                return;

            Canvas.SetLeft(movingElement, mousePosition.X - _dragStart.Value.X);
            Canvas.SetTop(movingElement, mousePosition.Y - _dragStart.Value.Y);
            e.Handled = true;
        }
        private void GameCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            var movingElement = (UIElement)sender;
            movingElement.ReleaseMouseCapture();
            _dragStart = null;
            e.Handled = true;
        }
        #endregion
        #region Game Data
        private void SetActiveGameSessionTo(GameSession gameSession)
        {
            _messageBroker.OnMessageRaised -= OnGameMessageRaised;

            _gameSession = gameSession;
            DataContext = _gameSession;

            // Clear out previous game's messages
            GameMessages.Document.Blocks.Clear();

            _messageBroker.OnMessageRaised += OnGameMessageRaised;
        }
        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            Startup startup = new Startup();
            startup.Show();
            Close();
        }
        private void SaveGame_OnClick(object sender, RoutedEventArgs e) =>
            SaveGame();
        private void Exit_OnClick(object sender, RoutedEventArgs e) =>
            Close();
        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            AskToSaveGame();
        }
        private void AskToSaveGame()
        {
            YesNoWindow message = new("Save Game", "Do you want to save your game?");
            message.Owner = GetWindow(this);
            message.ShowDialog();

            if (message.ClickedYes)
                SaveGame();
        }
        private void SaveGame()
        {
            SaveFileDialog saveFileDialog =
                new SaveFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
                };

            if (saveFileDialog.ShowDialog() == true)
            {
                SaveGameService.Save(new GameState(_gameSession.Player,
                    _gameSession.CurrentCell.X,
                    _gameSession.CurrentCell.Y,
                    _gameSession.CurrentCell.Z), saveFileDialog.FileName);
            }
        }
        #endregion
        #region Narration
        public void ClearMessageLog() =>
            GameMessages.Document.Blocks.Clear();
        private void OnGameMessageRaised(object sender, GameMessageEventArgs e)
        {
            GameMessages.Document.Blocks.Add(new Paragraph(new Run(e.Message)));
            GameMessages.ScrollToEnd();
        }
        #endregion
        #region Movement
        // Sender: the button itself, in this case
        // EventArgs: One or more objects of RoutedEventArgs type.
        private void OnClick_Ascend(object sender, RoutedEventArgs e) =>
            ButtonMovement(Direction.Above);
        private void OnClick_Descend(object sender, RoutedEventArgs e) =>
            ButtonMovement(Direction.Below);
        private void OnClick_MoveEast(object sender, RoutedEventArgs e) =>
            ButtonMovement(Direction.East);
        private void OnClick_MoveNorth(object sender, RoutedEventArgs e) =>
            ButtonMovement(Direction.North);
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e) =>
            ButtonMovement(Direction.South);
        private void OnClick_MoveWest(object sender, RoutedEventArgs e) =>
            ButtonMovement(Direction.West);
        private void ButtonMovement(Direction direction)
		{
            if (!_gameSession.HasNonEdgeNeighbor(direction))
                return;

            // TODO: I think a lot of this should be done within GameSession. Only put in here what's necessary.
            // Specifically, set ClearMessageLog 

            Cell origin = _gameSession.CurrentCell;
            Wall wall = origin.GetWall(direction);
            Cell destination = _gameSession.CurrentWorld.GetNeighbor(origin, direction);

            // TODO: Command pattern to store intended result action, to be gated behind ActionCommand attempt Command.
            //      This needs to be done before skill checks for other actions, to avoid redundant windows.

            if (wall != null && !wall.Passable)
			{
                List<ActionCommand> actionCommands = wall.ActionCommands;

                if (actionCommands.Count == 0)
                    return;
                else if (actionCommands.Count > 0)
                {
                    ActionCommand storedMovementAction = _gameSession.Player.MovementAction(direction);

                    foreach (ActionCommand ac in actionCommands)
                    {
                        _gameSession.CurrentActionCommands.Add(ac.CloneForInteraction(_gameSession.Player));
                    }

                    ActionWindow actionWindow = new(storedMovementAction);
                    actionWindow.Owner = GetWindow(this);
                    actionWindow.DataContext = _gameSession; // Attaches XAML
                    actionWindow.ShowDialog(); // actionWindow later calls CompleteMovement if check is passed
                    return;
                }
                else
                    throw new NotImplementedException();
            }

            CompleteMovement(direction);
        }
        private void CompleteMovement(Direction direction)
        {
            _gameSession.MoveDirection(direction);
            ClearMessageLog();
        }
        #endregion
        #region Action Rate
        private void OnClick_RateSlowest(object sender, RoutedEventArgs e) =>
            _gameSession.ActionRate = ActionRate.Slowest;
        private void OnClick_RateSlow(object sender, RoutedEventArgs e) =>
            _gameSession.ActionRate = ActionRate.Slow;
        private void OnClick_RateMedium(object sender, RoutedEventArgs e) =>
            _gameSession.ActionRate = ActionRate.Medium;
        private void OnClick_RateFast(object sender, RoutedEventArgs e) =>
            _gameSession.ActionRate = ActionRate.Fast;
        private void OnClick_RateFastest(object sender, RoutedEventArgs e) =>
            _gameSession.ActionRate = ActionRate.Fastest;
        #endregion
        #region Actions
        private void OnClick_AttackNPC(object sender, RoutedEventArgs e)
		{
            _gameSession.AttackCurrentNPC();
        }
        private void OnClick_Craft(object sender, RoutedEventArgs e)
        {
            Recipe recipe = ((FrameworkElement)sender).DataContext as Recipe;
            _gameSession.CraftItemUsing(recipe);
        }
        private void OnClick_UseCurrentConsumable(object sender, RoutedEventArgs e)
        {
            _gameSession.UseCurrentConsumable();
        }
        #endregion
        #region Submenus
        private void OnClick_DisplayTradeScreen(object sender, RoutedEventArgs e)
        {
            if (_gameSession.CurrentAutomat != null)
            {
                TradeScreen tradeScreen = new TradeScreen();
                tradeScreen.Owner = this;
                tradeScreen.DataContext = _gameSession; // Attaches XAML
                tradeScreen.ShowDialog(); // Show() = non-modal. ShowDialog() = modal
            }
        }
        #endregion
    }
}