using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Openbook.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedAnotherUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2", null, "User", "USER" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4b041e05-f263-40de-a17f-a8c522c0179b", "AQAAAAIAAYagAAAAEPpkv8lSSMU8GVIt+OhOAc/MCHpx8eIBaXBPbPme0bD2V2EPVFCs3lFd349MCqPNcA==", "376d5317-3595-4af2-b324-c2ba6416cb3e" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "Name", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2", 0, "03f3ee69-392f-405d-a667-ab3edd676c53", "jdoe@openbook.local", true, false, null, "John Doe", "JDOE@OPENBOOK.LOCAL", "JDOE", "AQAAAAIAAYagAAAAEERDGl+XAcoyUbX4ajaIvyP67kVgFlUUpYqek0W5fBg92Hq8/gUXBbmjNo5qVZdg1w==", null, false, "f13f4fdc-efac-4ff4-86a7-c1add83bd509", false, "jdoe" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2", "d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2", "d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c2c2c2c2-c2c2-c2c2-c2c2-c2c2c2c2c2c2");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d2d2d2d2-d2d2-d2d2-d2d2-d2d2d2d2d2d2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "bbd31a31-e631-48cb-9190-9a211617bdad", "AQAAAAIAAYagAAAAEDns5NjvEoIO/djVPyFDQNWjdqaSyA5TQ4wREMNOAq4uj2a2KI07OVzWeyYjaZ8D7Q==", "94a6fce0-3897-49c1-ba3c-cd2886dc5ebb" });
        }
    }
}
