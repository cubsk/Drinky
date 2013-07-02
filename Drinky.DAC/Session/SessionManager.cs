using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Configuration;
using NHibernate;
using NHibernate.Event;
using NHibernate.UserTypes;
using System.Data.SqlClient;
using NHibernate.SqlTypes;
using System.Collections.Generic;
using NHibernate.Engine;
using System.Diagnostics;
using System.Data;
using System.Web;
using System.Configuration;

namespace Drinky.DAC
{

	/// <summary>
	/// Handles the creation of an NHibernate session.
	/// </summary>
	public static class SessionManager
	{

        static ISession ThreadLocalStorage { get; set; }

        static ISession CurrentSession
        {
            get
            {
                try
                {
                    if (HttpContext.Current != null)
                    {
                        // RUN THIS TO CHECK AND SEE IF THIS IS A WEB APPLICATION
                        return (ISession)HttpContext.Current.Items["YPO_SESSION"];
                    }
                    else
                    {
                        return ThreadLocalStorage;
                    }
                }
                catch (System.Web.HttpException ex)
                {
                    return ThreadLocalStorage;
                }



            }

            set
            {
                try
                {
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Items["YPO_SESSION"] = value;
                    }
                    else
                    {
                        ThreadLocalStorage = value;
                    }
                }
                catch (System.Web.HttpException)
                {
                    ThreadLocalStorage = value;
                }

            }

        }
        static ISessionFactory SessionFactory { get; set; }

        static SessionManager()
        {
            DateTime start = DateTime.Now;
            string connectionString = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("No connection string specified ");

            string displaySQL = ConfigurationManager.AppSettings["NHibernate.DisplaySQL"];
            bool shouldDisplaySQL = false;
            if (!string.IsNullOrEmpty(displaySQL) && string.Compare(displaySQL, "YES", true) == 0)
                shouldDisplaySQL = true;

            try
            {
             var config = Fluently.Configure();
             if(shouldDisplaySQL){
                config.Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Is(connectionString)).ShowSql().FormatSql());
             } else
             {
                 config.Database(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.Is(connectionString)));
             }
             config.Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHTransaction>());
                

             SessionFactory = config.BuildSessionFactory();
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to initialize session factory : " + ex.Message, ex);
            }
            DateTime end = DateTime.Now;
            double duration = end.Subtract(start).Milliseconds;

        }

        public static ISession GetSession()
        {
            var currentSession = CurrentSession;

            if (currentSession == null || !currentSession.IsOpen)
            {
                currentSession = SessionFactory.OpenSession();
                CurrentSession = currentSession;
            }
            return currentSession;
        }

        public static void CloseSession()
        {
            if (CurrentSession != null && CurrentSession.IsOpen)
            {
                CurrentSession.Close();
            }
            CurrentSession = null;
        }
    
        public static void ClearSession()
        {
            if (CurrentSession != null && CurrentSession.IsOpen)
            {
                CurrentSession.Clear();
            }

        }

        public static NHTransaction BeginTransaction()
        {
            return new NHTransaction();
        }

        /// <summary>
        /// Starts a new transaction
        /// </summary>
        /// <param name="isolationLevel">Isolation level for transaction</param>
        /// <returns></returns>
        public static NHTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new NHTransaction(isolationLevel);
        }
    }




}
