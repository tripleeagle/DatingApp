using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Auth.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Auth_Credentials",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: true),
                    Relation_State_Id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auth_Credentials", x => x.Email);
                });

            migrationBuilder.CreateTable(
                name: "Jwt_Refresh_Token",
                columns: table => new
                {
                    Email = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Valid_To = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jwt_Refresh_Token", x => x.Email);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auth_Credentials");

            migrationBuilder.DropTable(
                name: "Jwt_Refresh_Token");
        }
    }
}
