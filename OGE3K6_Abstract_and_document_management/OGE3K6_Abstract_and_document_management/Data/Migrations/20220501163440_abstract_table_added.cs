using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OGE3K6_Abstract_and_document_management.Data.Migrations
{
    public partial class abstract_table_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abstracts",
                columns: table => new
                {
                    UID = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AbstractTitle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    AbstractText = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    UploadTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UploaderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abstracts", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Abstracts_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Abstracts_AspNetUsers_UploaderId",
                        column: x => x.UploaderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Abstracts_ReviewerId",
                table: "Abstracts",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Abstracts_UploaderId",
                table: "Abstracts",
                column: "UploaderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abstracts");
        }
    }
}
