using FinanceManagmentApplication.DAL.Entities;
using FinanceManagmentApplication.DAL.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagmentApplication.DAL.Seed
{
    public class DataInitializer
    {
        public static async Task OperationTypeInitialize(IOperationTypeRepository repository)
        {
            var Values = new string[] { "Доход", "Расход" };

            var Types = await repository.GetAllAsync();

            foreach (var Value in Values)
            {
                if (Types.Where(i => i.Name == Value).ToList().Count == 0)
                {
                    await repository.CreateAsync(new OperationType { Name = Value });
                }
            }
        }

        public static async Task PaymentTypeInitialize(IPaymentTypeRepository Repository)
        {
            var Values = new string[] { "Наличный", "Безналичный" };

            var Types = await Repository.GetAllAsync();

            foreach (var Value in Values)
            {
                if (Types.Where(i => i.Name == Value).ToList().Count == 0)
                {
                    await Repository.CreateAsync(new PaymentType { Name = Value });
                }
            }
        }

        public static async Task UserInitialize(IUserRepository Repository)
        {
            
        }

        public static async Task CounterPartyInitialize(ICounterPartyRepository Repository)
        {
            await Repository.CreateAsync(new CounterParty { IsCompany = true, Name="ОсОО Таргет"});
            await Repository.CreateAsync(new CounterParty { IsCompany = false, Name = "Аяна Каракаевна" });
            await Repository.CreateAsync(new CounterParty { IsCompany = false, Name ="Admin", UserId = 1});
        }

        public static async Task ScoreInitialize(IScoreRepository Repository)
        {
            await Repository.CreateAsync(new Score { CounterPartyId = 1, PaymentTypeId = 2, ScoreNumber="123123123"});
            await Repository.CreateAsync(new Score { CounterPartyId = 1, PaymentTypeId = 1, ScoreNumber = "123123123" });
            await Repository.CreateAsync(new Score { CounterPartyId = 1, PaymentTypeId = 2, ScoreNumber = "123123123" });
        }

        public static async Task ProjectInitialize(IProjectRepository Repository)
        {
            var Values = new string[] { "Neolabs", "Neobis Studio", "Neobis club", "Прочерр"};

            var Projects = await Repository.GetAllAsync();

            foreach (var Value in Values)
            {
                if (Projects.Where(i => i.Name == Value).ToList().Count == 0)
                {
                    await Repository.CreateAsync(new Project { Name = Value });
                }
            }
        }

        public static async Task OperationInitialize(IOperationRepository Repository)
        {
            var IncomeValues = new string[]
                {
                    
                    "Доход от реализации услуг",
                    "Доход от реализации продукции",
                    "Членский взнос за поступление",
                    "Прочие доходы",
                    
                    
                };
            var CostValues = new string[]
                {
                    "Выдача заработной платы сотруднику",
                    "Расходы на офисные принадлежности",
                    "Расходы на коммунальные услуги",
                    "Расходы на покупку оборудования",
                    "Расходы на ремонт оборудования",
                    "Прочие расходы",
                    "Расходы на оплату аренды"
                };

            var Operations = await Repository.GetAllAsync();


            foreach (var Value in IncomeValues)
            {
                if (Operations.Where(i => i.Name == Value).ToList().Count == 0)
                {
                    await Repository.CreateAsync(new Operation { Name = Value,OperationTypeId = 1 });
                }
            }

            foreach (var Value in CostValues)
            {
                if (Operations.Where(i => i.Name == Value).ToList().Count == 0)
                {
                    await Repository.CreateAsync(new Operation { Name = Value, OperationTypeId = 2 });
                }
            }
        }
    }
}
