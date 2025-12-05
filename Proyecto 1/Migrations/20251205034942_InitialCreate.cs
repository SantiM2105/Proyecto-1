using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyecto_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animales",
                columns: table => new
                {
                    IdAnimal = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Especie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Habitat = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    EstadoSalud = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IdCuidador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animales", x => x.IdAnimal);
                    table.ForeignKey(
                        name: "FK_Animales_Cuidadores_IdCuidador",
                        column: x => x.IdCuidador,
                        principalTable: "Cuidadores",
                        principalColumn: "IdCuidadores",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Visitantes",
                columns: table => new
                {
                    IdVisitante = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Telefono = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitantes", x => x.IdVisitante);
                });

            migrationBuilder.CreateTable(
                name: "MovimientoVisitas",
                columns: table => new
                {
                    IdMovimiento = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdVisitante = table.Column<int>(type: "int", nullable: false),
                    IdAnimal = table.Column<int>(type: "int", nullable: false),
                    IdCuidador = table.Column<int>(type: "int", nullable: false),
                    FechaVisita = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Duracion = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimientoVisitas", x => x.IdMovimiento);
                    table.ForeignKey(
                        name: "FK_MovimientoVisitas_Animales_IdAnimal",
                        column: x => x.IdAnimal,
                        principalTable: "Animales",
                        principalColumn: "IdAnimal",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoVisitas_Cuidadores_IdCuidador",
                        column: x => x.IdCuidador,
                        principalTable: "Cuidadores",
                        principalColumn: "IdCuidadores",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MovimientoVisitas_Visitantes_IdVisitante",
                        column: x => x.IdVisitante,
                        principalTable: "Visitantes",
                        principalColumn: "IdVisitante",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animales_IdCuidador",
                table: "Animales",
                column: "IdCuidador");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoVisitas_IdAnimal",
                table: "MovimientoVisitas",
                column: "IdAnimal");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoVisitas_IdCuidador",
                table: "MovimientoVisitas",
                column: "IdCuidador");

            migrationBuilder.CreateIndex(
                name: "IX_MovimientoVisitas_IdVisitante",
                table: "MovimientoVisitas",
                column: "IdVisitante");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovimientoVisitas");

            migrationBuilder.DropTable(
                name: "Animales");

            migrationBuilder.DropTable(
                name: "Visitantes");
        }
    }
}
