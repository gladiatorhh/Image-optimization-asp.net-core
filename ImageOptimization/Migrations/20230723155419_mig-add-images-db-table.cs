﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ImageOptimization.Migrations
{
    /// <inheritdoc />
    public partial class migaddimagesdbtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImagesDb",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OriginalImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    FullScreenImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ThumbImage = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImagesDb", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImagesDb");
        }
    }
}
