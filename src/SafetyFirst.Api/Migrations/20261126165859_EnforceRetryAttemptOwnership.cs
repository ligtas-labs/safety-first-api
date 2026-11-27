using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafetyFirst.Api.Migrations
{
    /// <inheritdoc />
    public partial class EnforceRetryAttemptOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attempts_attempts_retry_of_id",
                table: "attempts");

            migrationBuilder.DropIndex(
                name: "IX_attempts_retry_of_id",
                table: "attempts");

            migrationBuilder.CreateIndex(
                name: "IX_attempts_retry_of_id_learner_id",
                table: "attempts",
                columns: new[] { "retry_of_id", "learner_id" });

            migrationBuilder.AddForeignKey(
                name: "FK_attempts_attempts_retry_of_id_learner_id",
                table: "attempts",
                columns: new[] { "retry_of_id", "learner_id" },
                principalTable: "attempts",
                principalColumns: new[] { "id", "learner_id" },
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attempts_attempts_retry_of_id_learner_id",
                table: "attempts");

            migrationBuilder.DropIndex(
                name: "IX_attempts_retry_of_id_learner_id",
                table: "attempts");

            migrationBuilder.CreateIndex(
                name: "IX_attempts_retry_of_id",
                table: "attempts",
                column: "retry_of_id");

            migrationBuilder.AddForeignKey(
                name: "FK_attempts_attempts_retry_of_id",
                table: "attempts",
                column: "retry_of_id",
                principalTable: "attempts",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
