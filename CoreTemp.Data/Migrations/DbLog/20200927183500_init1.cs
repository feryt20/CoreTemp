﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreTemp.Data.Migrations.DbLog
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExtendedLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventId = table.Column<int>(nullable: false),
                    Level = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    TimeStamp = table.Column<DateTimeOffset>(nullable: false),
                    Browser = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    User = table.Column<string>(nullable: true),
                    Path = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExtendedLog", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExtendedLog");
        }
    }
}
