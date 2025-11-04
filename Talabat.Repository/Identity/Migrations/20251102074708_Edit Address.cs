using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Talabat.Repository.Identity.Migrations
{
    public partial class EditAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LName",
                table: "Adresses",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "FName",
                table: "Adresses",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Adresses",
                newName: "LName");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Adresses",
                newName: "FName");
        }
    }
}
