using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dbragas.Migrations
{
    public partial class FixPasswordValues : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("09ffba47-040d-44b0-ab4e-426df2479072"));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(max)",
                unicode: false,
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(MAX)",
                oldUnicode: false,
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "IsActive", "Name", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("3b7cf297-3f76-463d-80d8-1b611a41e9e9"), new DateTime(2023, 9, 26, 23, 11, 52, 299, DateTimeKind.Utc).AddTicks(1770), null, "administração@dbragas.com.br", true, "dBragas", "$2a$11$lniElpvb7zV4yqVO3pMrJ.qRztzLCtJPkzvjnyeGicomsEkSDFdCW", new DateTime(2023, 9, 26, 23, 11, 52, 299, DateTimeKind.Utc).AddTicks(1774), "DBragas" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("3b7cf297-3f76-463d-80d8-1b611a41e9e9"));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(MAX)",
                unicode: false,
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(max)",
                oldUnicode: false,
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "DeletedAt", "Email", "IsActive", "Name", "Password", "UpdatedAt", "Username" },
                values: new object[] { new Guid("09ffba47-040d-44b0-ab4e-426df2479072"), new DateTime(2023, 9, 26, 22, 58, 46, 532, DateTimeKind.Utc).AddTicks(1270), null, "administração@dbragas.com.br", true, "dBragas", "$2a$11$lniElpvb7zV4yqVO3pMrJ.qRztzLCtJPkzvjnyeGicomsEkSDFdCW", new DateTime(2023, 9, 26, 22, 58, 46, 532, DateTimeKind.Utc).AddTicks(1273), "DBragas" });
        }
    }
}
