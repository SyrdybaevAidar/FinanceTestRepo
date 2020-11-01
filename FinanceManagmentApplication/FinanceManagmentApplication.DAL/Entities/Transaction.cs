﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagmentApplication.DAL.Entities
{
    public class Transaction: IEntity
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int Sum { get; set; }

        public Operation Operation { get; set; }

        public int OperationId { get; set; }

        public Project Project { get; set; }

        public int ProjectId { get; set; }

        public Score Score1 { get; set; }

        public int Score1Id { get; set; }

        public virtual Score Score2 { get; set; }

        public int Score2Id { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }
    }
}
