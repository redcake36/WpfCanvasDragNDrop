﻿using System;
using System.Collections.Generic;
using System.Globalization;
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

namespace CanvasDragNDrop
{
    /// <summary>
    /// Логика взаимодействия для ElementPropertiesWindow.xaml
    /// </summary>
    public partial class ElementPropertiesWindow : Window
    {
        Canvas canvas;

        public Brush brush
        {
            get { return new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorBox.Text)); }
            set { colorBox.Text = value.ToString(); }
        }
        public String title
        {
            get { return titleBox.Text; }
            set { titleBox.Text = value; }
        }
        public int height
        {
            get { return Int32.Parse( heightBox.Text); }
            set { heightBox.Text = value.ToString();}
        }
        public int width
        {
            get { return Int32.Parse(widthBox.Text); }
            set { widthBox.Text = value.ToString();}
        }
        public double temperature
        {
            get { return GetDouble(temperatureBox.Text); }
            set { temperatureBox.Text = value.ToString(); }
        }
        public ElementPropertiesWindow()
        {
            InitializeComponent();
            //canvas = elementPropertiesWindowCanvas;
        }
        public void AddCanvasText(string s)
        {
            canvas.Children.Add(new TextBlock { Text = s });
        }
        public void AddListText(string s) {
            //elementPropertiesWindowList.Items.Add((new ListBoxItem { }).Content = s);
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
        public static double GetDouble(string value, double defaultValue = 0.0)
        {
            double result;

            //Try parsing in the current culture
            if (!double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.CurrentCulture, out result) &&
                //Then try in US english
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.GetCultureInfo("en-US"), out result) &&
                //Then in neutral language
                !double.TryParse(value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out result))
            {
                result = defaultValue;
            }

            return result;
        }
    }
}
