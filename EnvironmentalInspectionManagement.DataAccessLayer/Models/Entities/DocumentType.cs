namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Entities
{
    #region Usings
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    [Table(@"documenttypes")]
    public class DocumentType : BaseEntity
    {
        public DocumentType()
        {
            Documents = new List<Document>();
        }

        public string Name { get; set; }

        public ICollection<Document> Documents { get; set; }
    }
}
