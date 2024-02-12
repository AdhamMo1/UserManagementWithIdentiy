using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace UserManagementWithIdentiy.Data.Migrations
{
    public partial class asignroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                            table: "Roles",
                            columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                            values: new object[] { Guid.NewGuid().ToString(), "User", "user".ToUpper(), Guid.NewGuid().ToString() },
                            schema:"security"
                );
            migrationBuilder.InsertData(
                            table: "Roles",
                            columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                            values: new object[] { Guid.NewGuid().ToString(), "Admin", "admin".ToUpper(), Guid.NewGuid().ToString() },
                            schema: "security"
                );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete * from security.Roles");
        }
    }
}
