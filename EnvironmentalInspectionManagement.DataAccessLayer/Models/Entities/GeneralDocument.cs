namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}, Description: {Description}")]
    [Table(@"generaldocuments")]
    public class GeneralDocument : BaseEntity
    {
        public string ProtocolNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
