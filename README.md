Scaffold-DbContext "Server=127.0.0.1; Port=3308; Database=sqlBombDisposal; Uid=root; Pwd=password;" MySql.EntityFrameworkCore -OutputDir Models
ConfigurationManager.ConnectionStrings["BombsDb"].ConnectionString

Copy App.config.dist to App.config.
Add your connection string.
