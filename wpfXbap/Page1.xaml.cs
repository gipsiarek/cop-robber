using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace wpfXbap
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page 
    {
        public Page1()
        {
            try
            {
                InitializeComponent();
                edgeTypeList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #region FUNKCJE PODSTAWOWE
        private void btnDrawGraph_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Add("gWidth", txbGraphWidth.Text);
            Application.Current.Properties.Add("gHeight", txbGraphHeight.Text);
            Application.Current.Properties.Add("gType",  cmbEdgeType.SelectedValue.ToString());
            NavigationService.GetNavigationService(this).Navigate(new Uri("UserGame.xaml", UriKind.RelativeOrAbsolute));
        }

        private void btnStartTests_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Properties.Add("tNodeNumberMin", txbNodeNumberMin.Text);
            Application.Current.Properties.Add("tNodeNumberMax", txbNodeNumberMax.Text);
            Application.Current.Properties.Add("tTestNumber", txbTestNumber.Text);
            Application.Current.Properties.Add("tMaxNodeSt", txbMaxNodeSt.Text);
            NavigationService.GetNavigationService(this).Navigate(new Uri("Tests.xaml", UriKind.RelativeOrAbsolute));
        }
        #endregion

        #region EVENTS

        private void txbGraphWidth_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Handled = !AreAllValidNumericChars(txbGraphWidth.Text))
            {
                MessageBox.Show("Niepoprawny format, proszę wpisać tylko liczby");
                txbGraphWidth.Text = "";
                txbGraphWidth.Focus();
            }
        }

        private void txbGraphHeight_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Handled = !AreAllValidNumericChars(txbGraphHeight.Text))
            {
                MessageBox.Show("Niepoprawny format, proszę wpisać tylko liczby");
                txbGraphHeight.Text = "";
                txbGraphHeight.Focus();
            }
        }
        private void txbNodeNumberMin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Handled = !AreAllValidNumericChars(txbNodeNumberMin.Text))
            {
                MessageBox.Show("Niepoprawny format, proszę wpisać tylko liczby");
                txbNodeNumberMin.Text = "";
                txbNodeNumberMin.Focus();
            }
        }
        private void txbTestNumber_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Handled = !AreAllValidNumericChars(txbTestNumber.Text))
            {
                MessageBox.Show("Niepoprawny format, proszę wpisać tylko liczby");
                txbTestNumber.Text = "";
                txbTestNumber.Focus();
            }
        }
        private void txbNodeNumberMax_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Handled = !AreAllValidNumericChars(txbNodeNumberMax.Text))
            {
                MessageBox.Show("Niepoprawny format, proszę wpisać tylko liczby");
                txbNodeNumberMax.Text = "";
                txbNodeNumberMax.Focus();
            }
        }
        private void txbMaxNodeSt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (e.Handled = !AreAllValidNumericChars(txbMaxNodeSt.Text))
            {
                MessageBox.Show("Niepoprawny format, proszę wpisać tylko liczby");
                txbNodeNumberMax.Text = "";
                txbNodeNumberMax.Focus();
            }

        }

        #endregion

        #region FUNKCJE DODATKOWE

        /// Check if string is a number
        private bool AreAllValidNumericChars(string str)
        {
            bool ret = true;
            if (str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencyGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeSign |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NegativeInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.NumberGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentDecimalSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentGroupSeparator |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PercentSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PerMilleSymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveInfinitySymbol |
                str == System.Globalization.NumberFormatInfo.CurrentInfo.PositiveSign)
                return ret;

            int l = str.Length;
            for (int i = 0; i < l; i++)
            {
                char ch = str[i];
                ret &= Char.IsDigit(ch);
            }

            return ret;
        }
        /// Item source for combobox
        private void edgeTypeList()
        {
            List<string> edgeTypeList = new List<string>();
            edgeTypeList.Add("c3");
            edgeTypeList.Add("c4");
            edgeTypeList.Add("random");
            cmbEdgeType.DataContext = edgeTypeList;
        }

        #endregion


    }


}
