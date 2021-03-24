# XL.DataAccess
Simple Data Access Object for EntityFramework (.NET Framework)

#### Quick, simple and powerful
**XL.DataAccess** is just a sigle **Data Access Object Class** that aims for simplicity. 
It works for both **Code First** and **Database First**. 

## How to install
// TODO

## How to use
- Include both XL.DataAccess and the namespace where your EntityFramework models are located
```csharp
using EntityFramework.Models.Namespace; // Your custom database models
using XL.DataAccess; // This library
```

- Instantiate new Data Access Object
```csharp
// Init with default Connection String
DAO dao = new DAO();

// Init with Connection String ID
DAO dao = new DAO("DatabaseEntities");

// Init with RAW Connection String
DAO dao = new DAO("metadata=res://*/Model.csdl|res://*/Model.ssdl|res://*/Model.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost;initial catalog=DatabaseName;persist security info=True;user id=UserName;password=UserPassword;MultipleActiveResultSets=True;App=EntityFramework&quot;");
```

- Get an item
```csharp
// Gets one item
Product product = dao.Get<Product>(x => x.ProductID == 1);

// Gets one item and loads it's related images
Product product = dao
    .Get<Product>(x => x.ProductID == 1,
                  x => x.Images);
```

- Get a list of items
```csharp
// Gets a list of items
IList<Product> productList = dao.GetList<Product>(x => x.Price > 20);

// Gets a list of items and loads it's related images
Product product = dao
    .Get<Product>(x => x.Price > 20,
                  x => x.Images);
```

- Get a list of items with pagination
```csharp
// Gets a paginated list of items
PaginatedResult<Product> paginatedResult = dao.GetPaginated<Product>(0, 20, x => x.Price > 20);

// Gets a paginated list of items and loads it's related images
PaginatedResult<Product> paginatedResult = dao
        .GetPaginated<Product>(0, 20, x => x.Price > 20, x => x.Images);
```



- Instanciate new DAO object with your EntityFramework connectionString
    -   There are 3 different ways to instantiate your data access object
```csharp
// New DAO without any Connection String
// It takes by default the connection string you have defined in your web.config under <connectionString>
// If you only have one data source and no plan to add another, this is the most simple way to go
DAO dao = new DAO();
```
```csharp
// New DAO with Connection String ID
// It takes as a parameter the connection string ID corresponding with one you have defined in the web.config
// It's more explicit and it's the way to go if you have more than one connectionString defined under <connectionString>
DAO dao = new DAO("DatabaseEntityID");
```
```csharp
// New DAO with Connectrion String
// It takes as a parameter the raw connectionString
string connectionString = "metadata=res://*/EntityFrameworkModel.csdl|res://*/EntityFrameworkModel.ssdl|res://*/EntityFrameworkModel.msl;provider=System.Data.SqlClient;provider connection string=&quot;data source=localhost;initial catalog=DATABASENAME;persist security info=True;user id=sa;password=SuperSecurePassword;MultipleActiveResultSets=True;App=EntityFramework&quot;"
DAO dao = new DAO(connectionString);
```

Once you have your DAO instantiated, you can perform any of the following operations:

## Current supported operations

- **CRUD**
    - **Get**-> Retrieve one entity
    - **GetList** -> Retrieve a list matching a certain criteria
    - **Add** -> Adds multiple o single entities
    - **Update** -> Update one or multiple entities
    - **Save** -> Automatically Adds or Updates
    - **Remove** -> Deletes entities that matches criteria
- **Agrregate Functions**
    - **Count**
    - **Avg**
    - **Max**
    - **Min**
    - **Sum**
- **Shorcuts / Utils**
    - **Exists** -> Like Any() but with another name xd
    - **Execute** -> Executes Typed or Untyped RAW SQL Query



### Super simple example:

```csharp
// Import both models and XL.DataAccess
using YourEntityFrameworkModels;
using XL.DataAccess;

// Init DAO with Entity Framework connectionString (This can be the ID referencing web.config or complete connectionString data)
DAO dao = new DAO("entityFrameworkNameOrConnectionString");

// Example for retrieving and updating "User" entity
User user = dao.Get<User>(x => x.UserID == 1); // Get
user.Name = "Test"; // Modify
dao.Update(user); // Update entity
```


### Not so simple example:

Every CRUD method recieves 2 params:
- **where**: A linq expression for matching criteria
- **navigationProperties**: What? Nah, easy. A list of related properties that you want loaded

```csharp
// Import both models and XL.DataAccess
using YourEntityFrameworkModels;
using XL.DataAccess;

// Init DAO with Entity Framework connectionString (This can be the ID referencing web.config or complete connectionString data)
DAO dao = new DAO("entityFrameworkNameOrConnectionString");

```

