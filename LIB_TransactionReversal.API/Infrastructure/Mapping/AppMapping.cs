using AutoMapper;
using LIB_Documentmanagement.DAL.Entity;
using LIB_TransactionReversal.Application.Services;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_Usermanagement.Application.ViewModels;
using LIB_Usermanagement.DAL;
using LIB_Usermanagement.DAL.Entity.Account;


namespace LIB_Usermanagement.UI.Infrastructure
{
    public class AppMapping : Profile
    {
        public AppMapping()
        {
            CreateMap<User, UserRegistrationDto>()
               .ForMember(dest => dest.FullName,
                       opt => opt.MapFrom(src => src.Full_name))
               .ForMember(dest => dest.BranchName,
                       opt => opt.MapFrom(src => src.Branch_name))
               .ForMember(dest => dest.BranchCode,
                       opt => opt.MapFrom(src => src.Branch))
               .ReverseMap();

            CreateMap<UserUpdateDto, UserRegistrationDto>().ReverseMap();
            CreateMap<LibOutgoingTransaction, TransactionNotFoundAtEthSwitch>().ReverseMap();
            CreateMap<TransactionReversal, SuccessfullTransaction>().ReverseMap();
            CreateMap<TransactionReversal, TransactionNotFoundAtEthSwitch>().ReverseMap();
            CreateMap<ImportEthswichTransactionDto, TransactionNotFoundAtLIB>().ReverseMap();

            //CreateMap<EthswitchOutgoingTransactionImport, TransactionNotFoundAtLIB>()
            //   .ForMember(dest => dest.Amount,
            //           opt => opt.MapFrom(src => src.Amount))
            //   .ForMember(dest => dest.Reference,
            //           opt => opt.MapFrom(src => src.Refnum_F37))
               
            //   .ReverseMap();
            
            CreateMap<CBSEthswichOutgoingTransaction, TransactionNotFoundAtEthSwitch>()
               .ForMember(dest => dest.Amount,
                       opt => opt.MapFrom(src => src.amount))
               .ForMember(dest => dest.RefNo,
                       opt => opt.MapFrom(src => src.ref_no))
               .ForMember(dest => dest.DebitedAccountNumber,
                       opt => opt.MapFrom(src => src.account_debited))
               .ForMember(dest => dest.Branch,
                       opt => opt.MapFrom(src => src.branch))
               .ForMember(dest => dest.TransactionDate,
                       opt => opt.MapFrom(src => src.date_1))
                .ForMember(dest => dest.CreditedAccount,
                       opt => opt.MapFrom(src => src.account_credited))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<CBSEthswichIncomingTransaction, TransactionNotFoundAtEthSwitch>()
              .ForMember(dest => dest.Amount,
                      opt => opt.MapFrom(src => src.amount))
              .ForMember(dest => dest.RefNo,
                      opt => opt.MapFrom(src => src.ref_no))
              .ForMember(dest => dest.DebitedAccountNumber,
                      opt => opt.MapFrom(src => src.account_debited))
              .ForMember(dest => dest.Branch,
                      opt => opt.MapFrom(src => src.branch))
              .ForMember(dest => dest.TransactionDate,
                      opt => opt.MapFrom(src => src.date_1))
               .ForMember(dest => dest.CreditedAccount,
                      opt => opt.MapFrom(src => src.account_credited))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<CBSEthswichOutgoingTransaction, SuccessfullTransaction>()
               .ForMember(dest => dest.Amount,
                       opt => opt.MapFrom(src => src.amount))
               .ForMember(dest => dest.RefNo,
                       opt => opt.MapFrom(src => src.ref_no))
               .ForMember(dest => dest.DebitedAccountNumber,
                       opt => opt.MapFrom(src => src.account_debited))
               .ForMember(dest => dest.Branch,
                       opt => opt.MapFrom(src => src.branch))
               .ForMember(dest => dest.TransactionDate,
                       opt => opt.MapFrom(src => src.date_1))
                .ForMember(dest => dest.CreditedAccount,
                       opt => opt.MapFrom(src => src.account_credited))
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<CBSEthswichIncomingTransaction, SuccessfullTransaction>()
               .ForMember(dest => dest.Amount,
                       opt => opt.MapFrom(src => src.amount))
               .ForMember(dest => dest.RefNo,
                       opt => opt.MapFrom(src => src.ref_no))
               .ForMember(dest => dest.DebitedAccountNumber,
                       opt => opt.MapFrom(src => src.account_debited))
               .ForMember(dest => dest.Branch,
                       opt => opt.MapFrom(src => src.branch))
               .ForMember(dest => dest.TransactionDate,
                       opt => opt.MapFrom(src => src.date_1))
                .ForMember(dest => dest.CreditedAccount,
                       opt => opt.MapFrom(src => src.account_credited))
                 .ForMember(dest => dest.Id, opt => opt.Ignore())
               .ReverseMap();

            CreateMap<LibOutgoingTransaction, TransactionNotFoundAtLIB>()
              .ForMember(dest => dest.Amount,
                      opt => opt.MapFrom(src => src.Amount))
              .ForMember(dest => dest.RefNo,
                      opt => opt.MapFrom(src => src.Rrn))
              .ForMember(dest => dest.DebitedAccountNumber,
                      opt => opt.MapFrom(src => src.AccountNumber))
              .ForMember(dest => dest.Branch,
                      opt => opt.MapFrom(src => src.Branch))
              .ForMember(dest => dest.TransactionDate,
                      opt => opt.MapFrom(src => src.CreatedAt))
              .ForMember(dest => dest.CreditedAccount,
                      opt => opt.MapFrom(src => src.ReceiverAccount))
               .ForMember(dest => dest.Id, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<LibOutgoingTransaction, SuccessfullTransaction>().ReverseMap();
            CreateMap<LibOutgoingTransaction, LIBTransaction>().ReverseMap();
            CreateMap<ImportEthswichTransactionDto, EthswitchOutgoingTransactionImport>().ReverseMap();
            CreateMap<ImportEthswichTransactionDto, EthswitchIncommingTransactionImport>().ReverseMap();
            CreateMap<ImportEthswichTransactionDto, EthswitchInvalidDateTransaction>().ForMember(dest => dest.ExcelDate,
             opt => opt.MapFrom(src => src.TransactionDateFrom)).ReverseMap(); ;
            CreateMap<LIBIncommingTransactionResponse, LibIncommingTransaction>()
            .ForMember(dest => dest.BankName,
             opt => opt.MapFrom(src => src.name))
            .ForMember(dest => dest.Id, opt => opt.Ignore()).ReverseMap();
            
            CreateMap<LibIncommingTransaction, TransactionNotFoundAtLIB>()
            .ForMember(dest => dest.RefNo ,
             opt => opt.MapFrom(src => src.EthswitchRefNo))
            .ForMember(dest => dest.DebitedAccountNumber ,
             opt => opt.MapFrom(src => src.Account))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.TransactionDate,
             opt => opt.MapFrom(src => src.TimeStamp)).ReverseMap()
            .ForMember(dest => dest.BankName, opt => opt.Ignore()).ReverseMap();
        }
       
    }
}
 