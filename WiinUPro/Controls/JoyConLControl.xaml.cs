﻿using System;
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
using Shared;
using SharpDX.DirectInput;

namespace WiinUPro
{
    /// <summary>
    /// Interaction logic for JoyConLControl.xaml
    /// </summary>
    public partial class JoyConLControl : UserControl, IJoyControl
    {
        public event Delegates.StringDel OnInputRightClick;
        public event Delegates.StringDel OnInputSelected;
        public event AssignmentCollection.AssignDelegate OnQuickAssign;

        public Guid AssociatedInstanceID { get; set; }

        public JoyConLControl()
        {
            InitializeComponent();
        }

        public void UpdateVisual(JoystickUpdate[] updates)
        {
            foreach (var update in updates)
            {
                switch (update.Offset)
                {
                    case JoystickOffset.Buttons0: leftBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons1: downBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons2: upBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons3: rightBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons4: slBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons5: srBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons8: minusBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons10: joyStickButton.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons13: shareBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons14: lBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.Buttons15: zlBtn.Opacity = update.Value > 0 ? 1 : 0; break;
                    case JoystickOffset.PointOfViewControllers0:
                        var margin = new Thickness(185, 599, 0, 0);
                        if (update.Value == -1)
                        {
                            // nothing
                        }
                        else if (update.Value == 0)
                        {
                            margin.Left += 30;
                        }
                        else if (update.Value > 0 && update.Value < 9000)
                        {
                            margin.Left += 20;
                            margin.Top += 20;
                        }
                        else if (update.Value == 9000)
                        {
                            margin.Top += 30;
                        }
                        else if (update.Value > 9000 && update.Value < 18000)
                        {
                            margin.Top += 20;
                            margin.Left -= 20;
                        }
                        else if (update.Value == 18000)
                        {
                            margin.Left -= 30;
                        }
                        else if (update.Value > 18000 && update.Value < 27000)
                        {
                            margin.Left -= 20;
                            margin.Top -= 20;
                        }
                        else if (update.Value == 27000)
                        {
                            margin.Top -= 30;
                        }
                        else if (update.Value > 27000)
                        {
                            margin.Top -= 20;
                            margin.Left += 20;
                        }

                        joyStick.Margin = margin;
                        joyStickButton.Margin = margin;
                        break;
                }
            }
        }

        private void OpenInput(object sender)
        {
            var element = sender as FrameworkElement;
            var tag = element == null ? "" : element.Tag as string;

            // Open input assignment window
            if (OnInputSelected != null && tag != null)
            {
                OnInputSelected(tag);
            }
        }

        private void Btn_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                OpenInput(sender);
            }
        }

        private void Btn_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;
            var tag = element == null ? "" : element.Tag as string;

            // Open Context menu
            if (OnInputRightClick != null && tag != null)
            {
                OnInputRightClick(tag);
            }
        }

        private void OpenContextMenu(object sender, MouseButtonEventArgs e)
        {
            var element = sender as FrameworkElement;

            if (element != null && element.ContextMenu != null)
            {
                element.ContextMenu.IsOpen = true;
            }
        }

        private void Axis_Click(object sender, RoutedEventArgs e)
        {
            OpenInput(sender);
        }
    }
}