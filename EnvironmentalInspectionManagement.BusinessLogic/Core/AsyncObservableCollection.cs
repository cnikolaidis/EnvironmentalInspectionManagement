namespace EnvironmentalInspectionManagement.BusinessLogic.Core
{
    #region Usings
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    #endregion

    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private readonly SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        #region Constructor
        public AsyncObservableCollection() { }

        public AsyncObservableCollection(IEnumerable<T> list) : base(list) { }
        #endregion

        #region Event Methods
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext) RaiseCollectionChanged(e);
            else _synchronizationContext.Send(RaiseCollectionChanged, e);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext) RaisePropertyChanged(e);
            else _synchronizationContext.Send(RaisePropertyChanged, e);
        }
        #endregion

        private void RaiseCollectionChanged(object param) => base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);

        private void RaisePropertyChanged(object param) => base.OnPropertyChanged((PropertyChangedEventArgs)param);
    }
}
