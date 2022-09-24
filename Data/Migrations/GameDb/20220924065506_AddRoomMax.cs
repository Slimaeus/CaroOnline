using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations.GameDb
{
    public partial class AddRoomMax : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameUsers",
                columns: table => new
                {
                    UserName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameUsers", x => x.UserName);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    RoomName = table.Column<string>(type: "TEXT", nullable: false),
                    RoomMax = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.RoomName);
                });

            migrationBuilder.CreateTable(
                name: "Connections",
                columns: table => new
                {
                    ConnectionID = table.Column<string>(type: "TEXT", nullable: false),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: false),
                    Connected = table.Column<bool>(type: "INTEGER", nullable: false),
                    GameUserUserName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connections", x => x.ConnectionID);
                    table.ForeignKey(
                        name: "FK_Connections_GameUsers_GameUserUserName",
                        column: x => x.GameUserUserName,
                        principalTable: "GameUsers",
                        principalColumn: "UserName");
                });

            migrationBuilder.CreateTable(
                name: "GameUserPlayRoom",
                columns: table => new
                {
                    GameUsersUserName = table.Column<string>(type: "TEXT", nullable: false),
                    RoomsRoomName = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "IX_Connections_GameUserUserName",
                table: "Connections",
                column: "GameUserUserName");

            migrationBuilder.CreateIndex(
                name: "IX_GameUserPlayRoom_RoomsRoomName",
                table: "GameUserPlayRoom",
                column: "RoomsRoomName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connections");

            migrationBuilder.DropTable(
                name: "GameUserPlayRoom");

            migrationBuilder.DropTable(
                name: "GameUsers");

            migrationBuilder.DropTable(
                name: "Rooms");
        }
    }
}
