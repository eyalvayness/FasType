﻿using FasType.Models.Linguistics.Grammars;
using System;
using System.Collections.Generic;
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

namespace FasType.Controls
{
    /// <summary>
    /// Interaction logic for PluralControl.xaml
    /// </summary>
    public partial class PluralControl : UserControl
    {
        public PluralControl()
        {
            InitializeComponent();
        }

        private void Prefix_TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is GrammarType gt)
                gt.Position = GrammarPosition.Prefix;
        }

        private void Postfix_TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is GrammarType gt)
                gt.Position = GrammarPosition.Postfix;
        }
    }
}
