using System.Collections.Generic;
using System.Threading.Tasks;

namespace EasyOpc.WinService.Core.WorksService.Contract
{
    /// <summary>
    /// Source of works
    /// </summary>
    public interface IWorksService
    {
        /// <summary>
        /// Return list of works
        /// </summary>
        /// <returns>List of works</returns>
        Task<IEnumerable<IWork>> GetWorks();
    }
}
