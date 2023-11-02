Scaffold-DbContext "DataSource=.\Database\bombs.db" Microsoft.EntityFrameworkCore.Sqlite -Force -OutputDir Models


Once its ready, make it use a connectionstring for context instead of directly the DataSource