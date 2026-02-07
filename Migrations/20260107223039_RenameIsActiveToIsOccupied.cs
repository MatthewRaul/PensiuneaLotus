using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensiuneaLotus.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsActiveToIsOccupied : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Room",
                newName: "IsOccupied");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Room",
                newName: "IsOccupied");
        }
    }
}
