using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LIB_TransactionReversal.Infra.Data.Repository
{
    public class LibIncomingTransactionRepository: ILibIncomingTransactionRepository
    {
        private readonly TrasactionReversalDbContext _context;
        private readonly IMapper _mapper;
        public LibIncomingTransactionRepository(TrasactionReversalDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task GetBatchEthswitchIncommingTransaction()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, sslPolicyErrors) => true;

                List<LIBIncommingTransactionResponse> libtrans = new List<LIBIncommingTransactionResponse>();

                string URL = "http://10.1.10.90:5000/getEthswitchTransactionsDestination";
                // string URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/checktransactionposibility";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "GET";

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    libtrans = JsonConvert.DeserializeObject<List<LIBIncommingTransactionResponse>>(response);
                    List<LibIncommingTransaction> libIncommingTransaction = new List<LibIncommingTransaction>();
                    libIncommingTransaction = _mapper.Map<List<LibIncommingTransaction>>(libtrans);
                    DateTime LastDate = libIncommingTransaction.Min(p => p.TimeStamp);
                    LastDate = LastDate.AddDays(-3);
                    var existTrans = await _context.LibIncommingTransaction.Where(p => p.TimeStamp >= LastDate).ToListAsync();
                    List<string> existtranid = existTrans.Select(p => p.EthswitchRefNo).ToList();
                    libIncommingTransaction = libIncommingTransaction.Where(p => !existtranid.Contains(p.EthswitchRefNo)).ToList();
                    libIncommingTransaction.ForEach(p =>
                    {
                        p.ImportedDate = DateTime.Now;
                    });
                    await _context.LibIncommingTransaction.AddRangeAsync(libIncommingTransaction);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
