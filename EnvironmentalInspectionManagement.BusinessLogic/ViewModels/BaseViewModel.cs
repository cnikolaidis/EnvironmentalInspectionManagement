namespace EnvironmentalInspectionManagement.BusinessLogic.ViewModels
{
    #region Usings
    using DataAccessLayer.Models.Exceptions;
    using System.Runtime.CompilerServices;
    using System.Collections.Generic;
    using DataAccessLayer.Services;
    using System.Data.Entity.Core;
    using System.ComponentModel;
    using System.Security;
    using System.Linq;
    using Utilities;
    using System;
    using Core;
    #endregion

    public interface IBaseViewModel : INotifyPropertyChanged
    {
        bool OperationSuccess { get; set; }

        void RefreshInputs();
        void InvokeOperation(Action action);
        void InvokeOperation(Action<string> action, string param);
    }

    public abstract class BaseViewModel : IBaseViewModel
    {
        protected BaseViewModel()
        {
            Svc.LoggerService.Trace($"Navigated to: {GetType().UnderlyingSystemType.Name}");
        }

        public bool OperationSuccess { get; set; }

        public abstract void RefreshInputs();

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
                return false;

            oldValue = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public void InvokeOperation(Action action)
        {
            OperationSuccess = false;
            try
            {
                action.Invoke();
                Svc.LoggerService.Trace($"Operation Invoked: {action.Method.Name}");
                OperationSuccess = true;
            }
            catch (EiEntityException ex)
            {
                ex.Message.ErrMsg(@"Σφάλμα");
                Svc.LoggerService.Error(ex.Message, ex);
                OperationSuccess = false;
            }
            catch (EiViewException evx)
            {
                evx.Message.WarnMsg(@"Προσοχή");
                Svc.LoggerService.Warning(evx.Message, evx);
                OperationSuccess = false;
            }
            catch (SecurityException sx)
            {
                sx.Message.ErrMsg(@"Έλλειψη Δικαιωμάτων");
                Svc.LoggerService.Warning(sx.Message, sx);
                OperationSuccess = false;
            }
            catch (EntityException ex)
            {
                ex.Message.ErrMsg("Πρόβλημα Βάσης");
                Svc.LoggerService.Error(ex.Message, ex);
                OperationSuccess = false;
            }
            catch (Exception x)
            {
                x.ShowException();
                Svc.LoggerService.Error(x.Message, x);
                OperationSuccess = false;
            }
        }

        public void InvokeOperation(Action<string> action, string param)
        {
            OperationSuccess = false;
            try
            {
                action.Invoke(param);
                Svc.LoggerService.Trace($"Operation Invoked: {action.Method.Name} with parameter: {param}");
                OperationSuccess = true;
            }
            catch (EiEntityException ex)
            {
                ex.Message.ErrMsg(@"Σφάλμα");
                Svc.LoggerService.Error(ex.Message, ex);
                OperationSuccess = false;
            }
            catch (EiViewException evx)
            {
                evx.Message.WarnMsg(@"Προσοχή");
                Svc.LoggerService.Warning(evx.Message, evx);
                OperationSuccess = false;
            }
            catch (SecurityException sx)
            {
                sx.Message.ErrMsg(@"Έλλειψη Δικαιωμάτων");
                Svc.LoggerService.Warning(sx.Message, sx);
                OperationSuccess = false;
            }
            catch (EntityException ex)
            {
                ex.Message.ErrMsg("Πρόβλημα Βάσης");
                Svc.LoggerService.Error(ex.Message, ex);
                OperationSuccess = false;
            }
            catch (Exception x)
            {
                x.ShowException();
                Svc.LoggerService.Error(x.Message, x);
                OperationSuccess = false;
            }
        }

        public void InvokeOperationsParallel(ICollection<Action> actions)
        {
            OperationSuccess = false;
            try
            {
                var actionNames = actions.Select(x => x.Method.Name);
                System.Threading.Tasks.Parallel.Invoke(actions.ToArray());
                Svc.LoggerService.Trace($"Parallel Operations Invoked: {string.Join(@",", actionNames)}");
                OperationSuccess = true;
            }
            catch (EiEntityException ex)
            {
                ex.Message.ErrMsg(@"Σφάλμα");
                Svc.LoggerService.Error(ex.Message, ex);
                OperationSuccess = false;
            }
            catch (EiViewException evx)
            {
                evx.Message.WarnMsg(@"Προσοχή");
                Svc.LoggerService.Warning(evx.Message, evx);
                OperationSuccess = false;
            }
            catch (SecurityException sx)
            {
                sx.Message.ErrMsg(@"Έλλειψη Δικαιωμάτων");
                Svc.LoggerService.Warning(sx.Message, sx);
                OperationSuccess = false;
            }
            catch (EntityException ex)
            {
                ex.Message.ErrMsg("Πρόβλημα Βάσης");
                Svc.LoggerService.Error(ex.Message, ex);
                OperationSuccess = false;
            }
            catch (Exception x)
            {
                x.ShowException();
                Svc.LoggerService.Error(x.Message, x);
                OperationSuccess = false;
            }
        }
    }
}
