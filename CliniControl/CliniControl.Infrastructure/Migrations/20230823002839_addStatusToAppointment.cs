using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliniControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addStatusToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Paid",
                table: "Appointments");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Appointments",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.AddColumn<bool>(
                name: "Paid",
                table: "Appointments",
                type: "bit",
                nullable: true);
        }
    }
}
