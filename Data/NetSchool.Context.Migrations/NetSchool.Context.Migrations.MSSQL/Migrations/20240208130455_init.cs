using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetSchool.Context.Migrations.MSSQL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NickName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "card_collections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TimeExpiration = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_card_collections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_card_collections_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Front = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Reverse = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    CardCollectionId = table.Column<int>(type: "int", nullable: true),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_cards_card_collections_CardCollectionId",
                        column: x => x.CardCollectionId,
                        principalTable: "card_collections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_card_collections_Uid",
                table: "card_collections",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_card_collections_UserId",
                table: "card_collections",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_cards_CardCollectionId",
                table: "cards",
                column: "CardCollectionId");

            migrationBuilder.CreateIndex(
                name: "IX_cards_Uid",
                table: "cards",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_Uid",
                table: "users",
                column: "Uid",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cards");

            migrationBuilder.DropTable(
                name: "card_collections");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
