using System;
using Microsoft.Maui.Controls;

namespace MauiAppJogoDaVelha
{
    public partial class MainPage : ContentPage
    {
        private string currentPlayer = "X";
        private bool gameOver = false;

        public MainPage()
        {
            InitializeComponent();
            UpdateTurnLabel();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (gameOver) return;

            var button = (Button)sender;

            if (!string.IsNullOrEmpty(button.Text))
                return;

            button.Text = currentPlayer;

            if (CheckVictory())
            {
                gameOver = true;
                DisplayAlert("Vitória!", $"Jogador {currentPlayer} venceu!", "OK");
                HighlightWinningCombination();
            }
            else if (IsBoardFull())
            {
                gameOver = true;
                DisplayAlert("Empate", "Ninguém venceu!", "OK");
            }
            else
            {
                currentPlayer = (currentPlayer == "X") ? "O" : "X";
                UpdateTurnLabel();
            }
        }

        private void Reiniciar_Clicked(object sender, EventArgs e)
        {
            foreach (var view in GameGrid.Children)
            {
                if (view is Button btn)
                {
                    btn.Text = "";
                    btn.BackgroundColor = Colors.White;
                }
            }

            currentPlayer = "X";
            gameOver = false;
            UpdateTurnLabel();
        }

        private void UpdateTurnLabel()
        {
            lblVez.Text = $"Vez do jogador: {currentPlayer}";
        }

        private bool CheckVictory()
        {
            string[,] board = new string[3, 3];

            foreach (var view in GameGrid.Children)
            {
                if (view is Button btn)
                {
                    int row = Grid.GetRow(btn);
                    int col = Grid.GetColumn(btn);
                    board[row, col] = btn.Text;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(board[i, 0]) &&
                    board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
                    return true;

                if (!string.IsNullOrEmpty(board[0, i]) &&
                    board[0, i] == board[1, i] && board[1, i] == board[2, i])
                    return true;
            }

            if (!string.IsNullOrEmpty(board[0, 0]) &&
                board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
                return true;

            if (!string.IsNullOrEmpty(board[0, 2]) &&
                board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0])
                return true;

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (var view in GameGrid.Children)
            {
                if (view is Button btn && string.IsNullOrEmpty(btn.Text))
                    return false;
            }

            return true;
        }

        private void HighlightWinningCombination()
        {
            foreach (var view in GameGrid.Children)
            {
                if (view is Button btn && btn.Text == currentPlayer)
                {
                    btn.BackgroundColor = Color.FromArgb("#A5D6A7");
                }
            }
        }
    }
}
