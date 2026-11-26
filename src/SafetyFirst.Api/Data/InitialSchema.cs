using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SafetyFirst.Api.Features.Identity;
using SafetyFirst.Api.Features.Classes;
using SafetyFirst.Api.Features.Content;
using SafetyFirst.Api.Features.Attempts;
using SafetyFirst.Api.Features.Assignments;

namespace SafetyFirst.Api.Data;

internal static class InitialSchema
{
    public static void Configure(ModelBuilder model)
    {
        ConfigureUserAccount(model.Entity<UserAccount>());
        ConfigureClassroom(model.Entity<Classroom>());
        ConfigureEnrollment(model.Entity<Enrollment>());
        ConfigureLearningModule(model.Entity<LearningModule>());
        ConfigureScenarioVersion(model.Entity<ScenarioVersion>());
        ConfigureHazardEntry(model.Entity<HazardEntry>());
        ConfigureScenarioHazard(model.Entity<ScenarioHazard>());
        ConfigureRubricVersion(model.Entity<RubricVersion>());
        ConfigureRubricCriterion(model.Entity<RubricCriterion>());
        ConfigureAttempt(model.Entity<Attempt>());
        ConfigureAttemptEvent(model.Entity<AttemptEvent>());
        ConfigureAssessmentResult(model.Entity<AssessmentResult>());
        ConfigureCriterionResult(model.Entity<CriterionResult>());
        ConfigureAssignment(model.Entity<Assignment>());
        ConfigureSubmission(model.Entity<Submission>());
        ConfigureTeacherFeedback(model.Entity<TeacherFeedback>());
    }

    private static void ConfigureUserAccount(EntityTypeBuilder<UserAccount> entity)
    {
        entity.ToTable("users");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.Email).HasColumnName("email");
        entity.Property(row => row.DisplayName).HasColumnName("display_name");
        entity.Property(row => row.Role).HasColumnName("role");
        entity.Property(row => row.DisabledAt).HasColumnName("disabled_at");
        entity.Property(row => row.NeonAuthSubject).HasColumnName("neon_auth_subject");

