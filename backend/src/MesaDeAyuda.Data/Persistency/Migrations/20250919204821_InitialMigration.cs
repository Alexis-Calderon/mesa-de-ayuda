using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesaDeAyuda.Data.Persistency.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(name: "MesaDeAyuda");

            migrationBuilder.CreateTable(
                name: "Usuario",
                schema: "MesaDeAyuda",
                columns: table => new
                {
                    Rut = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    Nombre = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Rol = table.Column<int>(type: "INTEGER", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Rut);
                }
            );

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tipo = table.Column<int>(type: "INTEGER", nullable: false),
                    Prioridad = table.Column<int>(type: "INTEGER", nullable: false),
                    Area = table.Column<int>(type: "INTEGER", nullable: false),
                    Estado = table.Column<int>(type: "INTEGER", nullable: false),
                    Descripcion = table.Column<string>(
                        type: "TEXT",
                        maxLength: 100,
                        nullable: false
                    ),
                    Observaciones = table.Column<string>(
                        type: "TEXT",
                        maxLength: 500,
                        nullable: false
                    ),
                    UsuarioRutCreador = table.Column<string>(type: "TEXT", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UsuarioRutTecnico = table.Column<string>(type: "TEXT", nullable: true),
                    FechaResolucion = table.Column<DateTime>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Usuario_UsuarioRutCreador",
                        column: x => x.UsuarioRutCreador,
                        principalSchema: "MesaDeAyuda",
                        principalTable: "Usuario",
                        principalColumn: "Rut",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_Tickets_Usuario_UsuarioRutTecnico",
                        column: x => x.UsuarioRutTecnico,
                        principalSchema: "MesaDeAyuda",
                        principalTable: "Usuario",
                        principalColumn: "Rut"
                    );
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UsuarioRutCreador",
                table: "Tickets",
                column: "UsuarioRutCreador"
            );

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UsuarioRutTecnico",
                table: "Tickets",
                column: "UsuarioRutTecnico"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Tickets");

            migrationBuilder.DropTable(name: "Usuario", schema: "MesaDeAyuda");
        }
    }
}
