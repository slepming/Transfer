using System.Threading.Tasks;

namespace Transfer.Game.IO
{
    public interface ICanAcceptFile
    {
        /// <summary>
        /// Import files into temp a folder
        /// </summary>
        /// <param name="paths">Path to file in explorer</param>
        /// <returns></returns>
        Task Import(params string[] paths);

    }
}
