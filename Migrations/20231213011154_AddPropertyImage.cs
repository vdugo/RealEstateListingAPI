using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RealEstateListingAPI.Migrations
{
    public partial class AddPropertyImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Properties",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Properties");
        }
    }
}
