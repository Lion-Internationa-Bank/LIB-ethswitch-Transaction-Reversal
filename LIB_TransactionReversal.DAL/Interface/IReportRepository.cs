using LIB_Usermanagement.DAL.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace LIB_Usermanagement.DAL.Repository
{
   public interface IReportRepository
    {
        List<string> GetAccessions();
       
    }
}
