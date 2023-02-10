using Connect4;

namespace ConsoleGame;

public class ConsoleConnect4
{
    private readonly Game _game;


    public ConsoleConnect4(Game.PlayerMove player1Move, Game.PlayerMove player2Move)
    {
        _game = new Game(player1Move, player2Move);
    }

    public CellState Start() => _game.Start();
}