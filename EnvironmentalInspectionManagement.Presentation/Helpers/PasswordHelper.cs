namespace EnvironmentalInspectionManagement.Presentation.Helpers
{
    #region Usings
    using System.Windows.Controls;
    using System.Windows;
    #endregion

    public static class PasswordBoxAssistant
    {
        private static readonly DependencyProperty UpdatingPassword = DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false));
        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached("BindPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, OnBindPasswordChanged));
        public static readonly DependencyProperty BoundPassword = DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        private static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            var passBox = sender as PasswordBox;

            if (passBox == null)
                return;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(passBox, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(passBox, passBox.Password);
            SetUpdatingPassword(passBox, false);
        }

        private static void SetUpdatingPassword(DependencyObject dp, bool value) => dp.SetValue(UpdatingPassword, value);

        private static bool GetUpdatingPassword(DependencyObject dp) => (bool)dp.GetValue(UpdatingPassword);

        private static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event
            var passBox = dp as PasswordBox;

            if (passBox == null)
                return;

            var wasBound = (bool)e.OldValue;
            var needToBind = (bool)e.NewValue;

            if (wasBound)
                passBox.PasswordChanged -= HandlePasswordChanged;

            if (needToBind)
                passBox.PasswordChanged += HandlePasswordChanged;
        }
        
        public static void SetBindPassword(DependencyObject dp, bool value) => dp.SetValue(BindPassword, value);

        public static bool GetBindPassword(DependencyObject dp) => (bool)dp.GetValue(BindPassword);

        private static void OnBoundPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var passBox = dp as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (dp == null || !GetBindPassword(dp) || passBox == null)
                return;

            // avoid recursive updating by ignoring the box's changed event
            passBox.PasswordChanged -= HandlePasswordChanged;
            var newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(passBox))
                passBox.Password = newPassword;

            passBox.PasswordChanged += HandlePasswordChanged;
        }

        public static void SetBoundPassword(DependencyObject dp, string value) => dp.SetValue(BoundPassword, value);

        public static string GetBoundPassword(DependencyObject dp) => (string)dp.GetValue(BoundPassword);
    }
}
