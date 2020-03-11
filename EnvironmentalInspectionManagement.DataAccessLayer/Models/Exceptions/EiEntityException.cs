namespace EnvironmentalInspectionManagement.DataAccessLayer.Models.Exceptions
{
    #region Usings
    using System;
    #endregion

    [Serializable]
    public class EiEntityException : Exception
    {
        public EiEntityException() { }

        public EiEntityException(string message) : base(message) { }

        public EiEntityException(string message, Exception inner) : base(message, inner) { }
    }
}
