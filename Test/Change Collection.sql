
 
  
alter database ConsentManagement set single_user with rollback immediate
USE master;  
GO  
ALTER DATABASE ConsentManagement  
COLLATE Thai_CI_AS ;  
GO 

alter database ConsentManagement set MULTI_USER

--Verify the collation setting.  
SELECT name, collation_name  
FROM sys.databases  
WHERE name = N'ConsentManagement';  
GO 