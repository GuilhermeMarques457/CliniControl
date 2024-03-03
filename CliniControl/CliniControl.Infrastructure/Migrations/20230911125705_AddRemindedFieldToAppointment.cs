using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliniControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRemindedFieldToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Reminded",
                table: "Appointments",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reminded",
                table: "Appointments");
        }
    }
}
