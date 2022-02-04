using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using FALAAG.Models.EventArgs;
using Engine.Models;
using Engine.Services;
using Engine.ViewModels;
using Microsoft.Win32;
using WPFUI.Windows;

namespace WPFUI
{
	public partial class MainWindow : Window
    {
        #region Header
        private const string SAVE_GAME_FILE_EXTENSION = "falaag";

        private readonly MessageBroker _messageBroker = MessageBroker.GetInstance();
        private readonly Dictionary<Key, Action> _userInputActions = new Dictionary<Key, Action>();

        private GameSession _gameSession;

        public MainWindow(Player player, int x = 0, int y = 0, int z = 0)
        {
            InitializeComponent();
            InitializeUserInputActions();
            SetActiveGameSessionTo(new GameSession(player, x, y, z));
        }
        #endregion
        #region Input
        private void InitializeUserInputActions()
        {
            // To pass arguments with these action delegates, see https://soscsrpg.com/build-a-c-wpf-rpg/lesson-13-1-add-keyboard-input-for-actions-using-delegates/
            _userInputActions.Add(Key.W, () => _gameSession.MoveNorth());
            _userInputActions.Add(Key.A, () => _gameSession.MoveWest());
            _userInputActions.Add(Key.S, () => _gameSession.MoveSouth());
            _userInputActions.Add(Key.D, () => _gameSession.MoveEast());
            _userInputActions.Add(Key.Q, () => _gameSession.Ascend());
            _userInputActions.Add(Key.Z, () => _gameSession.Descend());

            _userInputActions.Add(Key.X, () => _gameSession.AttackCurrentNPC());
            _userInputActions.Add(Key.C, () => _gameSession.UseCurrentConsumable());
            _userInputActions.Add(Key.T, () => OnClick_DisplayTradeScreen(this, new RoutedEventArgs()));

            _userInputActions.Add(Key.I, () => SetTabFocusTo("InventoryTabItem"));
            _userInputActions.Add(Key.J, () => SetTabFocusTo("JobsTabItem"));
            _userInputActions.Add(Key.R, () => SetTabFocusTo("RecipesTabItem")); 
        }
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputActions.ContainsKey(e.Key))
                _userInputActions[e.Key].Invoke();
        }
        private void SetTabFocusTo(string tabName)
        {
            foreach (object item in PlayerDataTabControl.Items)
                if (item is TabItem tabItem)
                    if (tabItem.Name == tabName)
                    {
                        tabItem.IsSelected = true;
                        return;
                    }
        }
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
            YesNoWindow message = new YesNoWindow("Save Game", "Do you want to save your game?");
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
				SaveGameService.Save(_gameSession, saveFileDialog.FileName);
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
		private void OnClick_MoveNorth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveNorth();
        }
        private void OnClick_MoveWest(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveWest();
        }
        private void OnClick_MoveEast(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveEast();
        }
        private void OnClick_MoveSouth(object sender, RoutedEventArgs e)
        {
            _gameSession.MoveSouth();
        }
        private void OnClick_Ascend(object sender, RoutedEventArgs e)
		{
            _gameSession.Ascend();
		}
        private void OnClick_Descend(object sender, RoutedEventArgs e)
		{
            _gameSession.Descend();
		}
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