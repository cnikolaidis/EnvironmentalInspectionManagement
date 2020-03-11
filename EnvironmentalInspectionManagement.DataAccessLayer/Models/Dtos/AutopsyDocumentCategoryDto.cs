namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class AutopsyDocumentCategoryDto : BaseDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
