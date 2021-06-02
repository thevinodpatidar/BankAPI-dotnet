using Microsoft.EntityFrameworkCore.Migrations;

namespace BankAPIApplication.Migrations.SqlServerMigrations
{
    public partial class AddedTimestamps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("CREATE TRIGGER [dbo].[Users_UPDATEDAT] ON [dbo].[Users] AFTER UPDATE AS BEGIN SET NOCOUNT ON; IF((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN; DECLARE @Id INT SELECT @Id = INSERTED.Id FROM INSERTED UPDATE dbo.Users SET UpdatedAt = GETDATE() WHERE Id = @Id END");
            migrationBuilder.Sql("CREATE TRIGGER [dbo].[Transactions_UPDATEDAT] ON [dbo].[Transactions] AFTER UPDATE AS BEGIN SET NOCOUNT ON; IF((SELECT TRIGGER_NESTLEVEL()) > 1) RETURN; DECLARE @Id INT SELECT @Id = INSERTED.Id FROM INSERTED UPDATE dbo.Transactions SET UpdatedAt = GETDATE() WHERE Id = @Id END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop trigger Users_UPDATEAT");
            migrationBuilder.Sql("drop trigger Transactions_UPDATEAT");
        }
    }
}
