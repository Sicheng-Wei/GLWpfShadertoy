using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using OpenTK.Wpf;

namespace RealScaleSolar
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
            InitializeComponent();
            Dispatcher.BeginInvoke(() =>
            {
                var settings = new GLWpfControlSettings
                {
                    MajorVersion = 4,
                    MinorVersion = 6
                };
                TriangleDisplay.Start(settings);
            });
            
        }

        private static bool Locked = true;

        /* 窗口基础操作 */

        // 可拖动
        private void Window_MouseLeftButtonDrag(object sender, MouseButtonEventArgs e)
        {
            Point dragArea = e.GetPosition(FuncDock);
            if (dragArea.Y < 0)
            {
                DragMove();
            }
        }

        // 固定窗口大小
        private void Window_MouseClickLocke(object sender, RoutedEventArgs e)
        {
            if (Locked)
            {
                Locked = false;
                Btn_Locke.Content = "🔓";
            }
            else
            {
                Locked = true;
                Btn_Locke.Content = "🔒";
            }
        }

        // 最小化
        private void Window_MouseClickMinim(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // 窗口大小
        private void Window_MouseClickResiz(object sender, RoutedEventArgs e)
        {
            if (!Locked && WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
                Btn_Resiz.Content = "□";
                Btn_Resiz.FontSize = 16;
            }
            else if (!Locked && WindowState != WindowState.Maximized)
            {
                WindowState = WindowState.Maximized;
                Btn_Resiz.Content = "⚪";
                Btn_Resiz.FontSize = 12;
            }
        }

        // 关闭窗口
        private void Window_MouseClickClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Process.GetCurrentProcess().Kill();
        }

        private async void Triangle_OnRender(TimeSpan timeSpan)
        {
            await Triangle.Render();
        }
    }
}
