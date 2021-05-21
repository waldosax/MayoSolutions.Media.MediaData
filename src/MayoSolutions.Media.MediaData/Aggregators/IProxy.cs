namespace MayoSolutions.Media.MediaData.Aggregators
{
    /// <summary>
    /// Proxy usage.
    /// </summary>
    public interface IProxy
    {
        /// <summary>Proxy host.</summary>
        string Host { get; set; }

        /// <summary>Proxy port.</summary>
        int? Port { get; set; }
    }
}