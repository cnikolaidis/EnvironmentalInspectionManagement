﻿namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Dtos
{
    #region Usings
    using System.Diagnostics;
    #endregion

    [DebuggerDisplay("Id: {Id}, Name: {Name}")]
    public class LegalEntityCategoryDto : BaseDto
    {
        public string Name { get; set; }
    }
}
