using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.UpdateSupportProject;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Commands.Watchlist;
using Dfe.ManageSchoolImprovement.Application.SupportProject.Queries;
using Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject;
using Dfe.ManageSchoolImprovement.Domain.ValueObjects;
using Dfe.ManageSchoolImprovement.Frontend.Authorization;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.AssignDeliveryOfficer;


public class IndexModel(IUserRepository userRepository, 
   ISupportProjectQueryService supportProjectQueryService, 
   IWatchlistQueryService watchlistQueryService, 
   IMediator _mediator,
   IHostEnvironment environment,
   IHttpContextAccessor httpContextAccessor,
   IConfiguration configuration) : PageModel
{
   public string SchoolName { get; private set; }
   public int Id { get; set; }
   public IEnumerable<User> DeliveryOfficers { get; set; }
   public string SelectedDeliveryOfficer { get; set; }
   private string DeliveryOfficerEmail { get; set; }

   public async Task<IActionResult> OnGet(int id ,CancellationToken cancellationToken)
   {
      var projectResponse = await supportProjectQueryService.GetSupportProject(id,cancellationToken);
      Id = id;
      SchoolName = projectResponse.Value?.SchoolName!;
      SelectedDeliveryOfficer = projectResponse.Value?.AssignedDeliveryOfficerFullName!;

      DeliveryOfficers = await userRepository.GetAllUsers();

      return Page();
   }

   public async Task<IActionResult> OnPost(int id, string selectedName, string userInput, CancellationToken cancellationToken)
   {
      var projectResponse = await supportProjectQueryService.GetSupportProject(id, cancellationToken);
      
      SupportProjectId supportProjectId = new(projectResponse.Value!.Id);
      
      if (string.IsNullOrWhiteSpace(userInput))
      {
         selectedName = string.Empty;
      }

      if (!string.IsNullOrEmpty(selectedName))
      {
         IEnumerable<User> deliveryOfficers = await userRepository.GetAllUsers();

         var assignedDeliveryOfficer = deliveryOfficers.SingleOrDefault(u => u.FullName == selectedName);
         var initialDeliveryOfficerAssigned = true;
            
         var request = new SetDeliveryOfficerCommand(supportProjectId, assignedDeliveryOfficer?.FullName!, assignedDeliveryOfficer?.EmailAddress!, initialDeliveryOfficerAssigned);
         await _mediator.Send(request);
            
         if ((environment.IsDevelopment() || environment.IsEnvironment("Test")) && HeaderRequirementHandler.ClientSecretHeaderValid(environment, httpContextAccessor, configuration))
         {
            DeliveryOfficerEmail = "Cypress";
         }
         else
         {
            DeliveryOfficerEmail = assignedDeliveryOfficer?.EmailAddress!;
         }
            
         var watchlistSupportProjects =
            await watchlistQueryService.GetAllSchoolsForUser(DeliveryOfficerEmail, cancellationToken);
            
         if (watchlistSupportProjects.Value == null || watchlistSupportProjects.Value.All(s => s.SupportProjectId != supportProjectId))
         {

            var watchlistRequest =
               new AddSchoolToWatchlistCommand(supportProjectId, DeliveryOfficerEmail);
            await _mediator.Send(watchlistRequest);
         }

         TempData["deliveryOfficerAssigned"] = true;
      }

      return RedirectToPage(Links.TaskList.Index.Page, new { id });
   }
}
