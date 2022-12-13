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
using System.Windows.Shapes;

namespace DS2_DEATH_COUNT.EditorWindow
{
    /// <summary>
    /// Lógica interna para WinEditor.xaml
    /// </summary>
    public partial class WinEditor : Window
    {
        public MainWindow MyWin;
        public WinEditor(MainWindow win)
        {
            MyWin = win;
            InitializeComponent();
        }

        private void tb_WinHeight_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_actualEditor.Content = tb_WinHeight.Name;
        }

        private void tb_WinWidth_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_actualEditor.Content = tb_WinWidth.Name;
        }

        private void tb_WinButtom_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_actualEditor.Content = tb_WinButtom.Name;
        }

        private void tb_WinLeft_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_actualEditor.Content = tb_WinLeft.Name;
        }

        private void tb_FontSz_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_actualEditor.Content = tb_FontSz.Name;
        }

        private void tb_FontPadding_GotFocus(object sender, RoutedEventArgs e)
        {
            lbl_actualEditor.Content = tb_FontPadding.Name;
        }

        private void btn_GetValues_Click(object sender, RoutedEventArgs e)
        {
            /*
                public int PositionBottom   = 0;
                public int PositionLeft     = 0;
                public int WinHeight        = 0;
                public int WinWidth         = 0;
                public int FontSize         = 0;
                public int FontPaddingTop   = 0;
             */
            tb_WinHeight.Text = MyWin.WinHeight.ToString();
            tb_WinWidth.Text = MyWin.WinWidth.ToString();
            tb_WinButtom.Text = MyWin.PositionBottom.ToString();   
            tb_WinLeft.Text = MyWin.PositionLeft.ToString();
            tb_FontPadding.Text = MyWin.FontPaddingTop.ToString();
            tb_FontSz.Text = MyWin.FontSize.ToString();

            lbl_GameResolution.Content = MyWin.GWidth.ToString() + "x" + MyWin.GHeight.ToString();

        }

        private void btn_ApplyConfig_Click(object sender, RoutedEventArgs e)
        {
            MyWin.WinHeight         = Convert.ToInt32( tb_WinHeight.Text );
            MyWin.WinWidth          = Convert.ToInt32( tb_WinWidth.Text );
            MyWin.PositionBottom    = Convert.ToInt32( tb_WinButtom.Text );
            MyWin.PositionLeft      = Convert.ToInt32( tb_WinLeft.Text );
            MyWin.FontPaddingTop    = Convert.ToInt32( tb_FontPadding.Text );
            MyWin.FontSize          = Convert.ToInt32( tb_FontSz.Text );

            var str = "WinHeight = " + tb_WinHeight.Text + ";" + Environment.NewLine +
                "WinWidth = " + tb_WinWidth.Text + ";" + Environment.NewLine +
                "PositionBottom = " + tb_WinButtom.Text + ";" + Environment.NewLine +
                "PositionLeft = " + tb_WinLeft.Text + ";" + Environment.NewLine +
                "FontPaddingTop = " + tb_FontPadding.Text + ";" + Environment.NewLine +
                "FontSize = " + tb_FontSz.Text + ";";


            Clipboard.SetText(str);
        }
    }
}
