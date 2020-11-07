﻿using FinanceManagmentApplication.DAL.Entities;
using FinanceManagmentApplication.Models.OperationModels;
using FinanceManagmentApplication.Models.ProjectModels;
using FinanceManagmentApplication.Models.ScoreModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceManagmentApplication.Models.TransactionModels
{
    public class TransactionEditModel
    {
        public int Id { get; set; }

        public DateTime TransactionDate { get; set; }

        public int Sum { get; set; }

        public List<OperationIndexModel> Operations { get; set; }

        public int OperationId { get; set; }

        public List<ProjectIndexModel> Projects { get; set; }

        public int ProjectId { get; set; }

        public List<ScoreIndexModel> Scores { get; set; }

        public int Score1Id { get; set; }

        public int CounterPartyId { get; set; }

        public int UserId { get; set; }

        public string Description { get; set; }
    }
}
