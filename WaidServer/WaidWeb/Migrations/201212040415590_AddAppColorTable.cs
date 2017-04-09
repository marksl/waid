namespace WaidWeb.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class AddAppColorTable : DbMigration
    {
        public override void Up()
        {
            CreateTable("AppColors", c =>
                                     new
                                         {
                                             AppId = c.Long(nullable: false),
                                             Color = c.String(nullable: false, maxLength: 20)
                                         })
                .PrimaryKey(c => c.AppId);
        }

        public override void Down()
        {
            DropTable("AppColors");
        }
    }
}
