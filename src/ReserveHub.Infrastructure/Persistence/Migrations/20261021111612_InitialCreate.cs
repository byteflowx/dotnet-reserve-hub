using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814

namespace ReserveHub.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:btree_gist", ",,");

            migrationBuilder.CreateTable(
                name: "Resources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ResourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    BookedBy = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Start = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Resources_ResourceId",
                        column: x => x.ResourceId,
                        principalTable: "Resources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Resources",
                columns: new[] { "Id", "Capacity", "Name", "Type" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 6, "Room Alpha", "Room" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 12, "Room Beta", "Room" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 1, "Desk 12", "Desk" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 1, "Desk 14", "Desk" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 1, "Projector P1", "Equipment" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ResourceId_Start",
                table: "Bookings",
                columns: new[] { "ResourceId", "Start" });

            migrationBuilder.Sql("""
                ALTER TABLE "Bookings" ADD CONSTRAINT "EX_Bookings_NoOverlap"
                  EXCLUDE USING gist ("ResourceId" WITH =, tstzrange("Start", "End", '[)') WITH &&)
                  WHERE ("Status" = 'Confirmed');
                """);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Resources");
        }
    }
}
