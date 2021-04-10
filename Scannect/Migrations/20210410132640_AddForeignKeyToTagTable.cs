using Microsoft.EntityFrameworkCore.Migrations;

namespace Scannect.Migrations
{
    public partial class AddForeignKeyToTagTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Item_ItemId",
                table: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Tag",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Item_ItemId",
                table: "Tag",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_Item_ItemId",
                table: "Tag");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "Tag",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_Item_ItemId",
                table: "Tag",
                column: "ItemId",
                principalTable: "Item",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
