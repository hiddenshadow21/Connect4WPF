namespace Connect4;

public static class BoardChecks
{
    
    private static Kmp _kmpP1 = new Kmp(Enumerable.Repeat(CellState.Player1, 4).ToList());
    private static Kmp _kmpP2 = new Kmp(Enumerable.Repeat(CellState.Player2, 4).ToList());
    
    internal static bool CheckForDraw(Cell[,] board)
    {
        var row = 0; // top of the board
        for (var col = 0; col < board.GetLength(1); col++)
        {
            if (board[row, col].State == CellState.Empty)
            {
                return false;
            }
        }

        return true;
    }

    internal static bool CheckForWin(Cell[,] board, int column, int row, CellState currentPlayer)
    {
        return CheckDiagonalWin(board, column, row, currentPlayer) ||
               CheckAntiDiagonalWin(board, column, row, currentPlayer) ||
               CheckHorizontalWin(board, row,currentPlayer) ||
               CheckVerticalWin(board, column, currentPlayer);
    }

    private static bool CheckVerticalWin(Cell[,] board, int column, CellState currentPlayer)
    {
        var elements = board.ToEnumerable().Where(x => x.Column == column).Select(x => x.State).ToList();

        return currentPlayer == CellState.Player1 ? _kmpP1.Match(elements) : _kmpP2.Match(elements);
    }

    private static bool CheckHorizontalWin(Cell[,] board, int row, CellState currentPlayer)
    {
        
        var elements = board.ToEnumerable().Where(x => x.Row == row).Select(x => x.State).ToList();

        return currentPlayer == CellState.Player1 ? _kmpP1.Match(elements) : _kmpP2.Match(elements);
    }

    private static bool CheckDiagonalWin(Cell[,] board, int column, int row, CellState currentPlayer)
    {
        var elements = board.ToEnumerable().Where(x => x.Row - x.Column == row - column).Select(x => x.State).ToList();

        return currentPlayer == CellState.Player1 ? _kmpP1.Match(elements) : _kmpP2.Match(elements);
    }

    private static bool CheckAntiDiagonalWin(Cell[,] board, int column, int row, CellState currentPlayer)
    {
        var elements = board.ToEnumerable().Where(x => x.Row + x.Column == row + column).Select(x => x.State).ToList();

        return currentPlayer == CellState.Player1 ? _kmpP1.Match(elements) : _kmpP2.Match(elements);
    }
}