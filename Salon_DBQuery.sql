CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(20) NOT NULL CHECK (Role IN ('Admin', 'Cashier'))
);

CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(255)
);

CREATE TABLE Products (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL UNIQUE,
    ProductPrice DECIMAL(10,2) NOT NULL,
    ProductDescription NVARCHAR(255)
);

CREATE TABLE Services (
    ServiceID INT IDENTITY(1,1) PRIMARY KEY,
    ServiceName NVARCHAR(255) NOT NULL UNIQUE,
    ServicePrice DECIMAL(10,2) NOT NULL,
    ServiceDescription NVARCHAR(500) NOT NULL,
    CategoryID INT NOT NULL,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

CREATE TABLE Discounts (
    DiscountID INT IDENTITY(1,1) PRIMARY KEY,
    DiscountName NVARCHAR(100) NOT NULL,
    DiscountDescription NVARCHAR(500) NULL,
    DiscountAmount DECIMAL(10, 2) NOT NULL
);

CREATE TABLE TechnicianRoles (
    RoleID INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE,
    RoleDescription NVARCHAR (255) NULL
);

CREATE TABLE Technicians (
    TechnicianID INT IDENTITY(1,1) PRIMARY KEY,
    TechnicianCode AS 'TECH' + RIGHT('000' + CAST(TechnicianID AS VARCHAR(3)), 3) PERSISTED,
    TechnicianName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10) NOT NULL CHECK (Gender IN ('Male', 'Female')),
    TechnicianNumber NVARCHAR(15) NOT NULL,
    TechnicianAddress NVARCHAR(255) NOT NULL,
    RoleID INT NOT NULL,
    FOREIGN KEY (RoleID) REFERENCES TechnicianRoles(RoleID)
);

CREATE TABLE Appointments (
    AppointmentID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerFirstName NVARCHAR(100) NOT NULL,
    CustomerLastName NVARCHAR(100) NOT NULL,
    CustomerContact NVARCHAR(20),
    ServiceID INT NOT NULL,
    Service1 NVARCHAR(100) NULL,
    Service2 NVARCHAR(100) NULL,
    TechnicianID INT NULL,
    AppointmentDate DATE,
    AppointmentTime NVARCHAR(20) NOT NULL,
    ReferenceNumber NVARCHAR(100) NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Confirmed', 'Completed', 'Cancelled', 'Ready for Billing')) DEFAULT 'Confirmed',
    UserID INT NOT NULL,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (ServiceID) REFERENCES Services(ServiceID),
    FOREIGN KEY (TechnicianID) REFERENCES Technicians(TechnicianID)
);

CREATE TABLE PaymentModes (
    PaymentModeID INT IDENTITY(1,1) PRIMARY KEY,
    PaymentModeName NVARCHAR(50) NOT NULL UNIQUE
);
	

CREATE TABLE Transactions (
    TransactionID     INT             IDENTITY (1, 1) NOT NULL,
    AppointmentID    INT             NOT NULL,
    PaymentModeID INT NULL,
    ReferenceNumber NVARCHAR(100) NULL,
    CustomerFirstName NVARCHAR (100)  NULL,
    CustomerLastName  NVARCHAR (100)  NULL,
    CustomerContact   NVARCHAR (20)   NULL,
    AppointmentDate   DATE            NULL,
    AppointmentTime   TIME (7)        NULL,
    Service1          NVARCHAR (100)  NULL,
    Service1Price    DECIMAL (10, 2) NULL,
    Service2         NVARCHAR (100)  NULL,
    Service2Price     DECIMAL (10, 2) NULL,
    ProductName       NVARCHAR (100)  NULL,
    ProductPrice      DECIMAL (10, 2) NULL,
    DiscountType      NVARCHAR (50)   NULL,
    DiscountAmount    DECIMAL (10, 2) NULL,
    Subtotal          DECIMAL (10, 2) NULL,
    Total             DECIMAL (10, 2) NULL,
    TransactionDate   DATETIME        DEFAULT (getdate()) NULL,
    ReportFilePath NVARCHAR(MAX) NULL,
    PRIMARY KEY CLUSTERED ([TransactionID] ASC),
    FOREIGN KEY ([AppointmentID]) REFERENCES [dbo].[Appointments] ([AppointmentID]),
    FOREIGN KEY (PaymentModeID) REFERENCES PaymentModes(PaymentModeID)
);


