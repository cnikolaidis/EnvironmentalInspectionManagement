namespace EnvironmentalInspectionManagement
{
    #region Usings
    using System.Globalization;
    using System.Threading;
    using System.Windows;
    #endregion

    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var cultureInfo = new CultureInfo(Thread.CurrentThread.CurrentCulture.Name)
            {
                DateTimeFormat =
                {
                    ShortDatePattern = "dd/MM/yyyy",
                    DateSeparator = @"/"
                },
                NumberFormat =
                {
                    NumberDecimalSeparator = @",",
                    NumberGroupSeparator = @"."
                }
            };

            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
    }
}
