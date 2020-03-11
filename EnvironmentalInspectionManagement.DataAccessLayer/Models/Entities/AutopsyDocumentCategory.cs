namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"autopsydocumentcategories")]
    public class AutopsyDocumentCategory : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
