using System;
using System.IO;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using OpenTK.Wpf;
using StbImageSharp;
using OpenTK.Mathematics;
using System.Windows.Data;

namespace GLWpfShadertoy
{
    internal class wpfUI : Window
    {
        ///*** WPF Framework Operation ***/
        //private static bool Locked = true;

        //// Window Draggable
        //private void Window_MouseLeftButtonDrag(object sender, MouseButtonEventArgs e)
        //{
        //    Point dragArea = e.GetPosition(FuncDock);
        //    if (dragArea.Y < 0)
        //    {
        //        DragMove();
        //    }
        //}

        //// Lock Window Size
        //private void Window_MouseClickLocke(object sender, RoutedEventArgs e)
        //{
        //    if (Locked)
        //    {
        //        Locked = false;
        //        Btn_Locke.Content = "🔓";
        //    }
        //    else
        //    {
        //        Locked = true;
        //        Btn_Locke.Content = "🔒";
        //    }
        //}

        //// Window Minimize
        //private void Window_MouseClickMinim(object sender, RoutedEventArgs e)
        //{
        //    WindowState = WindowState.Minimized;
        //}

        //// Window ChangeSize
        //private void Window_MouseClickResiz(object sender, RoutedEventArgs e)
        //{
        //    if (!Locked && WindowState == WindowState.Maximized)
        //    {
        //        WindowState = WindowState.Normal;
        //        Btn_Resiz.Content = "□";
        //        Btn_Resiz.FontSize = 16;
        //    }
        //    else if (!Locked && WindowState != WindowState.Maximized)
        //    {
        //        WindowState = WindowState.Maximized;
        //        Btn_Resiz.Content = "⚪";
        //        Btn_Resiz.FontSize = 12;
        //    }
        //}

        //// Shutdown
        //private void Window_MouseClickClose(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.MainWindow.Close();
        //    Process.GetCurrentProcess().Kill();
        //}
    }
}
