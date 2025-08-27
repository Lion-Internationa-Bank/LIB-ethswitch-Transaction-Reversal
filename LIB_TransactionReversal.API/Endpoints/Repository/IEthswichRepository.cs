using System.Threading.Tasks;
using DTO;

namespace IRepository
{
    public interface IEthswichRepository
    {
        Task<EthswichResponseDTO> CreateEthswichTransaction(decimal amount, string accountNo, string instId);
    }
}
