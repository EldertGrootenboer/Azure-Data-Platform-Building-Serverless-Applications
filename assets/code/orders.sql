CREATE TABLE Orders (
    id UNIQUEIDENTIFIER PRIMARY KEY default NEWID(),
    orderNumber VARCHAR (50) NOT NULL,
    orderStatus VARCHAR (50) NOT NULL,
    hubId INT NOT NULL,
    customerName VARCHAR (100) NOT NULL,
    street VARCHAR (100) NOT NULL,
    city VARCHAR (50) NOT NULL,
    zipCode VARCHAR (50) NOT NULL,
    articleCount INT NOT NULL
);
