namespace Dfe.ManageSchoolImprovement.Frontend.Services.Dtos
{
    public class TrustListResponse<TItem> where TItem : class
    {
        public IEnumerable<TItem>? Data { get; set; }
    }
}

