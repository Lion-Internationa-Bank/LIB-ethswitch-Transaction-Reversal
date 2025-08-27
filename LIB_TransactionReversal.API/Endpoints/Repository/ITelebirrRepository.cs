using DTO;
using System.Threading.Tasks;

namespace IRepository
{
    public interface ITelebirrRepository
    {
        Task<TelebirrResponseDTO> CreateTelebirrTransaction(decimal amount, string phoneNo);
    }
}
