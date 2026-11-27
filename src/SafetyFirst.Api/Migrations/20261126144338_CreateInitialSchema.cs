using System;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafetyFirst.Api.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitialSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "hazard_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    topic_code = table.Column<string>(type: "text", nullable: false),
                    version_no = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    content = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    source_references = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    review_record = table.Column<JsonElement>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hazard_entries", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "modules",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    sequence_no = table.Column<int>(type: "integer", nullable: false),
                    kind = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_modules", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rubric_versions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    version_label = table.Column<string>(type: "text", nullable: false),
                    passing_rules = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    review_record = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    published_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rubric_versions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<string>(type: "text", nullable: false),
                    disabled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    neon_auth_subject = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "rubric_criteria",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    rubric_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    area_code = table.Column<string>(type: "text", nullable: false),
                    requirement_code = table.Column<string>(type: "text", nullable: false),
                    essential = table.Column<bool>(type: "boolean", nullable: false),
                    evaluation_type = table.Column<string>(type: "text", nullable: false),
                    evaluation_config = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    explanation = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rubric_criteria", x => x.id);
                    table.ForeignKey(
                        name: "FK_rubric_criteria_rubric_versions_rubric_version_id",
                        column: x => x.rubric_version_id,
                        principalTable: "rubric_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scenario_versions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    module_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rubric_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    variant_code = table.Column<string>(type: "text", nullable: false),
                    version_no = table.Column<int>(type: "integer", nullable: false),
                    package_url = table.Column<string>(type: "text", nullable: false),
                    package_sha256 = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    reviewed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scenario_versions", x => x.id);
                    table.ForeignKey(
                        name: "FK_scenario_versions_modules_module_id",
                        column: x => x.module_id,
                        principalTable: "modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_scenario_versions_rubric_versions_rubric_version_id",
                        column: x => x.rubric_version_id,
                        principalTable: "rubric_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "classes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    term = table.Column<string>(type: "text", nullable: false),
                    archived_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    join_code_hash = table.Column<string>(type: "text", nullable: true),
                    code_expires_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_classes", x => x.id);
                    table.ForeignKey(
                        name: "FK_classes_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attempts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    learner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scenario_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    retry_of_id = table.Column<Guid>(type: "uuid", nullable: true),
                    kind = table.Column<string>(type: "text", nullable: false),
                    device_id = table.Column<Guid>(type: "uuid", nullable: false),
                    started_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    completed_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    received_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    validation_status = table.Column<string>(type: "text", nullable: false),
                    payload_hash = table.Column<string>(type: "text", nullable: false),
                    client_attempt_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attempts", x => x.id);
                    table.UniqueConstraint("AK_attempts_id_learner_id", x => new { x.id, x.learner_id });
                    table.ForeignKey(
                        name: "FK_attempts_attempts_retry_of_id",
                        column: x => x.retry_of_id,
                        principalTable: "attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_attempts_scenario_versions_scenario_version_id",
                        column: x => x.scenario_version_id,
                        principalTable: "scenario_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_attempts_users_learner_id",
                        column: x => x.learner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "scenario_hazards",
                columns: table => new
                {
                    scenario_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hazard_entry_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scenario_hazards", x => new { x.scenario_version_id, x.hazard_entry_id });
                    table.ForeignKey(
                        name: "FK_scenario_hazards_hazard_entries_hazard_entry_id",
                        column: x => x.hazard_entry_id,
                        principalTable: "hazard_entries",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_scenario_hazards_scenario_versions_scenario_version_id",
                        column: x => x.scenario_version_id,
                        principalTable: "scenario_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assignments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scenario_version_id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    due_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    retry_policy = table.Column<JsonElement>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assignments", x => x.id);
                    table.ForeignKey(
                        name: "FK_assignments_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assignments_scenario_versions_scenario_version_id",
                        column: x => x.scenario_version_id,
                        principalTable: "scenario_versions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "enrollments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    joined_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    approved_by_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollments", x => x.id);
                    table.ForeignKey(
                        name: "FK_enrollments_classes_class_id",
                        column: x => x.class_id,
                        principalTable: "classes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_enrollments_users_approved_by_id",
                        column: x => x.approved_by_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_enrollments_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "assessment_results",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    scorer_version = table.Column<string>(type: "text", nullable: false),
                    overall_rating = table.Column<decimal>(type: "numeric", nullable: false),
                    pass_or_retry = table.Column<string>(type: "text", nullable: false),
                    scored_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assessment_results", x => x.id);
                    table.ForeignKey(
                        name: "FK_assessment_results_attempts_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "attempt_events",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    sequence_no = table.Column<int>(type: "integer", nullable: false),
                    event_type = table.Column<string>(type: "text", nullable: false),
                    payload = table.Column<JsonElement>(type: "jsonb", nullable: false),
                    elapsed_ms = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_attempt_events", x => x.id);
                    table.ForeignKey(
                        name: "FK_attempt_events_attempts_attempt_id",
                        column: x => x.attempt_id,
                        principalTable: "attempts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "submissions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    assignment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attempt_id = table.Column<Guid>(type: "uuid", nullable: false),
                    submitted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    late_status = table.Column<string>(type: "text", nullable: false),
                    student_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_submissions", x => x.id);
                    table.ForeignKey(
                        name: "FK_submissions_assignments_assignment_id",
                        column: x => x.assignment_id,
                        principalTable: "assignments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_submissions_attempts_attempt_id_student_id",
                        columns: x => new { x.attempt_id, x.student_id },
                        principalTable: "attempts",
                        principalColumns: new[] { "id", "learner_id" },
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_submissions_users_student_id",
                        column: x => x.student_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "criterion_results",
                columns: table => new
                {
                    assessment_id = table.Column<Guid>(type: "uuid", nullable: false),
                    criterion_id = table.Column<Guid>(type: "uuid", nullable: false),
                    applicable = table.Column<bool>(type: "boolean", nullable: false),
                    earned = table.Column<short>(type: "smallint", nullable: true),
                    feedback_code = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_criterion_results", x => new { x.assessment_id, x.criterion_id });
                    table.ForeignKey(
                        name: "FK_criterion_results_assessment_results_assessment_id",
                        column: x => x.assessment_id,
                        principalTable: "assessment_results",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_criterion_results_rubric_criteria_criterion_id",
                        column: x => x.criterion_id,
                        principalTable: "rubric_criteria",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "teacher_feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    submission_id = table.Column<Guid>(type: "uuid", nullable: false),
                    teacher_id = table.Column<Guid>(type: "uuid", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_feedback", x => x.id);
                    table.ForeignKey(
                        name: "FK_teacher_feedback_submissions_submission_id",
                        column: x => x.submission_id,
                        principalTable: "submissions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_teacher_feedback_users_teacher_id",
                        column: x => x.teacher_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_assessment_results_attempt_id",
                table: "assessment_results",
                column: "attempt_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_assignments_class_id",
                table: "assignments",
                column: "class_id");

            migrationBuilder.CreateIndex(
                name: "IX_assignments_scenario_version_id",
                table: "assignments",
                column: "scenario_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_attempt_events_attempt_id_sequence_no",
                table: "attempt_events",
                columns: new[] { "attempt_id", "sequence_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attempts_learner_id_client_attempt_id",
                table: "attempts",
                columns: new[] { "learner_id", "client_attempt_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_attempts_retry_of_id",
                table: "attempts",
                column: "retry_of_id");

            migrationBuilder.CreateIndex(
                name: "IX_attempts_scenario_version_id",
                table: "attempts",
                column: "scenario_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_classes_teacher_id",
                table: "classes",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_criterion_results_criterion_id",
                table: "criterion_results",
                column: "criterion_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_approved_by_id",
                table: "enrollments",
                column: "approved_by_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_class_id_student_id",
                table: "enrollments",
                columns: new[] { "class_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_enrollments_student_id",
                table: "enrollments",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_hazard_entries_topic_code_version_no",
                table: "hazard_entries",
                columns: new[] { "topic_code", "version_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_modules_code",
                table: "modules",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rubric_criteria_rubric_version_id",
                table: "rubric_criteria",
                column: "rubric_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_scenario_hazards_hazard_entry_id",
                table: "scenario_hazards",
                column: "hazard_entry_id");

            migrationBuilder.CreateIndex(
                name: "IX_scenario_versions_module_id_variant_code_version_no",
                table: "scenario_versions",
                columns: new[] { "module_id", "variant_code", "version_no" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_scenario_versions_rubric_version_id",
                table: "scenario_versions",
                column: "rubric_version_id");

            migrationBuilder.CreateIndex(
                name: "IX_submissions_assignment_id_student_id",
                table: "submissions",
                columns: new[] { "assignment_id", "student_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_submissions_attempt_id_student_id",
                table: "submissions",
                columns: new[] { "attempt_id", "student_id" });

            migrationBuilder.CreateIndex(
                name: "IX_submissions_student_id",
                table: "submissions",
                column: "student_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_feedback_submission_id",
                table: "teacher_feedback",
                column: "submission_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_feedback_teacher_id",
                table: "teacher_feedback",
                column: "teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_neon_auth_subject",
                table: "users",
                column: "neon_auth_subject",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attempt_events");

            migrationBuilder.DropTable(
                name: "criterion_results");

            migrationBuilder.DropTable(
                name: "enrollments");

            migrationBuilder.DropTable(
                name: "scenario_hazards");

            migrationBuilder.DropTable(
                name: "teacher_feedback");

            migrationBuilder.DropTable(
                name: "assessment_results");

            migrationBuilder.DropTable(
                name: "rubric_criteria");

            migrationBuilder.DropTable(
                name: "hazard_entries");

            migrationBuilder.DropTable(
                name: "submissions");

            migrationBuilder.DropTable(
                name: "assignments");

            migrationBuilder.DropTable(
                name: "attempts");

            migrationBuilder.DropTable(
                name: "classes");

            migrationBuilder.DropTable(
                name: "scenario_versions");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "modules");

            migrationBuilder.DropTable(
                name: "rubric_versions");
        }
    }
}
