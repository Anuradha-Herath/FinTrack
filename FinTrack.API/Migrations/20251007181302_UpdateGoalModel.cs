using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoalModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetDate",
                table: "Goals",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Goals",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Goals",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Goals",
                newName: "TargetDate");
        }
    }
}
