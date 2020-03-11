namespace EnvironmentalInspectionManagement.Presentation.Helpers
{
    #region Usings
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows;
    using System.Linq;
    using System;
    #endregion

    public static class NumericInputHelper
    {
        private static readonly IEnumerable<char> AllowedNumericCharacters = new List<char> { '.', '-' };

        public static DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(@"IsEnabled", typeof(bool), typeof(NumericInputHelper), new UIPropertyMetadata(false, OnValueChanged));

        private static void OnTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text.Where(x => !AllowedNumericCharacters.Contains(x)).Any(x => !char.IsDigit(x)))
                e.Handled = true;
        }

        private static void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private static void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(DataFormats.Text))
            {
                var text = Convert.ToString(e.DataObject.GetDataPresent(DataFormats.Text)).Trim();
                if (text.Where(x => !AllowedNumericCharacters.Contains(x)).Any(x => !char.IsDigit(x)))
                    e.CancelCommand();
            }
            else e.CancelCommand();
        }

        private static void OnValueChanged(DependencyObject dpo, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = dpo as Control;

            if (ctrl == null)
                return;

            if (e.NewValue is bool && (bool) e.NewValue)
            {
                ctrl.PreviewTextInput += OnTextInput;
                ctrl.PreviewKeyDown += OnPreviewKeyDown;
                DataObject.AddPastingHandler(ctrl, OnPaste);
            }
            else
            {
                ctrl.PreviewTextInput -= OnTextInput;
                ctrl.PreviewKeyDown -= OnPreviewKeyDown;
                DataObject.RemovePastingHandler(ctrl, OnPaste);
            }
        }

        public static bool GetIsEnabled(Control ctrl) => (bool) ctrl.GetValue(IsEnabledProperty);

        public static void SetIsEnabled(Control ctrl, bool value) => ctrl.SetValue(IsEnabledProperty, value);
    }
}
