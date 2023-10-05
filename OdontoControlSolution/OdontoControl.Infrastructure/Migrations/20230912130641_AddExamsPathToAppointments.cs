using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddExamsPathToAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamsPath",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamsPath",
                table: "Appointments");
        }
    }
}
