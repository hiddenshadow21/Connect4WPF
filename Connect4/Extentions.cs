namespace Connect4;

public static class Extentions
{
    public static int MakeMove(this Cell[,] board, int column, CellState currentPlayer)
    {
        if (board[0, column].State != CellState.Empty)
        {
            return -1;
        }

        var row = board.GetLength(0) - 1;
        while (board[row, column].State != CellState.Empty)
        {
            row--;
        }

        board[row, column].State = currentPlayer;

        return row;
    }

    public static void UndoMove(this Cell[,] board, int column)
    {
        //empty column
        if (board[board.GetLength(0) - 1, column].State == CellState.Empty)
            return;

        var row = 0;
        while (board[row, column].State == CellState.Empty)
        {
            row++;
        }

        board[row, column].State = CellState.Empty;
    }

    public static int GetEmptyRowNumber(this Cell[,] board, int column)
    {
        var row = board.GetLength(0) - 1;
        while (board[row, column].State != CellState.Empty)
        {
            row--;

            if (row == -1)
            {
                return -1;
            }
        }

        return row;
    }

    public static bool PlayerWon(this Cell[,] board, CellState player)
    {
        for (var i = 0; i < board.GetLength(0); i++)
        {
            for (var j = 0; j < board.GetLength(1); j++)
            {
                if (BoardChecks.CheckForWin(board, j, i, player))
                    return true;
            }
        }

        return false;
    }
    
    public static IEnumerable<Cell> ToEnumerable(this Cell[,] target)
    {
        foreach (var item in target)
            yield return item;
    }
}