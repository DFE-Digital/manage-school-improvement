//using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Interfaces.Repositories;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Infrastructure.Database;
//using Microsoft.EntityFrameworkCore;
//using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Entities.Schools;

//namespace Dfe.RegionalImprovementForStandardsAndExcellence.Infrastructure.Repositories
//{
//    /// <summary>
//    /// Please note this is an example of a specific repository for the aggregate root
//    /// the following methods and queries can be achieved by Generic Repository's Find and Fetch methods 
//    /// </summary>
//    /// <param name="context"></param>
//    public class SchoolRepository(RegionalImprovementForStandardsAndExcellenceContext context) : ISchoolRepository
//    {
//        public async Task<School?> GetPrincipalBySchoolAsync(string schoolName, CancellationToken cancellationToken)
//        {
//            return await context.Schools
//                .AsNoTracking()
//                .Include(c => c.PrincipalDetails)
//                .Where(c => c.SchoolName == schoolName
//                            && c.PrincipalDetails.TypeId == 1
//                            && !c.EndDate.HasValue)
//                .FirstOrDefaultAsync(cancellationToken);
//        }

//        public IQueryable<School> GetPrincipalsBySchoolsQueryable(List<string> schoolNames)
//        {
//            return context.Schools
//                .AsNoTracking()
//                .Include(c => c.PrincipalDetails) 
//                .Where(c => schoolNames.Contains(c.SchoolName)
//                            && c.PrincipalDetails.TypeId == 1
//                            && !c.EndDate.HasValue);
//        }
//    }
//}
