using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ptoject2.Data.Migrations
{
    public partial class addSharedProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SharedWith",
                table: "Photo",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SharedWith",
                columns: table => new
                {
                    SharedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(nullable: false),
                    SharedWithId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedWith", x => x.SharedId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SharedWith");

            migrationBuilder.DropColumn(
                name: "SharedWith",
                table: "Photo");
        }
    }
}
