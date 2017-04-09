using System.Data.Entity.Migrations;

namespace WaidWeb.Migrations
{
    public partial class AddUploadId : DbMigration
    {
        public override void Up()
        {
            AddColumn("UserProfile", "UploadId", a => a.Guid());
        }

        public override void Down()
        {
            DropColumn("UserProfile", "UploadId");
        }
    }
}
