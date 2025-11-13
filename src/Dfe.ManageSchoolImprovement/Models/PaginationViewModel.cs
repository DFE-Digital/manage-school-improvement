using GovUK.Dfe.CoreLibs.Contracts.Academies.V4;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.ManageSchoolImprovement.Frontend.Models
{
    public class PaginationViewModel
    {
        public int PageSize { get; set; } = 10;

        public PagingResponse Paging { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public string PagePath { get; set; } = "/";
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => !string.IsNullOrWhiteSpace(Paging?.NextPageUrl);
        public int StartingPage => CurrentPage > 5 ? CurrentPage - 5 : 1;
        public int PreviousPage => CurrentPage - 1;
        public int NextPage => CurrentPage + 1;
        public int TotalPages => Paging.RecordCount % PageSize == 0
         ? Paging.RecordCount / PageSize
         : (Paging.RecordCount / PageSize) + 1;
    }
}
