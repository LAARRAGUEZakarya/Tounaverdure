using Microsoft.EntityFrameworkCore.Migrations;

namespace G_Employes.Migrations
{
    public partial class createSAlaire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "SalaireParMois",
                table: "detailsPointeuses",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalaireParMois",
                table: "detailsPointeuses");
        }
    }
}
