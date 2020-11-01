﻿using FinanceManagmentApplication.Models.ScoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagmentApplication.Services.Contracts
{
    public interface IScoreService
    {
        Task<Response> Create(ScoreCreateModel model);

        Task<List<ScoreIndexModel>> GetAll();

        Task<ScoreCreateModel> GetCreateModel();

        Task<ScoreEditModel> GetEditModel(int Id);

        Task<Response> Edit(ScoreEditModel model);
        Task<Response> Delete(int Id);
    }
}
