using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using exp.Infrastructure.Entities;

namespace exp.Infrastructure.Context;

public partial class SurveyDBContext : DbContext
{
    public SurveyDBContext(DbContextOptions<SurveyDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivitySector> ActivitySectors { get; set; }

    public virtual DbSet<AnswersPrompt> AnswersPrompts { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DeliveryCountry> DeliveryCountries { get; set; }

    public virtual DbSet<Organisation> Organisations { get; set; }

    public virtual DbSet<OrganisationSector> OrganisationSectors { get; set; }

    public virtual DbSet<OrganisationSurvey> OrganisationSurveys { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionAttachment> QuestionAttachments { get; set; }

    public virtual DbSet<QuestionType> QuestionTypes { get; set; }

    public virtual DbSet<Survey> Surveys { get; set; }

    public virtual DbSet<SurveyResponse> SurveyResponses { get; set; }

    public virtual DbSet<SurveySection> SurveySections { get; set; }

    public virtual DbSet<VwOrganisationResult> VwOrganisationResults { get; set; }

    public virtual DbSet<VwOrgannisationsReport> VwOrgannisationsReports { get; set; }

    public virtual DbSet<VwSectionReport> VwSectionReports { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivitySector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activity_sectors_pkey");

            entity.ToTable("activity_sectors", "org");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivitySectorId).HasColumnName("activity_sector_id");
            entity.Property(e => e.Exemples).HasColumnName("exemples");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");

            entity.HasOne(d => d.ActivitySectorNavigation).WithMany(p => p.InverseActivitySectorNavigation)
                .HasForeignKey(d => d.ActivitySectorId)
                .HasConstraintName("activity_sectors_activity_sector_id_fkey");
        });

        modelBuilder.Entity<AnswersPrompt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("answers_prompts_pkey");

            entity.ToTable("answers_prompts", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Answers).HasColumnName("answers");
            entity.Property(e => e.Comment)
                .HasMaxLength(500)
                .HasColumnName("comment");
            entity.Property(e => e.Prompt).HasColumnName("prompt");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");

            entity.HasOne(d => d.Question).WithMany(p => p.AnswersPrompts)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("answers_prompts_question_id_fkey");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.ToTable("AspNetRoles", "auth");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.ToTable("AspNetRoleClaims", "auth");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.ToTable("AspNetUsers", "auth");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.ToTable("AspNetUserClaims", "auth");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.ToTable("AspNetUserLogins", "auth");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.ToTable("AspNetUserRoles", "auth");

            entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.RoleId);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserRoles).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.ToTable("AspNetUserTokens", "auth");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("attachments_pkey");

            entity.ToTable("attachments", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("countries_pkey");

            entity.ToTable("countries", "org");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsEuropean).HasColumnName("is_european");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<DeliveryCountry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("delivery_countries_pkey");

            entity.ToTable("delivery_countries", "org");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");

            entity.HasOne(d => d.Country).WithMany(p => p.DeliveryCountries)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("delivery_countries_country_id_fkey");

            entity.HasOne(d => d.Organisation).WithMany(p => p.DeliveryCountries)
                .HasForeignKey(d => d.OrganisationId)
                .HasConstraintName("delivery_countries_organisation_id_fkey");
        });

        modelBuilder.Entity<Organisation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organisations_pkey");

            entity.ToTable("organisations", "org");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessCode).HasColumnName("access_code");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.CompanySize)
                .HasMaxLength(200)
                .HasColumnName("company_size");
            entity.Property(e => e.ConfirmEmail).HasColumnName("confirm_email");
            entity.Property(e => e.ConfirmEmailCode)
                .HasMaxLength(100)
                .HasColumnName("confirm_email_code");
            entity.Property(e => e.ConfirmRegister).HasColumnName("confirm_register");
            entity.Property(e => e.ContactPersonEmail)
                .HasMaxLength(500)
                .HasColumnName("contact_person_email");
            entity.Property(e => e.ContactPersonName)
                .HasMaxLength(500)
                .HasColumnName("contact_person_name");
            entity.Property(e => e.ContactPersonRole)
                .HasMaxLength(200)
                .HasColumnName("contact_person_role");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Domain).HasColumnName("domain");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.OrganizationSurveyId).HasColumnName("organization_survey_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
            entity.Property(e => e.Vat).HasColumnName("vat");

            entity.HasOne(d => d.Account).WithMany(p => p.Organisations)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organisations_account_id_fkey");

            entity.HasOne(d => d.Country).WithMany(p => p.Organisations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organisations_country_id_fkey");
        });

        modelBuilder.Entity<OrganisationSector>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organisation_sectors_pkey");

            entity.ToTable("organisation_sectors", "org");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ActivitySectorId).HasColumnName("activity_sector_id");
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");

            entity.HasOne(d => d.ActivitySector).WithMany(p => p.OrganisationSectors)
                .HasForeignKey(d => d.ActivitySectorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organisation_sectors_activity_sector_id_fkey");

            entity.HasOne(d => d.Organisation).WithMany(p => p.OrganisationSectors)
                .HasForeignKey(d => d.OrganisationId)
                .HasConstraintName("organisation_sectors_organisation_id_fkey");
        });

        modelBuilder.Entity<OrganisationSurvey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("organisation_surveys_pkey");

            entity.ToTable("organisation_surveys", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Organisation).WithMany(p => p.OrganisationSurveys)
                .HasForeignKey(d => d.OrganisationId)
                .HasConstraintName("organisation_surveys_organisation_id_fkey");

            entity.HasOne(d => d.Survey).WithMany(p => p.OrganisationSurveys)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("organisation_surveys_survey_id_fkey");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("questions_pkey");

            entity.ToTable("questions", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.ParrentAnswerId).HasColumnName("parrent_answer_id");
            entity.Property(e => e.Position).HasColumnName("position");
            entity.Property(e => e.QuestionTypeId).HasColumnName("question_type_id");
            entity.Property(e => e.SectionId).HasColumnName("section_id");

            entity.HasOne(d => d.ParrentAnswer).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ParrentAnswerId)
                .HasConstraintName("questions_parrent_answer_id_fkey");

            entity.HasOne(d => d.QuestionType).WithMany(p => p.Questions)
                .HasForeignKey(d => d.QuestionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("questions_question_type_id_fkey");

            entity.HasOne(d => d.Section).WithMany(p => p.Questions)
                .HasForeignKey(d => d.SectionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("questions_section_id_fkey");
        });

        modelBuilder.Entity<QuestionAttachment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_attachments_pkey");

            entity.ToTable("question_attachments", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AttachmentId).HasColumnName("attachment_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");

            entity.HasOne(d => d.Attachment).WithMany(p => p.QuestionAttachments)
                .HasForeignKey(d => d.AttachmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_attachments_attachment_id_fkey");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionAttachments)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("question_attachments_question_id_fkey");
        });

        modelBuilder.Entity<QuestionType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("question_types_pkey");

            entity.ToTable("question_types", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("surveys_pkey");

            entity.ToTable("surveys", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CrearedBy).HasColumnName("creared_by");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EvaluationScript)
                .HasColumnType("json")
                .HasColumnName("evaluation_script");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.IsPublic).HasColumnName("is_public");
            entity.Property(e => e.IsPublished).HasColumnName("is_published");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.SurveyType)
                .HasMaxLength(50)
                .HasDefaultValueSql("'none'::character varying")
                .HasColumnName("survey_type");

            entity.HasOne(d => d.CrearedByNavigation).WithMany(p => p.Surveys)
                .HasForeignKey(d => d.CrearedBy)
                .HasConstraintName("surveys_creared_by_fkey");
        });

        modelBuilder.Entity<SurveyResponse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("survey_responses_pkey");

            entity.ToTable("survey_responses", "srv");

            entity.HasIndex(e => new { e.QuestionId, e.OrganisationSurveyId }, "uq_survey_responses_question_id_survey_id").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Mentions).HasColumnName("mentions");
            entity.Property(e => e.OrganisationSurveyId).HasColumnName("organisation_survey_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Response).HasColumnName("response");
            entity.Property(e => e.UpdateAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("update_at");

            entity.HasOne(d => d.OrganisationSurvey).WithMany(p => p.SurveyResponses)
                .HasForeignKey(d => d.OrganisationSurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("survey_responses_organisation_survey_id_fkey");

            entity.HasOne(d => d.Question).WithMany(p => p.SurveyResponses)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("survey_responses_question_id_fkey");
        });

        modelBuilder.Entity<SurveySection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("survey_sections_pkey");

            entity.ToTable("survey_sections", "srv");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.ParentSectionId).HasColumnName("parent_section_id");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");

            entity.HasOne(d => d.ParentSection).WithMany(p => p.InverseParentSection)
                .HasForeignKey(d => d.ParentSectionId)
                .HasConstraintName("survey_sections_parent_section_id_fkey");

            entity.HasOne(d => d.Survey).WithMany(p => p.SurveySections)
                .HasForeignKey(d => d.SurveyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("survey_sections_survey_id_fkey");
        });

        modelBuilder.Entity<VwOrganisationResult>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_organisation_results", "srv");

            entity.Property(e => e.ActivitySectorId).HasColumnName("activity_sector_id");
            entity.Property(e => e.ActivitySectorName)
                .HasMaxLength(500)
                .HasColumnName("activity_sector_name");
            entity.Property(e => e.CompanySize)
                .HasMaxLength(200)
                .HasColumnName("company_size");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.OrganisationId).HasColumnName("organisation_id");
            entity.Property(e => e.OrganisationName)
                .HasMaxLength(500)
                .HasColumnName("organisation_name");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");
        });

        modelBuilder.Entity<VwOrgannisationsReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_organnisations_report", "org");

            entity.Property(e => e.ActivitySectorId).HasColumnName("activity_sector_id");
            entity.Property(e => e.ActivitySectorName)
                .HasMaxLength(500)
                .HasColumnName("activity_sector_name");
            entity.Property(e => e.CompanySize)
                .HasMaxLength(200)
                .HasColumnName("company_size");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
        });

        modelBuilder.Entity<VwSectionReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_section_reports", "srv");

            entity.Property(e => e.ActivitySectorId).HasColumnName("activity_sector_id");
            entity.Property(e => e.CompanySize)
                .HasMaxLength(200)
                .HasColumnName("company_size");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(500)
                .HasColumnName("name");
            entity.Property(e => e.OrganisationSurveyId).HasColumnName("organisation_survey_id");
            entity.Property(e => e.QuestionId).HasColumnName("question_id");
            entity.Property(e => e.Response).HasColumnName("response");
            entity.Property(e => e.Result).HasColumnName("result");
            entity.Property(e => e.SurveyId).HasColumnName("survey_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
