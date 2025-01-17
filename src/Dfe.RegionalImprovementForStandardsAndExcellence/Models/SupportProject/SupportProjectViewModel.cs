﻿using Dfe.RegionalImprovementForStandardsAndExcellence.Application.SupportProject.Models;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.ValueObjects;
using System.Runtime.InteropServices;
using Dfe.RegionalImprovementForStandardsAndExcellence.Domain.Entities.SupportProject;
using Microsoft.AspNetCore.Mvc;

namespace Dfe.RegionalImprovementForStandardsAndExcellence.Frontend.Models.SupportProject
{
    public class SupportProjectViewModel
    {
        public int Id { get; set; }

        public string SchoolName { get; set; }

        public string SchoolUrn { get; set; }
        
        public string LocalAuthority { get; set; }

        public string Region { get; set; }

        public string AssignedAdviserFullName { get; set; }
        
        public string AssignedAdviserEmailAddress { get; set; }
        
        public string QualityOfEducation { get; set; }
        
        public string BehaviourAndAttitudes { get; set; }
        
        public string PersonalDevelopment { get; set; }
        
        public string LeadershipAndManagement { get; set; }
        public string LastInspectionDate { get; set; }
        
        public bool? FindSchoolEmailAddress { get; private set; }
    
        public bool? UseTheNotificationLetterToCreateEmail { get; private set; }
    
        public bool? AttachRiseInfoToEmail { get; private set; }
    
        public DateTime? ContactedTheSchoolDate { get; private set; }
        
        public IEnumerable<SupportProjectNote> Notes { get; set; }

        public DateTime? SchoolResponseDate { get; set; }
         
        public bool? HasAcceeptedTargetedSupport { get; set; }
         
        public bool? HasSavedSchoolResponseinSharePoint { get; set; }

        public static SupportProjectViewModel Create(SupportProjectDto supportProjectDto)
        {
            return new SupportProjectViewModel()
            {
                Id = supportProjectDto.id,
                AssignedAdviserFullName = supportProjectDto.assignedAdviserFullName,
                AssignedAdviserEmailAddress = supportProjectDto.assignedAdviserEmailAddress,
                LocalAuthority = supportProjectDto.localAuthority,
                Region = supportProjectDto.region,
                SchoolName = supportProjectDto.schoolName,
                SchoolUrn = supportProjectDto.schoolUrn,
                Notes = supportProjectDto.notes,
                FindSchoolEmailAddress = supportProjectDto.FindSchoolEmailAddress,
                UseTheNotificationLetterToCreateEmail = supportProjectDto.UseTheNotificationLetterToCreateEmail,
                AttachRiseInfoToEmail = supportProjectDto.AttachRiseInfoToEmail,
                ContactedTheSchoolDate = supportProjectDto.ContactedTheSchoolDate,
                SchoolResponseDate = supportProjectDto.SchoolResponseDate,
                HasAcceeptedTargetedSupport = supportProjectDto.HasAcceeptedTargetedSupport,
                HasSavedSchoolResponseinSharePoint = supportProjectDto.HasSavedSchoolResponseinSharePoint
            };
        }
    }
}
