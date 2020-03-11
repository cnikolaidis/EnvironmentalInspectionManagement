namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}, Description: {Description}")]
    public class GeneralDocumentDto : BaseDto
    {
        public string ProtocolNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
