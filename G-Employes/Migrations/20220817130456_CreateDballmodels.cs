using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace G_Employes.Migrations
{
    public partial class CreateDballmodels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "navchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "navchar(100)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "categorieOveriers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fonctionnalite = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorieOveriers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "categorieProduits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorieProduits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "overiers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prenom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tel = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date_embauche = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CIN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sexe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    categorieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_overiers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_overiers_categorieOveriers_categorieId",
                        column: x => x.categorieId,
                        principalTable: "categorieOveriers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "produits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Desgination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Prix = table.Column<float>(type: "real", nullable: false),
                    Quantite = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CategorieId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_produits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_produits_categorieProduits_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "categorieProduits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Commandes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produitId = table.Column<int>(type: "int", nullable: true),
                    ovrierId = table.Column<int>(type: "int", nullable: true),
                    Qtt_Diduir = table.Column<int>(type: "int", nullable: false),
                    Date_operation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Commandes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Commandes_overiers_ovrierId",
                        column: x => x.ovrierId,
                        principalTable: "overiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Commandes_produits_produitId",
                        column: x => x.produitId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produitId = table.Column<int>(type: "int", nullable: true),
                    ovrierId = table.Column<int>(type: "int", nullable: true),
                    Qtt_Diduir = table.Column<int>(type: "int", nullable: false),
                    Date_operation = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operations_overiers_ovrierId",
                        column: x => x.ovrierId,
                        principalTable: "overiers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_operations_produits_produitId",
                        column: x => x.produitId,
                        principalTable: "produits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_ovrierId",
                table: "Commandes",
                column: "ovrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Commandes_produitId",
                table: "Commandes",
                column: "produitId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_ovrierId",
                table: "operations",
                column: "ovrierId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_produitId",
                table: "operations",
                column: "produitId");

            migrationBuilder.CreateIndex(
                name: "IX_overiers_categorieId",
                table: "overiers",
                column: "categorieId");

            migrationBuilder.CreateIndex(
                name: "IX_produits_CategorieId",
                table: "produits",
                column: "CategorieId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Commandes");

            migrationBuilder.DropTable(
                name: "operations");

            migrationBuilder.DropTable(
                name: "overiers");

            migrationBuilder.DropTable(
                name: "produits");

            migrationBuilder.DropTable(
                name: "categorieOveriers");

            migrationBuilder.DropTable(
                name: "categorieProduits");

            migrationBuilder.AlterColumn<string>(
                name: "Prenom",
                table: "AspNetUsers",
                type: "navchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "AspNetUsers",
                type: "navchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);
        }
    }
}
