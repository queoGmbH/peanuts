using System;
using System.Collections.Generic;

using Com.QueoFlow.Peanuts.Net.Core.Domain.Accounting;
using Com.QueoFlow.Peanuts.Net.Core.Service;

using Common.Logging;

using NHibernate;

using Quartz;

using Spring.Data.NHibernate.Support;
using Spring.Scheduling.Quartz;
using Spring.Transaction;
using Spring.Transaction.Interceptor;

namespace Com.QueoFlow.Peanuts.Net.Core.CronJobs {
    public class SettleAcceptedBills : QuartzJobObject {
        private readonly ILog _logger = LogManager.GetLogger(typeof(SettleAcceptedBills));

        /// <summary>
        ///    Liefert oder setzt den SessionFactory
        /// </summary>
        public ISessionFactory SessionFactory { get; set; }

        /// <summary>
        ///     Liefert oder setzt den BillService
        /// </summary>
        public IBillService BillService { get; set; }

        /// <summary>
        ///     Liefert oder setzt den IsAutomaticBillSettlingActive
        /// </summary>
        public bool IsAutomaticBillSettlingActive { get; set; }

        /// <summary>
        ///     Execute the actual job. The job data map will already have been
        ///     applied as object property values by execute. The contract is
        ///     exactly the same as for the standard Quartz execute method.
        /// </summary>
        /// <seealso cref="M:Spring.Scheduling.Quartz.QuartzJobObject.Execute(Quartz.IJobExecutionContext)" />
        protected override void ExecuteInternal(IJobExecutionContext context) {
            
            if (IsAutomaticBillSettlingActive) {
         
                try {

                    using (SessionScope sessionScope = new SessionScope(SessionFactory,true) ) {
                        IList<Bill> settledBills = BillService.SettleAllSettleableBills();
                        _logger.Info($"Es wurden {settledBills.Count} automatisch abgerechnet."); 
                        sessionScope.Close();
                    }
                   
                } catch (Exception e) {
                    _logger.Error(e);
                    
                }

            }
            
        }
    }
}