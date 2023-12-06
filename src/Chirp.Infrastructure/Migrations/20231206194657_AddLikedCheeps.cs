using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddLikedCheeps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthorLikedCheeps",
                columns: table => new
                {
                    LikedByAuthorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedCheepsCheepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorLikedCheeps", x => new { x.LikedByAuthorId, x.LikedCheepsCheepId });
                    table.ForeignKey(
                        name: "FK_AuthorLikedCheeps_Authors_LikedByAuthorId",
                        column: x => x.LikedByAuthorId,
                        principalTable: "Authors",
                        principalColumn: "AuthorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorLikedCheeps_Cheeps_LikedCheepsCheepId",
                        column: x => x.LikedCheepsCheepId,
                        principalTable: "Cheeps",
                        principalColumn: "CheepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorLikedCheeps_LikedCheepsCheepId",
                table: "AuthorLikedCheeps",
                column: "LikedCheepsCheepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorLikedCheeps");
        }
    }
}
