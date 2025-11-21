using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WScoreInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialClean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_USER",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Nome = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USER", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_CHECKIN",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    DataCheckin = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    Humor = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Sono = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Foco = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Score = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Energia = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CargaTrabalho = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    UserId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CHECKIN", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CHECKIN_TB_USER_UserId",
                        column: x => x.UserId,
                        principalTable: "TB_USER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CHECKIN_UserId",
                table: "TB_CHECKIN",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CHECKIN");

            migrationBuilder.DropTable(
                name: "TB_USER");
        }
    }
}
