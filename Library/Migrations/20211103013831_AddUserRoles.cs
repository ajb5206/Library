using Microsoft.EntityFrameworkCore.Migrations;

namespace Library.Migrations
{
    public partial class AddUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) Values (1,'Librarian','LIBRARIAN')");
            migrationBuilder.Sql($"INSERT INTO AspNetRoles (Id, Name, NormalizedName) Values (2, 'Patron', 'PATRON')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = 1");
            migrationBuilder.Sql($"DELETE FROM AspNetRoles WHERE Id = 2");

        }
    }
}
