using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Data;

namespace Drinky.DAC
{
    public class NHTransaction : IDisposable
    {
        public ITransaction Transaction { get; protected set; }
        public ISession Session { get; protected set; }

        public NHTransaction()
        {
            this.Session = SessionManager.GetSession();
            if (Session.Transaction != null && Session.Transaction.IsActive)
                 this.Transaction = Session.BeginTransaction();
            else
                this.Transaction = Session.BeginTransaction();
        }

        public NHTransaction(IsolationLevel isolationLevel)
        {
            this.Session = SessionManager.GetSession();
            if (Session.Transaction != null && Session.Transaction.IsActive)
                this.Transaction = Session.BeginTransaction(isolationLevel);
            else
                this.Transaction = Session.BeginTransaction(isolationLevel);
        }

        public void Dispose()
        {

            if ((Transaction.WasCommitted == false) && (Transaction.WasRolledBack == false))
                Transaction.Rollback();
	        
        }

        public void Commit()
        {
            try
            {
                Transaction.Commit();
            }
            catch
            {
                Transaction.Rollback();
                throw;
            }
        }

        public void RollBack()
        {
            Session.Transaction.Rollback();
        }
    }

}
