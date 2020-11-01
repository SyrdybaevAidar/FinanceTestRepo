using AutoMapper;
using FinanceManagmentApplication.DAL.Entities;
using FinanceManagmentApplication.DAL.Factories;
using FinanceManagmentApplication.Models.ErrorModels;
using FinanceManagmentApplication.Models.OperationModels;
using FinanceManagmentApplication.Models.ProjectModels;
using FinanceManagmentApplication.Models.ScoreModel;
using FinanceManagmentApplication.Models.TransactionModels;
using FinanceManagmentApplication.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinanceManagmentApplication.Services
{
    public class TransactionService: ITransactionService
    {
        private IUnitOfWorkFactory UnitOfWorkFactory { get; }
        private UserManager<User> UserManager { get; }

        public TransactionService(IUnitOfWorkFactory unitOfWorkFactory, UserManager<User> userManager)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
            UserManager = userManager;
        }

        public async Task<Response> Create(TransactionCreateModel model, ClaimsPrincipal User)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var _User = await UserManager.FindByNameAsync(User.Identity.Name);
                

                if (model.Score1Id == model.Score2Id)
                {
                    return new Response { Status = StatusEnum.Error, Message = "Нельзя провести с транзакцию на один и тот же счет." };
                }

                if (!uow.Scores.Check(model.Score1Id) || !uow.Scores.Check(model.Score2Id))
                {
                    return new Response { Status = StatusEnum.Error, Message = "В транзакции указан несуществующий счет" };
                }

                if (!uow.Operations.Check(model.OperationId))
                {
                    return new Response { Status = StatusEnum.Error, Message = "В транзакции указана несуществующая операция!" };
                }

                if (!uow.Projects.Check(model.ProjectId))
                {
                    return new Response { Status = StatusEnum.Error, Message = "В транзакции указан несуществующий проект!" };
                }

                model.UserId = _User.Id;
                var Transaction = Mapper.Map<Transaction>(model);
                await uow.Transactions.CreateAsync(Transaction);
                return new Response { Status = StatusEnum.Accept, Message = "Транзакция успешно создана." };

            }
        }

        public async Task<TransactionCreateModel> GetCreateModel()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Model = new TransactionCreateModel();
                Model.Operations = Mapper.Map<List<OperationIndexModel>>(await uow.Operations.GetAllAsync());
                Model.Projects = Mapper.Map<List<ProjectIndexModel>>(await uow.Projects.GetAllAsync());
                Model.Scores = Mapper.Map<List<ScoreIndexModel>>(await uow.Scores.GetAllAsync());
                return Model;
            }
        }

        public async Task<Response> Edit(TransactionEditModel model, ClaimsPrincipal User)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
             
                var Transaction = Mapper.Map<Transaction>(model);
                if (model == null)
                    return new Response { Status = StatusEnum.Error, Message = "ничего на сервер не отправлено" };
                if (!uow.Operations.Check(model.OperationId))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого типа операций" };
                if (!uow.Projects.Check(model.ProjectId))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого проекта!" };
                if (!uow.Scores.Check(model.Score1Id) || !uow.Scores.Check(model.Score2Id))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого счета!" };
                if(model.Score1Id == model.Score2Id)
                    return new Response{ Status = StatusEnum.Error, Message = "Нельзя проводить операцию на один и тот же счет!"  };
                var _User = await UserManager.FindByNameAsync(User.Identity.Name);
                Transaction.UserId = _User.Id;
                await uow.Transactions.UpdateAsync(Transaction);
                return new Response { Status = StatusEnum.Accept, Message = "Редактирование транзакции прошло успешно." };

            }
        }

        public async Task<TransactionEditModel> GetEditModel(int Id)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Transaction = await uow.Transactions.GetByIdAsync(Id);
                var Model = Mapper.Map<TransactionEditModel>(Transaction);
                Model.Operations = Mapper.Map<List<OperationIndexModel>>(await uow.Operations.GetAllAsync());
                Model.Projects = Mapper.Map<List<ProjectIndexModel>>(await uow.Projects.GetAllAsync());
                Model.Scores = Mapper.Map<List<ScoreIndexModel>>(await uow.Scores.GetAllAsync());
                return Model;
            }
        }

        public async Task<TransactionDetailsModel> GetDetailsModel(int Id)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Transaction = uow.Transactions.GetFullTransaction(Id);
                var Model = Mapper.Map<TransactionDetailsModel>(Transaction);
                return Model;
            }
        }




        public async Task<List<TransactionIndexModel>> GetAll()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Transactions =  uow.Transactions.GetTransactionsToIndex();
                var Models = new List<TransactionIndexModel>();
                foreach (var Transaction in Transactions)
                {
                    Transaction.Score1.CounterParty = await uow.CounterParties.GetByIdAsync(Transaction.Score1.CounterPartyId);
                    Transaction.Score2.CounterParty = await uow.CounterParties.GetByIdAsync(Transaction.Score2.CounterPartyId);
                    var Model = Mapper.Map<TransactionIndexModel>(Transaction);
                    Models.Add(Model);
                }

                return Models;
            }
        }


    }
}
