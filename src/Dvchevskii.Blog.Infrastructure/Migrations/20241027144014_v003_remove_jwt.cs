using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dvchevskii.Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v003_remove_jwt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuditInfo_CreatedById = table.Column<int>(type: "int", nullable: false),
                    AuditInfo_DeletedById = table.Column<int>(type: "int", nullable: true),
                    AuditInfo_UpdatedById = table.Column<int>(type: "int", nullable: true),
                    AuditInfo_CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuditInfo_DeletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_AuditInfo_CreatedById",
                        column: x => x.AuditInfo_CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_AuditInfo_DeletedById",
                        column: x => x.AuditInfo_DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_AuditInfo_UpdatedById",
                        column: x => x.AuditInfo_UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AuditInfo_CreatedById",
                table: "RefreshTokens",
                column: "AuditInfo_CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AuditInfo_DeletedById",
                table: "RefreshTokens",
                column: "AuditInfo_DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AuditInfo_UpdatedById",
                table: "RefreshTokens",
                column: "AuditInfo_UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");
        }
    }
}
