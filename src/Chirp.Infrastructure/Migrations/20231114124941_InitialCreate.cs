﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chirp.Infrastructure.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Authors",
				columns: table => new
				{
					AuthorId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Authors", x => x.AuthorId);
				});

			migrationBuilder.CreateTable(
				name: "Cheeps",
				columns: table => new
				{
					CheepId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					AuthorId = table.Column<int>(type: "int", nullable: false),
					Text = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
					TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Cheeps", x => x.CheepId);
					table.ForeignKey(
						name: "FK_Cheeps_Authors_AuthorId",
						column: x => x.AuthorId,
						principalTable: "Authors",
						principalColumn: "AuthorId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Cheeps_AuthorId",
				table: "Cheeps",
				column: "AuthorId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Cheeps");

			migrationBuilder.DropTable(
				name: "Authors");
		}
	}
}
