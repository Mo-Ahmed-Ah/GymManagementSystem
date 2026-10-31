using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagementSystem.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSessionMembersTableWithAddNewColumnIdAsPK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberSessions",
                table: "MemberSessions");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MemberSessions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberSessions",
                table: "MemberSessions",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MemberSessions_MemberId",
                table: "MemberSessions",
                column: "MemberId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MemberSessions",
                table: "MemberSessions");

            migrationBuilder.DropIndex(
                name: "IX_MemberSessions_MemberId",
                table: "MemberSessions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MemberSessions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MemberSessions",
                table: "MemberSessions",
                columns: new[] { "MemberId", "SessionId" });
        }
    }
}
