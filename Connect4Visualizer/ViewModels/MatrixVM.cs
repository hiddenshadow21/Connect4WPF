using System.Collections.ObjectModel;

namespace Connect4Visualizer.ViewModels;

public class MatrixVM
{
    public ObservableCollection<Circle> Array { get; set; }

    public MatrixVM()
    {
        Array = new ObservableCollection<Circle>();
        double[,] data = new double[,] { { 1, 2, 3, 4, 5 }, { -1, -2, -3, -4, -5 } };

        for (int i = 0; i < data.GetLength(0); i++)
        {
            for (int j = 0; j < data.GetLength(1); j++)
            {
                Color color = data[i, j] > 0 ? Colors.Red : Colors.Yellow;
                Array.Add(new Circle { Color = new SolidColorBrush(color) });
            }
        }
    }
}

public class Circle
{
    public Brush Color { get; set; }
}