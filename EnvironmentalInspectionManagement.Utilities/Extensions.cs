namespace EnvironmentalInspectionManagement.Utilities
{
    #region Usings
    using System.Security.Cryptography;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Forms;
    using UtilityForms;
    using System.Text;
    using System.Linq;
    using System;
    #endregion

    public static class Extensions
    {
        /// <summary>
        /// Convert object to Decimal
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns>Decimal</returns>
        public static decimal ToDecimal(this object inObj)
        {
            if (inObj == null) return 0;
            if (inObj is bool)
                return (bool)inObj ? 1 : 0;
            var ttemp = inObj.ToString();
            if (ttemp == "" || ttemp == " " || ttemp.Trim() == "0" || ttemp.ToUpper() == "NONE")
                return 0;

            var s = ttemp.Replace(@".", string.Empty).Replace(",", ".");
            var ci = new CultureInfo("en-US");
            decimal val;
            var ret = decimal.TryParse(s, NumberStyles.Number, ci, out val);

            if (ret) return val;

            var dbl1 = decimal.TryParse(ttemp, NumberStyles.Float, new CultureInfo("el-GR"), out val);
            if (dbl1) return val;
            return 0;
        }

        /// <summary>
        /// Converts object to Int32.
        /// </summary>
        /// <param name="inObj">Get an object</param>
        /// <returns>Int32</returns>
        public static int ToInt32(this object inObj)
        {
            if (inObj == null) return 0;

            if (inObj is bool)
            {
                return (bool)inObj ? 1 : 0;
            }
            int tmpout;
            var ttemp = inObj.ToString();

            if (ttemp == "" || ttemp == " " || ttemp.Trim() == "0")
                return 0;

            ttemp = ttemp.Replace(",", ".");
            if (ttemp.Contains(".")) ttemp = ttemp.Substring(0, ttemp.IndexOf(".", StringComparison.Ordinal));

            var dbl = Int32.TryParse(ttemp, NumberStyles.Float, new CultureInfo("en-US"), out tmpout);
            if (dbl) return tmpout;

            var dbl1 = Int32.TryParse(ttemp, NumberStyles.Float, new CultureInfo("ru-RU"), out tmpout);
            if (dbl1) return tmpout;

            return 0;
        }

        /// <summary>
        /// Check if String is null or empty
        /// </summary>
        /// <param name="inStr"></param>
        /// <returns>Expression is true or false</returns>
        public static bool IsEmpty(this string inStr)
        {
            return (string.IsNullOrEmpty(inStr) || string.IsNullOrWhiteSpace(inStr));
        }

        /// <summary>
        /// Check if Object is null
        /// </summary>
        /// <param name="inObj"></param>
        /// <returns>Expression is true or false</returns>
        public static bool IsNull(this object inObj)
        {
            return (inObj == null);
        }

        /// <summary>
        /// Show Warning Message
        /// </summary>
        /// <param name="msgStr"></param>
        /// <param name="msgCaption"></param>
        public static void WarnMsg(this string msgStr, string msgCaption = @"Warning")
        {
            MessageBox.Show(msgStr, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Show Error Message
        /// </summary>
        /// <param name="msgStr"></param>
        /// <param name="msgCaption"></param>
        public static void ErrMsg(this string msgStr, string msgCaption = @"Error")
        {
            MessageBox.Show(msgStr, msgCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Show Simple Question Message
        /// </summary>
        /// <param name="msgStr"></param>
        /// <param name="msgCaption"></param>
        /// <returns>User response</returns>
        public static bool SimpleQMsg(this string msgStr, string msgCaption = @"Info")
        {
            DialogResult diagResult = MessageBox.Show(msgStr, msgCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            return (diagResult == DialogResult.Yes);
        }

        /// <summary>
        /// Inform User for Exception in a Grid Form
        /// </summary>
        /// <param name="x">Exception</param>
        public static void ShowException(this Exception x)
        {
            List<Exception> xList = new List<Exception>() { x };

            while (x.InnerException != null)
            {
                x = x.InnerException;
                xList.Add(x);
            }

            new MessageGridForm()
            {
                FormCaption = @"Exception Error",
                GridDataSource = xList.Select(exception => new
                {
                    Type = $"{exception.GetType()}",
                    Message = $"{exception.Message}",
                    Stack = $"{exception.StackTrace}",
                    Source = $"{exception.Source}"
                }).Cast<object>().ToList()
            }.ShowDialog();
        }

        /// <summary>
        /// Create hash with SHA256 algorithm
        /// </summary>
        /// <returns>Hashed String</returns>
        public static string ToSha256(this string cString)
        {
            string resultHash;

            using (var sha256Hasher = SHA256.Create())
            {
                resultHash = string.Concat(
                    sha256Hasher.ComputeHash(Encoding.UTF8.GetBytes(cString))
                        .Select(x => x.ToString(@"x2")));
            }

            return resultHash;
        }
    }
}
