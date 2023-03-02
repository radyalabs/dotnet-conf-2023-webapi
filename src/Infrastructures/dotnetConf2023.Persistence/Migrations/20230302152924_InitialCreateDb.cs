using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace dotnetConf2023.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    NormalizedUsername = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    LastPasswordChangeAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserState = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    AboutMe = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CountryCode = table.Column<int>(type: "int", nullable: true),
                    EmailActivationStatus = table.Column<int>(type: "int", nullable: false),
                    EmailActivationCode = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    EmailActivationAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequestId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByDtServer = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleLog",
                columns: table => new
                {
                    UserRoleLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByDtServer = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleLog", x => x.UserRoleLogId);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByDtServer = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ExpiryAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeviceType = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastUpdatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedDtServer = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    DeletedByDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedByDtServer = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => x.UserTokenId);
                    table.ForeignKey(
                        name: "FK_UserToken_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedDt",
                table: "User",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_User_CreatedDtServer",
                table: "User",
                column: "CreatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_User_DeletedByDt",
                table: "User",
                column: "DeletedByDt");

            migrationBuilder.CreateIndex(
                name: "IX_User_FullName",
                table: "User",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedDt",
                table: "User",
                column: "LastUpdatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_User_LastUpdatedDtServer",
                table: "User",
                column: "LastUpdatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_User_NormalizedUsername",
                table: "User",
                column: "NormalizedUsername");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CreatedDt",
                table: "UserRole",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_CreatedDtServer",
                table: "UserRole",
                column: "CreatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_DeletedByDt",
                table: "UserRole",
                column: "DeletedByDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_LastUpdatedDt",
                table: "UserRole",
                column: "LastUpdatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_LastUpdatedDtServer",
                table: "UserRole",
                column: "LastUpdatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleLog_CreatedDt",
                table: "UserRoleLog",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleLog_CreatedDtServer",
                table: "UserRoleLog",
                column: "CreatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleLog_DeletedByDt",
                table: "UserRoleLog",
                column: "DeletedByDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleLog_LastUpdatedDt",
                table: "UserRoleLog",
                column: "LastUpdatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoleLog_LastUpdatedDtServer",
                table: "UserRoleLog",
                column: "LastUpdatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_CreatedDt",
                table: "UserToken",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_CreatedDtServer",
                table: "UserToken",
                column: "CreatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_DeletedByDt",
                table: "UserToken",
                column: "DeletedByDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_LastUpdatedDt",
                table: "UserToken",
                column: "LastUpdatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_LastUpdatedDtServer",
                table: "UserToken",
                column: "LastUpdatedDtServer");

            migrationBuilder.CreateIndex(
                name: "IX_UserToken_UserId",
                table: "UserToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserRoleLog");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
