using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Threading;
using System.Threading.Tasks;
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
            StartTick = DateTime.Now.Ticks;
            InitializeComponent();
            
            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 6,
            };
            GLScreen.Start(settings);
        }

        private static bool Locked = true;
        private static float[] GLScreenSize = { 0.0f, 0.0f };
        private static long StartTick;

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

        private void Screen_OnRender(TimeSpan timeSpan)
        {
            GLScreenSize[0] = GLScreen.FrameBufferWidth;
            GLScreenSize[1] = GLScreen.FrameBufferHeight;

            //Thread GLThread = new Thread(() =>
            //    {
            //        float iTime = (DateTime.Now.Ticks - StartTick) / 10000000f;
            //        Application.Current.Dispatcher.BeginInvoke(
            //            System.Windows.Threading.DispatcherPriority.Normal,
            //            new Action(() =>
            //            {
            //                GLSLPort.Render(GLScreenSize, iTime);
            //                FrameRate.Text = iTime.ToString("00000.00000");
            //            })
            //        );
            //    }
            //);
            //GLThread.Start();

            Dispatcher.Invoke(
                new Action(
                    delegate
                    {
                        float iTime = (DateTime.Now.Ticks - StartTick) / 10000000f;
                        GLSLPort.Render(GLScreenSize, iTime);
                        FrameRate.Text = iTime.ToString("00000.00000");
                    }
                )
            );
        }
    }
}
