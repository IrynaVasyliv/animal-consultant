using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimalConsultant.DAL.Migrations
{
    public partial class AddedPetToQuesiton : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PetId",
                table: "Questions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Questions_PetId",
                table: "Questions",
                column: "PetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Pets_PetId",
                table: "Questions",
                column: "PetId",
                principalTable: "Pets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Pets_PetId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_PetId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Questions");
        }
    }
}
