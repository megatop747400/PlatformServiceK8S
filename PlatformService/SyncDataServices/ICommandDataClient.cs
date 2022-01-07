using System.Threading.Tasks;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices
{
    public interface ICommandDataClient
    {
        Task<string> SendPlatformToCommand(PlatformReadDto platform);
    }
}