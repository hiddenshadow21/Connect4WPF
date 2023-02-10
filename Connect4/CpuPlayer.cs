using MethodTimer;

namespace Connect4;

public class CpuPlayer
{
    public int Depth { get; set; } = 3;
    public bool UseStandardMiniMax { get; set; }
    private readonly CellState _aiPlayer;
    private readonly CellState _opponent;

    public CpuPlayer(CellState aiPlayer)
    {
        _aiPlayer = aiPlayer;
        _opponent = _aiPlayer == CellState.Player1 ? CellState.Player2 : CellState.Player1;
    }

    [Time]
    public int GetNextMove(Cell[,] board)
    {
        if (UseStandardMiniMax)
        {
            var (column, score) = MinimaxStandard(board, Depth, true);
            return column;
        }
        else
        {
            var (column, score) = Minimax(board, Depth, int.MinValue, int.MaxValue, true);
            return column;
        }
    }

    private (int col, int score) Minimax(Cell[,] board, int depth, int alpha, int beta, bool isMaximizingPlayer)
    {
        var terminalNode = TerminalNode(board);
        if (depth == 0 || terminalNode)
        {
            if (!terminalNode)
                return (-1, EvaluateBoard(board));
            if (board.PlayerWon(_aiPlayer))
                return (-1, 4000000);
            if (board.PlayerWon(_opponent))
                return (-1, -4000000);

            return (-1, 0);
        }

        var height = board.GetLength(0);
        var length = board.GetLength(1);

        if (isMaximizingPlayer)
        {
            var bestScore = int.MinValue;
            var bestColumn = 0;
            for (var column = 0; column < length; column++)
            {
                var result = board.MakeMove(column, _aiPlayer);
                if (result == -1)
                    continue;

                var newScore = Minimax(board, depth - 1, alpha, beta, false).score;
                if (newScore > bestScore)
                {
                    bestColumn = column;
                    bestScore = newScore;
                }

                board.UndoMove(column);
                alpha = Math.Max(alpha, bestScore);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return (bestColumn, bestScore);
        }
        else
        {
            var bestScore = int.MaxValue;
            var bestColumn = 0;
            for (var column = 0; column < length; column++)
            {
                var result = board.MakeMove(column, _opponent);
                if (result == -1)
                    continue;

                var newScore = Minimax(board, depth - 1, alpha, beta, true).score;
                if (newScore < bestScore)
                {
                    bestColumn = column;
                    bestScore = newScore;
                }

                board.UndoMove(column);
                beta = Math.Min(beta, bestScore);
                if (beta <= alpha)
                {
                    break;
                }
            }

            return (bestColumn, bestScore);
        }
    }

    private (int col, int score) MinimaxStandard(Cell[,] board, int depth, bool isMaximizingPlayer)
    {
        var terminalNode = TerminalNode(board);
        if (depth == 0 || terminalNode)
        {
            if (!terminalNode)
                return (-1, EvaluateBoard(board));
            if (board.PlayerWon(_aiPlayer))
                return (-1, 4000000);
            if (board.PlayerWon(_opponent))
                return (-1, -4000000);

            return (-1, 0);
        }

        var height = board.GetLength(0);
        var length = board.GetLength(1);

        if (isMaximizingPlayer)
        {
            var bestScore = int.MinValue;
            var bestColumn = 0;
            for (var column = 0; column < length; column++)
            {
                var result = board.MakeMove(column, _aiPlayer);
                if (result == -1)
                    continue;

                var newScore = MinimaxStandard(board, depth - 1, false).score;
                if (newScore > bestScore)
                {
                    bestColumn = column;
                    bestScore = newScore;
                }

                board.UndoMove(column);
            }

            return (bestColumn, bestScore);
        }
        else
        {
            var bestScore = int.MaxValue;
            var bestColumn = 0;
            for (var column = 0; column < length; column++)
            {
                var result = board.MakeMove(column, _opponent);
                if (result == -1)
                    continue;

                var newScore = MinimaxStandard(board, depth - 1, true).score;
                if (newScore < bestScore)
                {
                    bestColumn = column;
                    bestScore = newScore;
                }

                board.UndoMove(column);
            }

            return (bestColumn, bestScore);
        }
    }


    private bool TerminalNode(Cell[,] board)
    {
        if (board.PlayerWon(_aiPlayer) || board.PlayerWon(_opponent))
            return true;

        var list = new List<int>();
        for (var i = 0; i < board.GetLength(1); i++)
        {
            list.Add(board.GetEmptyRowNumber(i));
        }

        return list.All(x => x == -1);
    }

    private int EvaluateBoard(Cell[,] board)
    {
        var score = 0;
        var rows = board.GetLength(0);
        var cols = board.GetLength(1);

        // Prefer center column
        for (var i = 0; i < rows; i++)
        {
            if (board[i, (int) Math.Floor(cols / 2.0)].State == _aiPlayer)
                score += 4;
        }

        var listSize = 4;
        //Score horizontals
        for (var i = 0; i < rows; i++)
        {
            var row = board.ToEnumerable().Where(x => x.Row == i).ToList();

            for (int c = 0; c < row.Count - listSize + 1; c++)
            {
                score += Evaluate4Elements(row.Skip(c).Take(listSize).Select(x => x.State).ToList());
            }
        }

        //Score vertical
        for (var i = 0; i < cols; i++)
        {
            var column = board.ToEnumerable().Where(x => x.Column == i).Select(x => x.State).ToList();

            for (int r = 0; r < column.Count - listSize + 1; r++)
            {
                score += Evaluate4Elements(column.Skip(r).Take(listSize).ToList());
            }
        }

        // diagonals
        for (var i = -cols + 3; i < rows - 2; i++)
        {
            var diagonal = board.ToEnumerable().Where(x => x.Row - x.Column == i).Select(x => x.State).ToList();

            for (var j = 0; j < diagonal.Count - listSize + 1; j++)
            {
                score += Evaluate4Elements(diagonal.Skip(j).Take(listSize).ToList());
            }
        }

        // anti diagonals
        for (var i = 3; i < rows + cols - 4; i++)
        {
            var diagonal = board.ToEnumerable().Where(x => x.Row + x.Column == i).Select(x => x.State).ToList();

            for (var j = 0; j < diagonal.Count - listSize + 1; j++)
            {
                score += Evaluate4Elements(diagonal.Skip(j).Take(listSize).ToList());
            }
        }

        return score;
    }

    private int Evaluate4Elements(List<CellState> list)
    {
        var score = 0;

        var aiCount = list.Count(x => x == _aiPlayer);
        var emptyCount = list.Count(x => x == CellState.Empty);

        switch (aiCount)
        {
            case 4:
                score += 1000;
                break;
            case 3 when emptyCount == 1:
                score += 10;
                break;
            case 2 when emptyCount == 2:
                score += 5;
                break;
        }

        if (list.Count(x => x == _opponent) == 2 && emptyCount == 2)
            score -= 9;

        return score;
    }
}