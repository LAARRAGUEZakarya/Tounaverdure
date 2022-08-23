using Microsoft.EntityFrameworkCore.Migrations;

namespace G_Employes.Migrations
{
    public partial class addculomnEtattoCommandmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Etat",
                table: "Commandes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Etat",
                table: "Commandes");
        }
    }
}
