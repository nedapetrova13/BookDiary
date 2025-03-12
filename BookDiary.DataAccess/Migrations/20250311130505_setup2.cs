using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookDiary.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class setup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksTags_Books_BookId",
                table: "BooksTags");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksTags_Tags_TagId",
                table: "BooksTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BooksTags",
                table: "BooksTags");

            migrationBuilder.DropIndex(
                name: "IX_BooksTags_BookId",
                table: "BooksTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BooksTags",
                table: "BooksTags",
                columns: new[] { "BookId", "TagId" });

            migrationBuilder.CreateIndex(
                name: "IX_BooksTags_TagId",
                table: "BooksTags",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksTags_Books_BookId",
                table: "BooksTags",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BooksTags_Tags_TagId",
                table: "BooksTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksTags_Books_BookId",
                table: "BooksTags");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksTags_Tags_TagId",
                table: "BooksTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BooksTags",
                table: "BooksTags");

            migrationBuilder.DropIndex(
                name: "IX_BooksTags_TagId",
                table: "BooksTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BooksTags",
                table: "BooksTags",
                columns: new[] { "TagId", "BookId" });

            migrationBuilder.CreateIndex(
                name: "IX_BooksTags_BookId",
                table: "BooksTags",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksTags_Books_BookId",
                table: "BooksTags",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksTags_Tags_TagId",
                table: "BooksTags",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id");
        }
    }
}
