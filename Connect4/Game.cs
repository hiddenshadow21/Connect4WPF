namespace Connect4;

public class Game
{
    private readonly Cell[,] _board = new Cell[6, 7];

    public Cell[,] Board => _board;

    public CellState CurrentPlayer = CellState.Player1;

    public bool GameOver { get; set; }

    public Game()
    {
        for (var i = 0; i < 6; i++)
        {
            for (var j = 0; j < 7; j++)
            {
                _board[i, j] = new Cell
                {
                    Row = i,
                    Column = j,
                    State = CellState.Empty
                };
            }
        }
    }

    public MoveResult MakeMove(int column)
    {
        var row = _board.MakeMove(column, CurrentPlayer);
        if (row < 0)
            return MoveResult.IllegalMove;

        if (BoardChecks.CheckForWin(_board, column, row, CurrentPlayer))
        {
            GameOver = true;
            return MoveResult.Win;
        }

        if (BoardChecks.CheckForDraw(_board))
        {
            GameOver = true;
            return MoveResult.Draw;
        }

        // switch to the other player's turn
        CurrentPlayer = (CurrentPlayer == CellState.Player1) ? CellState.Player2 : CellState.Player1;

        return MoveResult.CorrectMove;
    }

    public void Restart()
    {
        foreach (var cell in _board)
        {
            cell.State = CellState.Empty;
        }
        GameOver = false;
        CurrentPlayer = CellState.Player1;
    }

}