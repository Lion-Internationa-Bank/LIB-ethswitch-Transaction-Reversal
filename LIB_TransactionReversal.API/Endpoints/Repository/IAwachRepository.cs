using DTO;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IAwachRepository
    {
        Task<AwachResponseDTO> CreateAwachTransfer(decimal amount, string accountNo);
    }
}
