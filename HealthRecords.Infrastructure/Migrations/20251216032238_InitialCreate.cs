using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace HealthRecords.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PATIENTS",
                columns: table => new
                {
                    PATIENT_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NOMBRE = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EMAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FECHA_NACIMIENTO = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DOCUMENTO = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PATIENTS", x => x.PATIENT_ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    USER_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    USERNAME = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EMAIL = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PASSWORD_HASH = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ROLE = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "User"),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.USER_ID);
                });

            migrationBuilder.CreateTable(
                name: "MEDICAL_RECORDS",
                columns: table => new
                {
                    RECORD_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PATIENT_ID = table.Column<int>(type: "integer", nullable: false),
                    FECHA = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DIAGNOSTICO = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    TRATAMIENTO = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    MEDICO = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MEDICAL_RECORDS", x => x.RECORD_ID);
                    table.ForeignKey(
                        name: "FK_MEDICAL_RECORDS_PATIENTS_PATIENT_ID",
                        column: x => x.PATIENT_ID,
                        principalTable: "PATIENTS",
                        principalColumn: "PATIENT_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MEDICAL_RECORDS_PATIENT_ID",
                table: "MEDICAL_RECORDS",
                column: "PATIENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_PATIENTS_DOCUMENTO",
                table: "PATIENTS",
                column: "DOCUMENTO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PATIENTS_EMAIL",
                table: "PATIENTS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_EMAIL",
                table: "USERS",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USERS_USERNAME",
                table: "USERS",
                column: "USERNAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MEDICAL_RECORDS");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "PATIENTS");
        }
    }
}
