using _2048_Core.Abstract;
using _2048_Core.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _2048_WPF
{
   /// <summary>
   /// Interaction logic for MainWindow.xaml
   /// </summary>
   public partial class MainWindow : Window, IMainView
   {
      private Label[,] _labels;
      private const double _imageFontSizeLarge = 36;
      private const double _imageFontSizeMedium = 20;
      private const double _imageFontSizeSmall = 16;
      private const double _imageFontSizeVerySmall = 10;
      private const int LABEL_SIZE = 90;
      private const int WIDTH_CORRECTION = 10;
      private const int HEIGHT_CORRECTION = 100;

      public MainWindow()
      {
         InitializeComponent();
         IMessageService messageService = new WpfMessageService();
         Presenter presenter = new Presenter(this, messageService);
      }

      public uint CurrentScore { set => lblScore.Content = value.ToString(); }
      public uint HighScore { set => lblHighScore.Content = value.ToString(); }

      public event EventHandler ResetHighscore;
      public event EventHandler AboutClick;
      public event EventHandler RestartClick;
      public event EventHandler BackClick;
      public event EventHandler<GameParametersEventArgs> NewGameClick;
      public event EventHandler<ExitGameEventArgs> ExitClick;
      public event EventHandler<MousePositionEventArgs> MouseDownView;
      public event EventHandler<MousePositionEventArgs> MouseUpView;

      public void DrawGameField(int width, int height)
      {

         _mainGrid.Children.Clear();
         _mainGrid.ColumnDefinitions.Clear();
         for (int i = 0; i < width; i++)
         {
            ColumnDefinition cd = new ColumnDefinition();
            cd.Width = new GridLength(LABEL_SIZE);
            cd.MinWidth = LABEL_SIZE;
            _mainGrid.ColumnDefinitions.Add(cd);
         }
         _mainGrid.RowDefinitions.Clear();
         for (int j = 0; j < height; j++)
         {
            RowDefinition rd = new RowDefinition();
            rd.Height = new GridLength(LABEL_SIZE);
            rd.MinHeight = LABEL_SIZE;
            _mainGrid.RowDefinitions.Add(rd);
         }

         _labels = new Label[width, height];
         _mainGrid.Width = LABEL_SIZE * width;
         _mainGrid.Height = LABEL_SIZE * height;
         _mainWindow.Width = _mainGrid.Width + WIDTH_CORRECTION;
         _mainWindow.Height = _mainGrid.Height + HEIGHT_CORRECTION;
         for (int i = 0; i < width; i++)
         {
            for (int j = 0; j < height; j++)
            {
               Label lbl = new Label();
               lbl.Width = lbl.MinWidth = lbl.Height = lbl.MinHeight = LABEL_SIZE;
               lbl.HorizontalContentAlignment = HorizontalAlignment.Center;
               lbl.VerticalContentAlignment = VerticalAlignment.Center;
               lbl.FontWeight = FontWeights.Bold;
               lbl.FontSize = _imageFontSizeLarge;
               lbl.MouseDown += MouseDownHandler;
               lbl.MouseUp += MouseUpHandler;
               Grid.SetRow(lbl, i);
               Grid.SetColumn(lbl, j);
               _mainGrid.Children.Add(lbl);
               _labels[i, j] = lbl;
            }
         }
      }

      private void MouseUpHandler(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
         {
            Point ctrlPoint = Mouse.GetPosition(_mainGrid);
            MouseUpView?.Invoke(sender, new MousePositionEventArgs(ctrlPoint.X, ctrlPoint.Y));
         }
      }

      private void MouseDownHandler(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
         {
            Point ctrlPoint = Mouse.GetPosition(_mainGrid);
            MouseDownView?.Invoke(sender, new MousePositionEventArgs(ctrlPoint.X, ctrlPoint.Y));
         }
      }

      public void Set0(int i, int j)
      {
         _labels[i, j].Content = "";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Black;

      }
      public void Set2(int i, int j)
      {
         _labels[i, j].Content = "2";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Black;
      }
      public void Set4(int i, int j)
      {
         _labels[i, j].Content = "4";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.DarkGreen;
      }
      public void Set8(int i, int j)
      {
         _labels[i, j].Content = "8";
         _labels[i, j].FontSize = _imageFontSizeLarge; 
         _labels[i, j].Foreground = Brushes.Chartreuse;
      }
      public void Set16(int i, int j)
      {
         _labels[i, j].Content = "16";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Coral;
      }
      public void Set32(int i, int j)
      {
         _labels[i, j].Content = "32";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.BlueViolet;
      }
      public void Set64(int i, int j)
      {
         _labels[i, j].Content = "64";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Blue;
      }
      public void Set128(int i, int j)
      {
         _labels[i, j].Content = "128";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Yellow;
      }
      public void Set256(int i, int j)
      {
         _labels[i, j].Content = "256";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Coral;
      }
      public void Set512(int i, int j)
      {
         _labels[i, j].Content = "512";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Red;
      }
      public void Set1024(int i, int j)
      {
         _labels[i, j].Content = "1024";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.DarkRed;
      }
      public void Set2048(int i, int j)
      {
         _labels[i, j].Content = "2048";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.DarkRed;
      }
      public void Set4096(int i, int j)
      {
         _labels[i, j].Content = "4096";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Orange;
      }
      public void Set8192(int i, int j)
      {
         _labels[i, j].Content = "8192";
         _labels[i, j].FontSize = _imageFontSizeLarge;
         _labels[i, j].Foreground = Brushes.Orange;
      }
      public void Set16384(int i, int j)
      {
         _labels[i, j].Content = "16384";
         _labels[i, j].FontSize = _imageFontSizeSmall;
         _labels[i, j].Foreground = Brushes.DarkOrange;
      }
      public void Set32768(int i, int j)
      {
         _labels[i, j].Content = "32768";
         _labels[i, j].FontSize = _imageFontSizeSmall;
         _labels[i, j].Foreground = Brushes.DarkOrange;
      }
      public void Set65536(int i, int j)
      {
         _labels[i, j].Content = "65536";
         _labels[i, j].FontSize = _imageFontSizeSmall;
         _labels[i, j].Foreground = Brushes.DarkOrange;
      }
      public void Set131072(int i, int j)
      {
         _labels[i, j].Content = "131072";
         _labels[i, j].FontSize = _imageFontSizeVerySmall;
         _labels[i, j].Foreground = Brushes.DarkOrange;
      }

      private void BtnBack_Click(object sender, RoutedEventArgs e)
      {
         BackClick?.Invoke(sender, e);
      }

      private void BtnRestart_Click(object sender, RoutedEventArgs e)
      {
         RestartClick?.Invoke(sender, e);
      }

      private void BigGame_Click(object sender, RoutedEventArgs e)
      {
         NewGameClick?.Invoke(sender, new GameParametersEventArgs(5, 5));
      }

      private void MediumGame_Click(object sender, RoutedEventArgs e)
      {
         NewGameClick?.Invoke(sender, new GameParametersEventArgs(4, 4));
      }

      private void TinyGame_Click(object sender, RoutedEventArgs e)
      {
         NewGameClick?.Invoke(sender, new GameParametersEventArgs(3, 3));
      }

      private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
      {
         ExitClick?.Invoke(sender, new ExitGameEventArgs(false));
      }

      private void Exit_Click(object sender, RoutedEventArgs e)
      {
         ExitClick?.Invoke(sender, new ExitGameEventArgs(true));
      }

      private void About_Click(object sender, RoutedEventArgs e)
      {
         AboutClick?.Invoke(sender, e);
      }

      private void ResetHighScore_Click(object sender, RoutedEventArgs e)
      {
         ResetHighscore?.Invoke(sender, e);
      }
   }
}
