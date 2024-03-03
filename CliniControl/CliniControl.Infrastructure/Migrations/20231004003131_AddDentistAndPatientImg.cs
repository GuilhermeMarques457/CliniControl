using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CliniControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDentistAndPatientImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Patients",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhotoPath",
                table: "Dentists",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PhotoPath",
                table: "Dentists");
        }
    }
}
