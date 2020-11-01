using AutoMapper;
using FinanceManagmentApplication.DAL.Entities;
using FinanceManagmentApplication.DAL.Factories;
using FinanceManagmentApplication.Models.CounterPartiesModel;
using FinanceManagmentApplication.Models.ErrorModels;
using FinanceManagmentApplication.Models.PaymentType;
using FinanceManagmentApplication.Models.ScoreModel;
using FinanceManagmentApplication.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanceManagmentApplication.Services
{
    public class ScoreService : IScoreService
    {
        private IUnitOfWorkFactory UnitOfWorkFactory { get; }

        public ScoreService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            UnitOfWorkFactory = unitOfWorkFactory;
        }

        public async Task<List<ScoreIndexModel>> GetAll()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                return Mapper.Map<List<ScoreIndexModel>>(uow.Scores.GetFullScore());
            }
        }


        public async Task<Response> Create(ScoreCreateModel model)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Score = Mapper.Map<Score>(model);
                if (model == null)
                    return new Response { Status = StatusEnum.Error, Message="ничего на сервер не отправлено"};
                if(!uow.OperationTypes.Check(model.PaymentTypeId))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого типа операций" };
                if (!uow.CounterParties.Check(model.CounterPartyId))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого контрагента" };
                await uow.Scores.CreateAsync(Score);
                return new Response { Status = StatusEnum.Accept, Message = "Запрос прошел успешно" };

            }
        }

        public async Task<ScoreCreateModel> GetCreateModel()
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Score = new ScoreCreateModel();
                Score.CounterParties = Mapper.Map<List<CounterPartyIndexModel>>(await uow.CounterParties.GetAllAsync());
                Score.paymentTypes = Mapper.Map<List<PaymentTypeIndexModel>>(await uow.PaymentTypes.GetAllAsync());
                return Score;

            }
        }

        public async Task<ScoreEditModel> GetEditModel(int Id)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Score = uow.Scores.GetByIdAsync(Id);
                var Model = Mapper.Map<ScoreEditModel>(Score);
                Model.CounterParties = Mapper.Map<List<CounterPartyIndexModel>>(await uow.CounterParties.GetAllAsync());
                Model.paymentTypes = Mapper.Map<List<PaymentTypeIndexModel>>(await uow.PaymentTypes.GetAllAsync());
                return Model;

            }
        }

        public async Task<Response> Edit(ScoreEditModel model)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                var Score = Mapper.Map<Score>(model);
                if (model == null)
                    return new Response { Status = StatusEnum.Error, Message = "ничего на сервер не отправлено" };
                if (uow.PaymentTypes.Check(model.PaymentTypeId))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого типа операций" };
                if (uow.CounterParties.Check(model.CounterPartyId))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого контрагента" };
                if (!uow.Scores.Check(model.Id))
                    return new Response { Status = StatusEnum.Error, Message = "Нет такого счета!" };
                await uow.Scores.UpdateAsync(Score);
                return new Response { Status = StatusEnum.Accept, Message = "Запрос прошел успешно" };

            }
        }

        public async Task<Response> Delete(int Id)
        {
            using (var uow = UnitOfWorkFactory.Create())
            {
                if (!uow.Scores.Check(Id))
                    return new Response { Status = StatusEnum.Error, Message="Нет такого счета!"};
                if (uow.Transactions.CheckToScore(Id))
                { 
                    return new Response { Status = StatusEnum.Error, Message = "На этот счет есть транзакции, его нельзя удалить!" };
                }
                var Score = await uow.Scores.GetByIdAsync(Id);
                await uow.Scores.RemoveAsync(Score);
                return new Response { Status = StatusEnum.Error, Message = "Счет удален успешно" };
            }
        }

    }
}
