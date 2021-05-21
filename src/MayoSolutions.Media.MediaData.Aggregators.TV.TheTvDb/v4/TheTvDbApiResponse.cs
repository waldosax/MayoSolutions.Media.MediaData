namespace MayoSolutions.Media.MediaData.Aggregators.TV.TheTvDb.v4
{
    /// <summary>
    /// Generic rapper for every api response.
    /// </summary>
    /// <typeparam name="TDATA"></typeparam>
    internal class TheTvDbApiResponse<TDATA>
    {
        public string Status { get; set; }

        public string Message { get; set; }

        public TDATA Data { get; set; }
    }
    
}
