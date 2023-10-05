using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoControl.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddClinicIDToRequestedPatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClinicID",
                table: "RequestedPatients",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClinicID",
                table: "RequestedPatients");
        }
    }
}
