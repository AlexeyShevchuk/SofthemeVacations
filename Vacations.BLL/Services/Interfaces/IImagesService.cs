using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Vacations.BLL.Services
{
    public interface IImagesService
    {
        Task<string> GetUrlAsync(string imgName);

        Task UploadAsync(IFormFile file);
    }
}