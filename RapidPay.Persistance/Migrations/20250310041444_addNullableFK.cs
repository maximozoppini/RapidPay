using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RapidPay.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class addNullableFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationsLogs_Cards_CardId",
                table: "AuthorizationsLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "CardId",
                table: "AuthorizationsLogs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationsLogs_Cards_CardId",
                table: "AuthorizationsLogs",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationsLogs_Cards_CardId",
                table: "AuthorizationsLogs");

            migrationBuilder.AlterColumn<Guid>(
                name: "CardId",
                table: "AuthorizationsLogs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationsLogs_Cards_CardId",
                table: "AuthorizationsLogs",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
