namespace EasyOpc.WinService.Core.Configuration.Contract
{
    /// <summary>
    /// Configuration module contract
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Returns the value of the setting by key 
        /// </summary>
        /// <param name="key">Setting key</param>
        /// <returns>Value</returns>
        string this[string key] { get; }
    }
}
