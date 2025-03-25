﻿// <auto-generated />
using System;
using Dfe.ManageSchoolImprovement.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Dfe.ManageSchoolImprovement.Infrastructure.Migrations
{
    [DbContext(typeof(RegionalImprovementForStandardsAndExcellenceContext))]
    partial class RegionalImprovementForStandardsAndExcellenceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.FundingHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("FinancialYear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FundingAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("FundingRounds")
                        .HasColumnType("int");

                    b.Property<string>("FundingType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<int>("ReadableId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReadableId"));

                    b.Property<int>("SupportProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupportProjectId");

                    b.ToTable("FundingHistories", "RISE");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                            {
                                ttb.UseHistoryTable("FundingHistoriesHistory", "RISE");
                                ttb
                                    .HasPeriodStart("PeriodStart")
                                    .HasColumnName("PeriodStart");
                                ttb
                                    .HasPeriodEnd("PeriodEnd")
                                    .HasColumnName("PeriodEnd");
                            }));
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdviserEmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("AdviserVisitDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("AskTheAdviserToSendYouTheirNotes")
                        .HasColumnType("bit");

                    b.Property<string>("AssignedDeliveryOfficerEmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AssignedDeliveryOfficerFullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("AttachRiseInfoToEmail")
                        .HasColumnType("bit");

                    b.Property<bool?>("CheckChoiceWithTrustRelationshipManagerOrLaLead")
                        .HasColumnType("bit");

                    b.Property<bool?>("CheckFinancialConcernsAtSupportingOrganisation")
                        .HasColumnType("bit");

                    b.Property<bool?>("CheckOrganisationHasCapacityAndWillingToProvideSupport")
                        .HasColumnType("bit");

                    b.Property<bool?>("CheckTheOrganisationHasAVendorAccount")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ContactedTheSchoolDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateAdviserAllocated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateConflictsOfInterestWereChecked")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateDueDiligenceCompleted")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateImprovementGrantOfferLetterSent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateNoteOfVisitSavedInSharePoint")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSupportOrganisationChosen")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSupportingOrganisationContactDetailsAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTeamContactedForConfirmingPlanningGrantOfferLetter")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTeamContactedForRequestingImprovementGrantOfferLetter")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTeamContactedForRequestingPlanningGrantOfferLetter")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateTemplatesSent")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DeletedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeletedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisapprovingImprovementPlanDecisionNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisapprovingSupportingOrganisationAppointmentNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("DiscussChoiceWithSfso")
                        .HasColumnType("bit");

                    b.Property<bool?>("FindSchoolEmailAddress")
                        .HasColumnType("bit");

                    b.Property<bool?>("FundingHistoryDetailsComplete")
                        .HasColumnType("bit");

                    b.Property<bool?>("GiveTheAdviserTheNoteOfVisitTemplate")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasAcceptedTargetedSupport")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasApprovedImprovementPlanDecision")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasCompleteAssessmentTemplate")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasConfirmedSupportingOrganisationAppointment")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasEmailedAgreedPlanToRegionalDirectorForApproval")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasReceivedFundingInThelastTwoYears")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasSavedImprovementPlanInSharePoint")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasSavedSchoolResponseinSharePoint")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasSchoolMatchedWithSupportingOrganisation")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasShareEmailTemplateWithAdviser")
                        .HasColumnType("bit");

                    b.Property<bool?>("HasTalkToAdviserAboutFindings")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("ImprovementPlanReceivedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("IntroductoryEmailSentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LocalAuthority")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NotMatchingSchoolWithSupportingOrgNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<bool?>("ReceiveCompletedConflictOfInterestForm")
                        .HasColumnType("bit");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RegionalDirectorAppointmentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RegionalDirectorDecisionDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RegionalDirectorImprovementPlanDecisionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("RemindAdviserToCopyRiseTeamWhenSentEmail")
                        .HasColumnType("bit");

                    b.Property<bool?>("ReviewImprovementPlanWithTeam")
                        .HasColumnType("bit");

                    b.Property<bool?>("SaveCompletedConflictOfinterestFormInSharePoint")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("SavedAssessmentTemplateInSharePointDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SchoolIsNotEligibleNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SchoolName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SchoolResponseDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SchoolUrn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("SchoolVisitDate")
                        .HasColumnType("datetime2");

                    b.Property<bool?>("SendConflictOfInterestFormToProposedAdviserAndTheSchool")
                        .HasColumnType("bit");

                    b.Property<bool?>("SendTheTemplateToTheSchoolsResponsibleBody")
                        .HasColumnType("bit");

                    b.Property<bool?>("SendTheTemplateToTheSupportingOrganisation")
                        .HasColumnType("bit");

                    b.Property<string>("SupportOrganisationIdNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportOrganisationName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SupportProjectStatus")
                        .HasColumnType("int");

                    b.Property<string>("SupportingOrganisationContactEmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SupportingOrganisationContactName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("UseTheNotificationLetterToCreateEmail")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("SupportProject", "RISE");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                            {
                                ttb.UseHistoryTable("SupportProjectHistory", "RISE");
                                ttb
                                    .HasPeriodStart("PeriodStart")
                                    .HasColumnName("PeriodStart");
                                ttb
                                    .HasPeriodEnd("PeriodEnd")
                                    .HasColumnName("PeriodEnd");
                            }));
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProjectContact", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Organisation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OtherRoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("SupportProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupportProjectId");

                    b.ToTable("SupportProjectContacts", "RISE");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                            {
                                ttb.UseHistoryTable("SupportProjectContactsHistory", "RISE");
                                ttb
                                    .HasPeriodStart("PeriodStart")
                                    .HasColumnName("PeriodStart");
                                ttb
                                    .HasPeriodEnd("PeriodEnd")
                                    .HasColumnName("PeriodEnd");
                            }));
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProjectNote", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastModifiedOn")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PeriodEnd")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodEnd");

                    b.Property<DateTime>("PeriodStart")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime2")
                        .HasColumnName("PeriodStart");

                    b.Property<int>("SupportProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SupportProjectId");

                    b.ToTable("SupportProjectNotes", "RISE");

                    b.ToTable(tb => tb.IsTemporal(ttb =>
                            {
                                ttb.UseHistoryTable("SupportProjectNotesHistory", "RISE");
                                ttb
                                    .HasPeriodStart("PeriodStart")
                                    .HasColumnName("PeriodStart");
                                ttb
                                    .HasPeriodEnd("PeriodEnd")
                                    .HasColumnName("PeriodEnd");
                            }));
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.FundingHistory", b =>
                {
                    b.HasOne("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProject", null)
                        .WithMany("FundingHistories")
                        .HasForeignKey("SupportProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProjectContact", b =>
                {
                    b.HasOne("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProject", null)
                        .WithMany("Contacts")
                        .HasForeignKey("SupportProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProjectNote", b =>
                {
                    b.HasOne("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProject", null)
                        .WithMany("Notes")
                        .HasForeignKey("SupportProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Dfe.ManageSchoolImprovement.Domain.Entities.SupportProject.SupportProject", b =>
                {
                    b.Navigation("Contacts");

                    b.Navigation("FundingHistories");

                    b.Navigation("Notes");
                });
#pragma warning restore 612, 618
        }
    }
}