        entity.HasIndex(row => row.Email).IsUnique();
        entity.HasIndex(row => row.NeonAuthSubject).IsUnique();
    }

    private static void ConfigureClassroom(EntityTypeBuilder<Classroom> entity)
    {
        entity.ToTable("classes");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.TeacherId).HasColumnName("teacher_id");
        entity.Property(row => row.Name).HasColumnName("name");
        entity.Property(row => row.Term).HasColumnName("term");
        entity.Property(row => row.ArchivedAt).HasColumnName("archived_at");
        entity.Property(row => row.JoinCodeHash).HasColumnName("join_code_hash");
        entity.Property(row => row.CodeExpiresAt).HasColumnName("code_expires_at");

        entity.HasOne<UserAccount>().WithMany()
            .HasForeignKey(row => row.TeacherId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureEnrollment(EntityTypeBuilder<Enrollment> entity)
    {
        entity.ToTable("enrollments");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.ClassId).HasColumnName("class_id");
        entity.Property(row => row.StudentId).HasColumnName("student_id");
        entity.Property(row => row.Status).HasColumnName("status");
        entity.Property(row => row.JoinedAt).HasColumnName("joined_at");
        entity.Property(row => row.RequestedAt).HasColumnName("requested_at");
        entity.Property(row => row.ApprovedById).HasColumnName("approved_by_id");

        entity.HasIndex(row => new { row.ClassId, row.StudentId }).IsUnique();

        entity.HasOne<Classroom>().WithMany()
            .HasForeignKey(row => row.ClassId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<UserAccount>().WithMany()
            .HasForeignKey(row => row.StudentId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<UserAccount>().WithMany()
            .HasForeignKey(row => row.ApprovedById).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureLearningModule(EntityTypeBuilder<LearningModule> entity)
    {
        entity.ToTable("modules");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.Code).HasColumnName("code");
        entity.Property(row => row.Title).HasColumnName("title");
        entity.Property(row => row.SequenceNo).HasColumnName("sequence_no");
        entity.Property(row => row.Kind).HasColumnName("kind");

        entity.HasIndex(row => row.Code).IsUnique();
    }

    private static void ConfigureScenarioVersion(EntityTypeBuilder<ScenarioVersion> entity)
    {
        entity.ToTable("scenario_versions");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.ModuleId).HasColumnName("module_id");
        entity.Property(row => row.RubricVersionId).HasColumnName("rubric_version_id");
        entity.Property(row => row.VariantCode).HasColumnName("variant_code");
        entity.Property(row => row.VersionNo).HasColumnName("version_no");
        entity.Property(row => row.PackageUrl).HasColumnName("package_url");
        entity.Property(row => row.PackageSha256).HasColumnName("package_sha256");
        entity.Property(row => row.Status).HasColumnName("status");
        entity.Property(row => row.ReviewedAt).HasColumnName("reviewed_at");

        entity.HasIndex(row => new { row.ModuleId, row.VariantCode, row.VersionNo }).IsUnique();

        entity.HasOne<LearningModule>().WithMany()
            .HasForeignKey(row => row.ModuleId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<RubricVersion>().WithMany()
            .HasForeignKey(row => row.RubricVersionId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureHazardEntry(EntityTypeBuilder<HazardEntry> entity)
    {
        entity.ToTable("hazard_entries");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.TopicCode).HasColumnName("topic_code");
        entity.Property(row => row.VersionNo).HasColumnName("version_no");
        entity.Property(row => row.Title).HasColumnName("title");
        entity.Property(row => row.Category).HasColumnName("category");
        entity.Property(row => row.Content).HasColumnName("content").HasColumnType("jsonb");
        entity.Property(row => row.SourceReferences).HasColumnName("source_references").HasColumnType("jsonb");
        entity.Property(row => row.ReviewRecord).HasColumnName("review_record").HasColumnType("jsonb");

        entity.HasIndex(row => new { row.TopicCode, row.VersionNo }).IsUnique();
    }

    private static void ConfigureScenarioHazard(EntityTypeBuilder<ScenarioHazard> entity)
    {
        entity.ToTable("scenario_hazards");
        entity.HasKey(row => new { row.ScenarioVersionId, row.HazardEntryId });

        entity.Property(row => row.ScenarioVersionId).HasColumnName("scenario_version_id");
        entity.Property(row => row.HazardEntryId).HasColumnName("hazard_entry_id");

        entity.HasOne<ScenarioVersion>().WithMany()
            .HasForeignKey(row => row.ScenarioVersionId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<HazardEntry>().WithMany()
            .HasForeignKey(row => row.HazardEntryId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureRubricVersion(EntityTypeBuilder<RubricVersion> entity)
    {
        entity.ToTable("rubric_versions");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.VersionLabel).HasColumnName("version_label");
        entity.Property(row => row.PassingRules).HasColumnName("passing_rules").HasColumnType("jsonb");
        entity.Property(row => row.ReviewRecord).HasColumnName("review_record").HasColumnType("jsonb");
        entity.Property(row => row.PublishedAt).HasColumnName("published_at");
    }

    private static void ConfigureRubricCriterion(EntityTypeBuilder<RubricCriterion> entity)
    {
        entity.ToTable("rubric_criteria");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.RubricVersionId).HasColumnName("rubric_version_id");
        entity.Property(row => row.AreaCode).HasColumnName("area_code");
        entity.Property(row => row.RequirementCode).HasColumnName("requirement_code");
        entity.Property(row => row.Essential).HasColumnName("essential");
        entity.Property(row => row.EvaluationType).HasColumnName("evaluation_type");
        entity.Property(row => row.EvaluationConfig).HasColumnName("evaluation_config").HasColumnType("jsonb");
        entity.Property(row => row.Explanation).HasColumnName("explanation");

        entity.HasOne<RubricVersion>().WithMany()
            .HasForeignKey(row => row.RubricVersionId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAttempt(EntityTypeBuilder<Attempt> entity)
    {
        entity.ToTable("attempts");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.LearnerId).HasColumnName("learner_id");
        entity.Property(row => row.ScenarioVersionId).HasColumnName("scenario_version_id");
        entity.Property(row => row.RetryOfId).HasColumnName("retry_of_id");
        entity.Property(row => row.Kind).HasColumnName("kind");
        entity.Property(row => row.DeviceId).HasColumnName("device_id");
        entity.Property(row => row.StartedAt).HasColumnName("started_at");
        entity.Property(row => row.CompletedAt).HasColumnName("completed_at");
        entity.Property(row => row.ReceivedAt).HasColumnName("received_at");
        entity.Property(row => row.ValidationStatus).HasColumnName("validation_status");
        entity.Property(row => row.PayloadHash).HasColumnName("payload_hash");
        entity.Property(row => row.ClientAttemptId).HasColumnName("client_attempt_id");

        entity.HasIndex(row => new { row.LearnerId, row.ClientAttemptId }).IsUnique();
        entity.HasAlternateKey(row => new { row.Id, row.LearnerId });

        entity.HasOne<UserAccount>().WithMany()
            .HasForeignKey(row => row.LearnerId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<ScenarioVersion>().WithMany()
            .HasForeignKey(row => row.ScenarioVersionId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<Attempt>().WithMany()
            .HasForeignKey(row => row.RetryOfId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAttemptEvent(EntityTypeBuilder<AttemptEvent> entity)
    {
        entity.ToTable("attempt_events");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.AttemptId).HasColumnName("attempt_id");
        entity.Property(row => row.SequenceNo).HasColumnName("sequence_no");
        entity.Property(row => row.EventType).HasColumnName("event_type");
        entity.Property(row => row.Payload).HasColumnName("payload").HasColumnType("jsonb");
        entity.Property(row => row.ElapsedMs).HasColumnName("elapsed_ms");

        entity.HasIndex(row => new { row.AttemptId, row.SequenceNo }).IsUnique();

        entity.HasOne<Attempt>().WithMany()
            .HasForeignKey(row => row.AttemptId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAssessmentResult(EntityTypeBuilder<AssessmentResult> entity)
    {
        entity.ToTable("assessment_results");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.AttemptId).HasColumnName("attempt_id");
        entity.Property(row => row.ScorerVersion).HasColumnName("scorer_version");
        entity.Property(row => row.OverallRating).HasColumnName("overall_rating");
        entity.Property(row => row.PassOrRetry).HasColumnName("pass_or_retry");
        entity.Property(row => row.ScoredAt).HasColumnName("scored_at");

        entity.HasIndex(row => row.AttemptId).IsUnique();

        entity.HasOne<Attempt>().WithMany()
            .HasForeignKey(row => row.AttemptId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureCriterionResult(EntityTypeBuilder<CriterionResult> entity)
    {
        entity.ToTable("criterion_results");
        entity.HasKey(row => new { row.AssessmentId, row.CriterionId });

        entity.Property(row => row.AssessmentId).HasColumnName("assessment_id");
        entity.Property(row => row.CriterionId).HasColumnName("criterion_id");
        entity.Property(row => row.Applicable).HasColumnName("applicable");
        entity.Property(row => row.Earned).HasColumnName("earned");
        entity.Property(row => row.FeedbackCode).HasColumnName("feedback_code");

        entity.HasOne<AssessmentResult>().WithMany()
            .HasForeignKey(row => row.AssessmentId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<RubricCriterion>().WithMany()
            .HasForeignKey(row => row.CriterionId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureAssignment(EntityTypeBuilder<Assignment> entity)
    {
        entity.ToTable("assignments");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.ClassId).HasColumnName("class_id");
        entity.Property(row => row.ScenarioVersionId).HasColumnName("scenario_version_id");
        entity.Property(row => row.Title).HasColumnName("title");
        entity.Property(row => row.DueAt).HasColumnName("due_at");
        entity.Property(row => row.RetryPolicy).HasColumnName("retry_policy").HasColumnType("jsonb");

        entity.HasOne<Classroom>().WithMany()
            .HasForeignKey(row => row.ClassId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<ScenarioVersion>().WithMany()
            .HasForeignKey(row => row.ScenarioVersionId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureSubmission(EntityTypeBuilder<Submission> entity)
    {
        entity.ToTable("submissions");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.AssignmentId).HasColumnName("assignment_id");
        entity.Property(row => row.AttemptId).HasColumnName("attempt_id");
        entity.Property(row => row.SubmittedAt).HasColumnName("submitted_at");
        entity.Property(row => row.LateStatus).HasColumnName("late_status");
        entity.Property(row => row.StudentId).HasColumnName("student_id");

        entity.HasIndex(row => new { row.AssignmentId, row.StudentId }).IsUnique();

        entity.HasOne<Assignment>().WithMany()
            .HasForeignKey(row => row.AssignmentId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<Attempt>().WithMany()
            .HasForeignKey(row => new { row.AttemptId, row.StudentId })
            .HasPrincipalKey(row => new { row.Id, row.LearnerId }).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<UserAccount>().WithMany()
            .HasForeignKey(row => row.StudentId).OnDelete(DeleteBehavior.Restrict);
    }

    private static void ConfigureTeacherFeedback(EntityTypeBuilder<TeacherFeedback> entity)
    {
        entity.ToTable("teacher_feedback");
        entity.HasKey(row => row.Id);

        entity.Property(row => row.Id).HasColumnName("id");
        entity.Property(row => row.SubmissionId).HasColumnName("submission_id");
        entity.Property(row => row.TeacherId).HasColumnName("teacher_id");
        entity.Property(row => row.Body).HasColumnName("body");
        entity.Property(row => row.CreatedAt).HasColumnName("created_at");

        entity.HasOne<Submission>().WithMany()
            .HasForeignKey(row => row.SubmissionId).OnDelete(DeleteBehavior.Restrict);
        entity.HasOne<UserAccount>().WithMany()
            .HasForeignKey(row => row.TeacherId).OnDelete(DeleteBehavior.Restrict);
    }
}
