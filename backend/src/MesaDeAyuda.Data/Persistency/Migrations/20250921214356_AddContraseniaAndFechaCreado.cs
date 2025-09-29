using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace MesaDeAyuda.Data.Persistency.Migrations;

/// <inheritdoc />
public partial class AddContraseniaAndFechaCreado : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "Contrasenia",
            table: "Usuario",
            type: "TEXT",
            maxLength: 100,
            schema: "MesaDeAyuda",
            nullable: true
        );

        migrationBuilder.AddColumn<DateTime>(
            name: "FechaCreacion",
            table: "Usuario",
            type: "TEXT",
            schema: "MesaDeAyuda",
            nullable: false,
            defaultValue: new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
        );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "Contrasenia", table: "Usuario", schema: "MesaDeAyuda");

        migrationBuilder.DropColumn(name: "FechaCreacion", table: "Usuario", schema: "MesaDeAyuda");
    }
}
