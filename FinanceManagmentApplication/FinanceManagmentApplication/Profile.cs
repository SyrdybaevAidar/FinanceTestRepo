using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FinanceManagmentApplication.DAL.Entities;
using FinanceManagmentApplication.Models.CounterPartiesModel;
using FinanceManagmentApplication.Models.OperationModels;
using FinanceManagmentApplication.Models.PaymentType;
using FinanceManagmentApplication.Models.ProjectModels;
using FinanceManagmentApplication.Models.ScoreModel;
using FinanceManagmentApplication.Models.TransactionModels;
using FinanceManagmentApplication.Models.UserModels;

namespace FinanceManagmentApplication
{
    public class MapperProfile: Profile
    {
        public MapperProfile()
        {
            ProjectMapping();
            CounterPartyMapping();
            UserMapping();
            OperationMapping();
            TransactionMapping();
            ScoreTransaction();
            PaymentTypeMapper();
        }

        private void ProjectMapping()
        {
            CreateMap<ProjectCreateModel, Project>();

            CreateMap<Project, ProjectIndexModel>();

            CreateMap<ProjectIndexModel, Project>();
        }

        private void CounterPartyMapping()
        {
            CreateMap<CounterParty, CounterPartyIndexModel>();
            CreateMap<CounterPartyCreateModel, CounterParty>();
            CreateMap<CounterPartyEditModel, CounterParty>();
        }

        private void UserMapping()
        {
            CreateMap<User, UserIndexModel>();
        }
        private void OperationMapping()
        {
            CreateMap<Operation, OperationIndexModel>();
            CreateMap<Operation, OperationDetailsModel>();
            CreateMap<OperationCreateModel, Operation>();    
            CreateMap<OperationDetailsModel, Operation>();
           
        }

        private void TransactionMapping()
        {
            CreateMap<Transaction, TransactionIndexModel>()
                .ForMember(source => source.NameCounterParty1, target => target.MapFrom(src => src.Score1.CounterParty.Name))
                .ForMember(source => source.NameCounterParty2, target => target.MapFrom(src => src.Score2.CounterParty.Name))
                .ForMember(source => source.OperationName, target => target.MapFrom(src => src.Operation.Name))
                .ForMember(source => source.ProjectName, target => target.MapFrom(src => src.Project.Name));
            CreateMap<Transaction, TransactionDetailsModel>();
            CreateMap<TransactionCreateModel, Transaction>();
            CreateMap<TransactionEditModel, Transaction>();
            CreateMap<Transaction,TransactionEditModel>();
        }

        private void ScoreTransaction()
        {
            CreateMap<Score, ScoreIndexModel>()
                .ForMember(i => i.CounterPartyName, src => src.MapFrom(i => i.CounterParty.Name));
            CreateMap<Score, ScoreDetailsModel>();
            CreateMap<ScoreCreateModel, Score>();
            CreateMap<ScoreEditModel, Score>();
        }

        private void PaymentTypeMapper()
        {
            CreateMap<PaymentType, PaymentTypeIndexModel>();
        }
    }
}
