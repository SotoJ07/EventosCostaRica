-- Create the database
CREATE DATABASE EventosCostaRicaDb;
GO

USE EventosCostaRicaDb;
GO

-- Create Roles table
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(50) NOT NULL
);

-- Create Usuarios table
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
	Apellidos NVARCHAR(100) NULL,
    Correo NVARCHAR(100) NOT NULL UNIQUE,
    ContraseñaHash NVARCHAR(255) NOT NULL,
    RolId INT NOT NULL,
    Estado BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (RolId) REFERENCES Roles(Id)
);

ALTER TABLE Usuarios
ADD Apellidos NVARCHAR(100) NULL;

-- Seed roles for Movie Theater
INSERT INTO Roles (Nombre) VALUES ('Admin');
INSERT INTO Roles (Nombre) VALUES ('Guest');