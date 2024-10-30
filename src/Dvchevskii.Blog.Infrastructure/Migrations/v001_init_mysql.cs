using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dvchevskii.Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v001_init_mysql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    IsAdmin = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AuditInfo_CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuditInfo_CreatedById = table.Column<int>(type: "int", nullable: false),
                    AuditInfo_UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_UpdatedById = table.Column<int>(type: "int", nullable: true),
                    AuditInfo_DeletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_DeletedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_AuditInfo_CreatedById",
                        column: x => x.AuditInfo_CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Users_AuditInfo_DeletedById",
                        column: x => x.AuditInfo_DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_AuditInfo_UpdatedById",
                        column: x => x.AuditInfo_UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Filename = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AuditInfo_CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuditInfo_CreatedById = table.Column<int>(type: "int", nullable: false),
                    AuditInfo_UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_UpdatedById = table.Column<int>(type: "int", nullable: true),
                    AuditInfo_DeletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_DeletedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Users_AuditInfo_CreatedById",
                        column: x => x.AuditInfo_CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Images_Users_AuditInfo_DeletedById",
                        column: x => x.AuditInfo_DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Images_Users_AuditInfo_UpdatedById",
                        column: x => x.AuditInfo_UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tagline = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Body = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsDraft = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HeaderImageId = table.Column<int>(type: "int", nullable: true),
                    AuditInfo_CreatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AuditInfo_CreatedById = table.Column<int>(type: "int", nullable: false),
                    AuditInfo_UpdatedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_UpdatedById = table.Column<int>(type: "int", nullable: true),
                    AuditInfo_DeletedAtUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AuditInfo_DeletedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Images_HeaderImageId",
                        column: x => x.HeaderImageId,
                        principalTable: "Images",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuditInfo_CreatedById",
                        column: x => x.AuditInfo_CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuditInfo_DeletedById",
                        column: x => x.AuditInfo_DeletedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_Users_AuditInfo_UpdatedById",
                        column: x => x.AuditInfo_UpdatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AuditInfo_CreatedById",
                table: "Images",
                column: "AuditInfo_CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AuditInfo_DeletedById",
                table: "Images",
                column: "AuditInfo_DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Images_AuditInfo_UpdatedById",
                table: "Images",
                column: "AuditInfo_UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuditInfo_CreatedById",
                table: "Posts",
                column: "AuditInfo_CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuditInfo_DeletedById",
                table: "Posts",
                column: "AuditInfo_DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuditInfo_UpdatedById",
                table: "Posts",
                column: "AuditInfo_UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_HeaderImageId",
                table: "Posts",
                column: "HeaderImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditInfo_CreatedById",
                table: "Users",
                column: "AuditInfo_CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditInfo_DeletedById",
                table: "Users",
                column: "AuditInfo_DeletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuditInfo_UpdatedById",
                table: "Users",
                column: "AuditInfo_UpdatedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
