using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LIB_TransactionReversal.Application.Services;
using LIB_TransactionReversal.DAL.DTO;
using LIB_TransactionReversal.DAL.Entity;
using LIB_TransactionReversal.DAL.Interface;
using LIB_Usermanagement.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace LIB_TransactionReversal.Infra.Data.Repository
{

    public class LibOutgoingTransactionRepository : ILibOutgoingTransactionRepository
    {
        private readonly TrasactionReversalDbContext _context;
        private readonly IMapper _mapper;
        public LibOutgoingTransactionRepository(TrasactionReversalDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task GetBatchEthswitchOutgoingTransaction()
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback =
            (sender, certificate, chain, sslPolicyErrors) => true;

                libtransResponse libtrans = new libtransResponse();

                string URL = "http://10.1.10.90:7000/api/lib/v1/checkDailyLimit";
                // string URL = _configuration["EndPointUrl:CheckAccoutUrl"] + "/api/lib/v1/checktransactionposibility";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
                request.Method = "GET";

                WebResponse webResponse = request.GetResponse();
                using (Stream webStream = webResponse.GetResponseStream() ?? Stream.Null)
                using (StreamReader responseReader = new StreamReader(webStream))
                {
                    string response = responseReader.ReadToEnd();
                    libtrans.transactions = new List<LIBTransaction>();
                    libtrans = JsonConvert.DeserializeObject<libtransResponse>(response);
                    List<LibOutgoingTransaction> libOutgoingTransaction = new List<LibOutgoingTransaction>();
                    libOutgoingTransaction = _mapper.Map<List<LibOutgoingTransaction>>(libtrans.transactions);
                    DateTime LastDate = libOutgoingTransaction.Min(p => p.CreatedAt);
                    LastDate = LastDate.AddDays(-3);
                    var existTrans = await _context.LibOutgoingTransaction.Where(p => p.CreatedAt >= LastDate).ToListAsync();
                    List<string> existtranid = existTrans.Select(p => p.Rrn).ToList();
                    libOutgoingTransaction = libOutgoingTransaction.Where(p => !existtranid.Contains(p.Rrn)).ToList();
                    libOutgoingTransaction.ForEach(p =>
                    {
                        p.ImportedDate = DateTime.Now;
                    });
                    await _context.LibOutgoingTransaction.AddRangeAsync(libOutgoingTransaction);
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
