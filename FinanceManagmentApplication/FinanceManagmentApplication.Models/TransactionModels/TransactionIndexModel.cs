using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagmentApplication.Models.TransactionModels
{
    public class TransactionIndexModel
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int Sum { get; set; }

        public string OperationName { get; set; }

        public string ProjectName { get; set; }

        public string NameCounterParty1 { get ; set; }

        public string NameCounterParty2 { get; set; }

    }
}
