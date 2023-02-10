using Connect4;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Connect4WPF.ViewModels
{
    public class ConnectFourVM : INotifyPropertyChanged
    {
        private Game _model;
        
        private CpuPlayer _cpuPlayer;
        
        public ConnectFourVM()
        {
            _cpuPlayer = new CpuPlayer(CellState.Player2);

            _model = new Game();
        }

        public Cell[,] Board => _model.Board;

        public int Depth
        {
            get => _cpuPlayer.Depth;
            set
            {
                _cpuPlayer.Depth = value;
                OnPropertyChanged();
            } 
        }

        public CellState CurrentPlayer => _model.CurrentPlayer;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand MakeMoveCommand => new RelayCommand(async column =>
        {
            var result = _model.MakeMove((int)column);
            OnPropertyChanged(nameof(CurrentPlayer));
            OnPropertyChanged(nameof(Board));
            AllowUiToUpdate();

            if (CheckResult(result))
            {
                RestartGame();
                return;
            }

            if (CurrentPlayer == CellState.Player2)
            {
                var x = await Task.Run(() => _cpuPlayer.GetNextMove(Board));
                result = _model.MakeMove(x);
            }
            
            OnPropertyChanged(nameof(CurrentPlayer));
            OnPropertyChanged(nameof(Board));
            if (CheckResult(result))
            {
                RestartGame();
            }
        });

        private void RestartGame()
        {
            _model.Restart();
            OnPropertyChanged(nameof(CurrentPlayer));
            OnPropertyChanged(nameof(Board));
        }

        private bool CheckResult(MoveResult result)
        {
            switch (result)
            {
                case MoveResult.Win:
                    MessageBox.Show("Player " + CurrentPlayer + " wins!");
                    return true;
                case MoveResult.Draw:
                    MessageBox.Show("Draw!");
                    return true;
                case MoveResult.IllegalMove:
                    MessageBox.Show("Can't make this move. Try again.");
                    return true;
                default:
                    return false;
            }
        }

        private static void AllowUiToUpdate()
        {
            DispatcherFrame frame = new();
            // DispatcherPriority set to Input, the highest priority
            Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Input, new DispatcherOperationCallback(delegate (object parameter)
            {
                frame.Continue = false;
                Thread.Sleep(20); // Stop all processes to make sure the UI update is perform
                return null;
            }), null);
            Dispatcher.PushFrame(frame);
            // DispatcherPriority set to Input, the highest priority
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Input, new Action(delegate { }));
        }
    }
}
