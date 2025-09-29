using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MesaDeAyuda.Data.Persistency.Migrations
{
    /// <inheritdoc />
    public partial class AddComentarioEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comentario",
                schema: "MesaDeAyuda",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketId = table.Column<int>(type: "INTEGER", nullable: false),
                    UsuarioRut = table.Column<string>(type: "TEXT", maxLength: 12, nullable: false),
                    Contenido = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comentario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comentario_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comentario_Usuario_UsuarioRut",
                        column: x => x.UsuarioRut,
                        principalSchema: "MesaDeAyuda",
                        principalTable: "Usuario",
                        principalColumn: "Rut",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_TicketId",
                schema: "MesaDeAyuda",
                table: "Comentario",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Comentario_UsuarioRut",
                schema: "MesaDeAyuda",
                table: "Comentario",
                column: "UsuarioRut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comentario",
                schema: "MesaDeAyuda");
        }
    }
}
