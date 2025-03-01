using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YapeServices.Database.Migrations
{
    /// <inheritdoc />
    public partial class TransactionsAddAntifraudReason : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AntifraudReason",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AntifraudReason",
                table: "Transactions");
        }
    }
}
