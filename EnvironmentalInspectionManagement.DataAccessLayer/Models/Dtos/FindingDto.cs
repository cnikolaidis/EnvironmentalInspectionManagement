namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Description: {Description}")]
    public class FindingDto : BaseDto
    {
        public int DocumentId { get; set; }
        public string Description { get; set; }
    }
}
