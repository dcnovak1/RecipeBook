In this project I create the backend for my recipe book website. This portion of the project is a RESTful api using asp.netcore and mssql server. 


To setup and use this project you will need:
  Visual Studios: https://visualstudio.microsoft.com/
  Docker Desktop: https://www.docker.com/products/docker-desktop/
  SQL server managment studio: https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

Visual Studios 2022 modify instalation:
  ![image](https://github.com/user-attachments/assets/4c23cee7-1ef5-4a1d-a275-339e4eaf4edb)


Make sure Docker Desktop is open.

Now you can clone the repo with:

```
git clone https://github.com/dcnovak1/RecipeBook.git
```

Then run the RecipeBook.sln

On running it should have a popup to install the targeted .net framework update it to the recomended version.
![image](https://github.com/user-attachments/assets/5b8de566-3f6e-4bc7-88b3-055fe9f62324)


Before you run the Docker compose you need to go into the docker-compose.yml and change the SA_PASSWORD or add a file with the password.

Now you can run the Docker Compose and it will setup the docker containers.

After its done you should get a sqlException because the database isn't setup.


To settup the sql database you need to connect to it through the SQL Server Management studio and create a RecipeBook database in the databases.

![image](https://github.com/user-attachments/assets/c5b515f0-b2b7-44db-b3dd-51d400b35ea5)

![image](https://github.com/user-attachments/assets/243cd50d-0348-4f32-88b0-ed8ae5a23d85)


after that you need to go back to the project and find the DevSqlCompare.scmp in RecipeBook.Main.
On the top right of the window in the sql compare will be a drop down arrow where you will Select Target > Select Connection in DataBase > Browse then type in server info and connect

![image](https://github.com/user-attachments/assets/f0f05dbb-d6c9-4b3c-8e1b-aac69481ae0a)


After that compare at the top left then update.

You can now run the Docker Compose and it is setup.
