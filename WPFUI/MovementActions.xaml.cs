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
	/// <summary>
	/// Interaction logic for MovementActions.xaml
	/// </summary>
	public partial class MovementActions : Window
    {
        public GameSession Session => DataContext as GameSession;

        public MovementActions()
		{
			InitializeComponent();
		}

        private void OnClick_AttemptSelectedAction(object sender, RoutedEventArgs e)
        {
            Session.Player.Attempt(Session.SelectedAction);
            Close();
            ResetGameSession();
        }
        private void OnClick_SelectActionOption(object sender, RoutedEventArgs e)
        {
            Session.SelectedAction = ((FrameworkElement)sender).DataContext as ActionOption;
            // TODO: Bind TextBlock to update according to ActionOption text contents
        }
        private void OnClick_CancelActionChoice(object sender, RoutedEventArgs e)
        {
            Close();
            ResetGameSession();
        }
        private void ResetGameSession()
        {
            Session.SelectedAction = null;
            Session.CurrentActionOptions.Clear();
            Session.MovementActionScreenModal = false;
        }
    }
}
