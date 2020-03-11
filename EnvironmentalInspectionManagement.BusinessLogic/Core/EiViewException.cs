namespace EnvironmentalInspectionManagement.BusinessLogic.Core
{
    #region Usings
    using System;
    #endregion

    [Serializable]
    public class EiViewException : Exception
    {
        public EiViewException() { }

        public EiViewException(string message) : base(message) { }

        public EiViewException(string message, Exception inner) : base(message, inner) { }
    }
}
