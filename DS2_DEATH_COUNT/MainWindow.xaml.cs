using DS2_DEATH_COUNT.EditorWindow;
using DS2_DEATH_COUNT.memory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DS2_DEATH_COUNT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private mem MyMem = new mem();
        private DispatcherTimer TimerOne = new DispatcherTimer();
        private DispatcherTimer TimerKey = new DispatcherTimer();
        private Process? GameProcess = null;
        private IntPtr BaseAddress = (IntPtr)0x16148F0;
        private List<uint> Offsets = new List<uint>() { 0xD0, 0x490, 0x1A4 };
        private bool HideMenu = false;
        private bool inGame = false;

        /// <summary>
        ///  Position and Size to Window for Window size game relative
        ///  using on editor to facilite my usefull :)
        /// </summary>
        public int PositionBottom   = 0;
        public int PositionLeft     = 0;
        public int WinHeight        = 0;
        public int WinWidth         = 0;
        public int FontSize         = 15;
        public int FontPaddingTop   = 0;
        public int FontPaddingBack  = 0;

        //Game Resolution
        public int GHeight;
        public int GWidth;


        WinEditor winEditor;


        public MainWindow()
        {
            InitializeComponent();
            TimerOne.Tick += TimerOne_Tick;
            TimerOne.Interval = new TimeSpan(0, 0, 0, 0, 500);
            TimerOne.Start();

            TimerKey.Tick += TimerKey_Tick;
            TimerKey.Interval = new TimeSpan(0, 0, 0, 0, 5);
            TimerKey.Start();


        #if DEBUGWINDOW
            winEditor = new WinEditor(this);
            winEditor.Show();
        #endif

        }

        private void TimerKey_Tick(object? sender, EventArgs e)
        {
            if (mem.GetKey(0x24))
                HideMenu = !HideMenu;

            if (HideMenu || !inGame )
                MainWin.Visibility = Visibility.Collapsed;
            else
                MainWin.Visibility = Visibility.Visible;
        }

        private bool MenageProcess(string name)
        {
            if (GameProcess == null)
            {
                GameProcess = mem.GetProcessByName(name); //GetProcess Game "DarkSoulsII"
                return false;
            }

            if (GameProcess == null)
                return false; // If not found any game


            if (GameProcess.HasExited) // If Process has exited
            {
                GameProcess.Kill();
                GameProcess = null;
                return false;
            }

            return true;
        }

        //Does work in FullWindow
        private void Overlay()
        {
            mem.RECT WinPos = new mem.RECT();
            IntPtr handle = mem.FindWindowA("DarkSouls2", "DARK SOULS II");
            MainWin.Title = "DS2 Count Death";

            if (mem.GetWindowRect(handle, out WinPos))
            {
                //Game Window Size / Resolution
                GWidth = WinPos.esquerda - WinPos.direita;
                GHeight = WinPos.baixo - WinPos.topo;


                if (GWidth ==  820 && GHeight == 493)
                {
                    PositionBottom = 100;
                    PositionLeft = 210;
                    WinHeight = 145;
                    WinWidth = 25;
                    FontSize = 13;
                    FontPaddingTop = 0;
                }

                if (GWidth == 1044 && GHeight == 619)
                {
                    PositionBottom = 120;
                    PositionLeft = 270;
                    WinHeight = 190;
                    WinWidth = 30;
                    FontSize = 15;
                    FontPaddingTop = 5;
                }

                if (GWidth == 1172 && GHeight == 691)
                {
                    PositionBottom = 140;
                    PositionLeft = 300;
                    WinHeight = 210;
                    WinWidth = 35;
                    FontSize = 15;
                    FontPaddingTop = 5;

                }

                if (GWidth == 1300 && GHeight == 763)
                {
                    WinHeight = 235;
                    WinWidth = 36;
                    PositionBottom = 150;
                    PositionLeft = 335;
                    FontPaddingTop = 5;
                    FontSize = 15;
                    FontPaddingBack = 20;
                }

   
                if (GWidth == 1460 && GHeight == 853)
                {
                    WinHeight = 260;
                    WinWidth = 40;
                    PositionBottom = 165;
                    PositionLeft = 370;
                    FontPaddingTop = 5;
                    FontSize = 20;
                    FontPaddingBack = 22;
                }

                if (GWidth == 1620 && GHeight == 943)
                {
                    WinHeight = 290;
                    WinWidth = 40;
                    PositionBottom = 175;
                    PositionLeft = 415;
                    FontPaddingTop = 5;
                    FontSize = 20;
                    FontPaddingBack = 24;
                }

                if (GWidth == 1700 && GHeight == 988)
                {
                    WinHeight = 310;
                    WinWidth = 50;
                    PositionBottom = 190;
                    PositionLeft = 435;
                    FontPaddingTop = 6;
                    FontSize = 25;
                    FontPaddingBack = 25;
                }

                if (GWidth == 1940 && GHeight == 1123)
                {
                    WinHeight = 360;
                    WinWidth = 60;
                    PositionBottom = 220;
                    PositionLeft = 500;
                    FontPaddingTop = 10;
                    FontSize = 30;
                    FontPaddingBack = 25;
                }


                MainWin.Left = WinPos.esquerda - PositionLeft;
                MainWin.Top = WinPos.baixo - PositionBottom;
                MainWin.Width = WinHeight;
                MainWin.Height = WinWidth;
                label_module.FontSize = FontSize;
                label_module.Padding = new Thickness(0, FontPaddingTop, FontPaddingBack, 0);
            }
        }

        private void TimerOne_Tick(object sender, EventArgs e)
        {
           if(!MenageProcess("DarkSoulsII"))
            return;

            if (GameProcess == null)
                return;

            if (GameProcess.MainModule == null)
                return;

            var bm = (UIntPtr)((long)GameProcess.MainModule.BaseAddress + (long)BaseAddress);
            label_module.Content = GameProcess.MainModule.BaseAddress.ToString("x");
            int count_death = 0;

            if (mem.ReadPointerInteger(GameProcess.Handle, bm, Offsets, out count_death))
            {
                inGame = true;
                Overlay();
                label_module.Content = count_death.ToString();
            }else
                inGame = false;
        } 
    }
    
}
