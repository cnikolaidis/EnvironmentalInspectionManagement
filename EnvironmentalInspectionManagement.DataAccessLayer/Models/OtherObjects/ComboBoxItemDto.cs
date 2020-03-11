namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.OtherObjects
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Text: {Text}, Value: {Value}")]
    public class ComboBoxItemDto
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public override string ToString() => Text;
    }
}
