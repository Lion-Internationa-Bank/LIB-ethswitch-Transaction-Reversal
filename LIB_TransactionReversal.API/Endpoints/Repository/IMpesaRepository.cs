using DTO;
using System.Threading.Tasks;

namespace IRepository
{
    public interface IMpesaRepository
    {
        Task<MpesaResponseDTO> CreateMpesaTransfer(decimal amount, string phoneNo);
    }
}
