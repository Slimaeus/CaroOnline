using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.GameDb
{
    public partial class Inititalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUsers", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomMax = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomName);
                });

            migrationBuilder.CreateTable(
                name: "GameUserPlayRoom",
                columns: table => new
                {
                    GameUsersUserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoomsRoomName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUserPlayRoom", x => new { x.GameUsersUserName, x.RoomsRoomName });
                    table.ForeignKey(
                        name: "FK_GameUserPlayRoom_GameUsers_GameUsersUserName",
                        column: x => x.GameUsersUserName,
                        principalTable: "GameUsers",
                        principalColumn: "UserName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameUserPlayRoom_Rooms_RoomsRoomName",
                        column: x => x.RoomsRoomName,
                        principalTable: "Rooms",
                        principalColumn: "RoomName",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameUserPlayRoom_RoomsRoomName",
                table: "GameUserPlayRoom",
                column: "RoomsRoomName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUserPlayRoom");

            migrationBuilder.DropTable(
                name: "GameUsers");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
