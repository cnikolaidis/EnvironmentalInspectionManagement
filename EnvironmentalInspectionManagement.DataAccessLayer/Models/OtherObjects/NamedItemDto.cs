namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.OtherObjects
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class NamedItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
