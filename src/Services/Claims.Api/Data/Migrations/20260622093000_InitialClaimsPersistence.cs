using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Claims.Api.Data.Migrations;

public partial class InitialClaimsPersistence : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Claims",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                ClaimNumber = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                CustomerId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                PolicyNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                EstimatedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                LossDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Status = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                SubmittedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Claims", x => x.Id);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Claims_ClaimNumber",
            table: "Claims",
            column: "ClaimNumber",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Claims");
    }
}
