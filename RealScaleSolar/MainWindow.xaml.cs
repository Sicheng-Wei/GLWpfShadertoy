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
    public struct GLState
    {
        public bool     iInitialize;
        public float    iTime;
        public float[]  iResolution;
        public float[]  iMouse;

        public Matrix4 viewMatrix;
    }

    public partial class MainWindow : Window
    {
        /*** WPF Framework Operation ***/
        private static bool Locked = true;
        
        // Window Draggable
        private void Window_MouseLeftButtonDrag(object sender, MouseButtonEventArgs e)
        {
            Point dragArea = e.GetPosition(FuncDock);
            if (dragArea.Y < 0)
            {
                DragMove();
            }
        }

        // Lock Window Size
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

        // Window Minimize
        private void Window_MouseClickMinim(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        // Window ChangeSize
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

        // Shutdown
        private void Window_MouseClickClose(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Process.GetCurrentProcess().Kill();
        }



        /*** GLWpfControl Render ***/
        
        public static GLState CurrentGLState = new GLState();
        private static long StartTick;
        private static ImageResult image = ImageResult.FromStream(File.OpenRead("./Resources/ResBitmap/3_Earth/DayMap.jpg"), ColorComponents.RedGreenBlueAlpha);
        private static byte[] data = image.Data;
        private static int[] stats = { image.Width, image.Height };
        
        private void Screen_OnRender(TimeSpan timeSpan)
        {
            CurrentGLState.iTime = (DateTime.Now.Ticks - StartTick) / 1e7f;

            CurrentGLState.iResolution[0] = GLScreen.FrameBufferWidth;
            CurrentGLState.iResolution[1] = GLScreen.FrameBufferHeight;

            //MousePosition
            CurrentGLState.iMouse[0] = (float)Mouse.GetPosition(GLScreen).X;
            CurrentGLState.iMouse[1] = (float)Mouse.GetPosition(GLScreen).Y;
            ViewUpdate();
            GLSLPort.Render(data, stats);
            FrameRate.Text = CurrentGLState.iTime.ToString("00000.00000");
            
            // FrameRate.Text += "(" + CurrentGLState.iMouse[0].ToString() + ", " + CurrentGLState.iMouse[1].ToString() + ")";
            FrameRate.Text += "\n" + (1.0f / ((DateTime.Now.Ticks - StartTick) / 1e7f - CurrentGLState.iTime)).ToString();
            
            CurrentGLState.iInitialize = true;
        }

        // Keyboard Response
        private static bool W, A, S, D, Q, E, UP, DOWN;     // Directional Keys
        private void Handle_KeyDown(object sender, KeyEventArgs e)
        {
            // Front & Back
            if (e.Key == Key.W) W = true;
            if (e.Key == Key.S) S = true;
            
            // Left & Right
            if (e.Key == Key.A) A = true;
            if (e.Key == Key.D) D = true;

            // CounterClk & Clk
            if (e.Key == Key.Q) Q = true;
            if (e.Key == Key.E) E = true;

        }

        private void ViewUpdate()
        {
            Vector4 positeVector = new Vector4(CurrentGLState.viewMatrix.ExtractTranslation(), 0.0f);
            Matrix4 relateMatrix = CurrentGLState.viewMatrix.ClearTranslation();

            //// Front & Back
            if (W) positeVector += Vector4.Multiply(relateMatrix.Row0, 0.01f);
            if (S) positeVector -= Vector4.Multiply(relateMatrix.Row0, 0.01f);

            //// Left & Right
            if (A) positeVector += Vector4.Multiply(relateMatrix.Row2, 0.01f);
            if (D) positeVector -= Vector4.Multiply(relateMatrix.Row2, 0.01f);

            //// CounterClk & Clk
            if (Q) relateMatrix *= Matrix4.CreateRotationX(+0.01f);
            if (E) relateMatrix *= Matrix4.CreateRotationX(-0.01f);

            relateMatrix.Row3 += positeVector;

            CurrentGLState.viewMatrix = relateMatrix;
        }

        private void Handle_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.W) W = false;
            if (e.Key == Key.A) A = false;
            if (e.Key == Key.S) S = false;
            if (e.Key == Key.D) D = false;
            if (e.Key == Key.Q) Q = false;
            if (e.Key == Key.E) E = false;
        }

        public MainWindow()
        {
            MaxHeight = SystemParameters.WorkArea.Height;
            MaxWidth = SystemParameters.WorkArea.Width;
            StartTick = DateTime.Now.Ticks;
            InitializeComponent();

            CurrentGLState.iInitialize = false;
            CurrentGLState.iResolution = new float[2];
            CurrentGLState.iMouse = new float[2];

            CurrentGLState.viewMatrix = Matrix4.CreateTranslation(0.0f, 1.0f, 0.0f);

            var settings = new GLWpfControlSettings
            {
                MajorVersion = 4,
                MinorVersion = 6,
            };
            GLScreen.Start(settings);
        }
    }
}
