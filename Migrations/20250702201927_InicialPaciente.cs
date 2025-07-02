using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelemedicinaMonitores.Migrations
{
    /// <inheritdoc />
    public partial class InicialPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    MonitorId = table.Column<string>(type: "TEXT", nullable: false),
                    NombreCompleto = table.Column<string>(type: "TEXT", nullable: false),
                    Domicilio = table.Column<string>(type: "TEXT", nullable: false),
                    Enfermedades = table.Column<string>(type: "TEXT", nullable: false),
                    Vacunas = table.Column<string>(type: "TEXT", nullable: false),
                    HistorialClinico = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.MonitorId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