CREATE TABLE Queuing (
    QueueID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerName NVARCHAR(100) NOT NULL,
    ServiceID INT NOT NULL,
    QueueNumber INT NOT NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Waiting', 'In Progress', 'Done', 'Cancelled', 'Ready for Billing')) DEFAULT 'Waiting',
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (ServiceID) REFERENCES Services(ServiceID)
);

INSERT INTO PaymentModes (PaymentModeName) VALUES
('Cash'),
('GCash');

INSERT INTO Categories (CategoryName, Description) VALUES
('Hair Services', 'All haircut, styling, and hair treatments'),
('Other Services', 'Nail care, eyelashes, and other beauty services');

INSERT INTO Products (ProductName, ProductDescription, ProductPrice) VALUES
('Pomade Men', 'Strong Hold & Matte 75g', 204);

INSERT INTO Discounts (DiscountName, DiscountDescription, DiscountAmount) VALUES
('Persons with Disability', 'PWD', 100),
('Senior Citizen', 'Ages from 60 and Above', 100);


INSERT INTO Services (ServiceName, ServiceDescription, ServicePrice, CategoryID) VALUES
('Hair Cut', 'Basic haircut based on the customer’s preferred style.', 249, 1),
('Haircut w/ Shampoo & Blow Dry', 'Haircut with shampoo and blow dry after the service.', 349, 1),
('Haircolor w/ Treatment', 'Hair coloring service that includes hair treatment.', 999, 1),
('Hair Rebond', 'Straightening service that makes hair smooth and shiny.', 1499, 1),
('Keratin Treatment', 'Treatment that removes frizz and makes hair soft.', 549, 1),
('Hair Botox - Short Hair', 'Treatment for short hair to make it smooth and healthy.', 999, 1),
('Hair Botox - Medium Hair', 'Treatment for medium hair to repair and soften it.', 1199, 1),
('Hair Botox - Long Hair', 'Treatment for long hair to reduce dryness and frizz.', 1499, 1),
('Hair & Make Up', 'Hair styling and makeup for special occasions.', 1499, 1),
('Manicure', 'Cleaning, shaping, and polishing of fingernails.', 149, 2),
('Pedicure', 'Cleaning, shaping, and polishing of toenails.', 179, 2),
('Foot Spa', 'Soaking and cleaning of feet with light massage.', 349, 2),
('Mani & Pedi w/ Foot Spa', 'Combination of manicure, pedicure, and foot spa.', 349, 2),
('Manicure (Gel Polish)', 'Manicure with long-lasting gel polish.', 609, 2),
('Pedicure (Gel Polish)', 'Pedicure with long-lasting gel polish.', 349, 2),
('Soft Gel Extension', 'Nail extension using soft gel materials.', 399, 2),
('Classic Eyelash Extension', 'Adds extra lashes for a longer and fuller look.', 699, 2),
('Eyelash Lift', 'Lifts natural lashes to make them curl upward.', 199, 2),
('Eyebrow Lamination', 'Sets and shapes eyebrows to look neat and full.', 349, 2);

INSERT INTO TechnicianRoles (RoleName, RoleDescription) VALUES
('Hairstylist', 'Provides hair styles'),
('Manicurist', 'Provides Nail care service'),
('Pedicurist', 'Provides Care for feet and toenails');

INSERT INTO Technicians (TechnicianName, TechnicianNumber, TechnicianAddress, Gender, RoleID) 
VALUES
('Jennilyn Gozum', '09171234567', 'San Rafael, Tarlac City', 'Female', 1),
('Michael Bunagan', '09181234567', 'San Vicente, Tarlac City', 'Male', 2),
('Honey Surla', '09221234567', 'San Rafael, Tarlac City', 'Female', 3);