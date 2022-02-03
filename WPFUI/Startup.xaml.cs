using System;
using System.Windows;
using Engine.Services;
using Engine.ViewModels;
using Microsoft.Win32;

namespace WPFUI
{
    public partial class Startup : Window
    {
        private const string SAVE_GAME_FILE_EXTENSION = "falaag";

        public Startup()
        {
            InitializeComponent();
            DataContext = GameDetailsService.ReadGameDetails();
        }

        private void LoadSavedGame_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog =
                new OpenFileDialog
                {
                    InitialDirectory = AppDomain.CurrentDomain.BaseDirectory,
                    Filter = $"Saved games (*.{SAVE_GAME_FILE_EXTENSION})|*.{SAVE_GAME_FILE_EXTENSION}"
                };

            if (openFileDialog.ShowDialog() == true)
            {
                GameSession gameSession =
                    SaveGameService.LoadLastSaveOrCreateNew(openFileDialog.FileName);

                MainWindow mainWindow =
                    new MainWindow(gameSession.Player,
                                   gameSession.CurrentCell.X,
                                   gameSession.CurrentCell.Y,
                                   gameSession.CurrentCell.Z);

                mainWindow.Show();
                Close();
            }
        }

        private void StartNewGame_OnClick(object sender, RoutedEventArgs e)
        {
            CharacterCreation characterCreationWindow = new CharacterCreation();
            characterCreationWindow.Show();
            Close();
        }

        private void Exit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}