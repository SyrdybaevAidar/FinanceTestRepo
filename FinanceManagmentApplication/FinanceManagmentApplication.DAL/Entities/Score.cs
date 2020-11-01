using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FinanceManagmentApplication.DAL.Entities
{
    public class Score: IEntity
    {
        public int Id { get; set; }

        public string ScoreNumber { get; set; }

        public PaymentType PaymentType { get; set; }

        public int PaymentTypeId { get; set; }

        public CounterParty CounterParty { get; set; }

        public int CounterPartyId { get; set; }

        public int Money { get; set; }

        [InverseProperty("Score1")]
        public virtual IEnumerable<Transaction> Transactions1 { get; set; }

        [InverseProperty("Score2")]
        public virtual IEnumerable<Transaction> Transactions2 { get; set; }

    }
}
