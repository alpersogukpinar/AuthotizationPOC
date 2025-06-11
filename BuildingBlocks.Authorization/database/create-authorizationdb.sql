-- Veritabanını oluştur
CREATE DATABASE AuthorizationDB;
GO

USE AuthorizationDB;
GO

-- Applications tablosu
CREATE TABLE Applications (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Code VARCHAR(100) NOT NULL UNIQUE,
    Name VARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL
);

-- Resources tablosu
CREATE TABLE Resources (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ApplicationId UNIQUEIDENTIFIER NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    CONSTRAINT FK_Resources_Applications FOREIGN KEY (ApplicationId) REFERENCES Applications(Id)
);
CREATE UNIQUE INDEX IX_Resources_ApplicationId_Name ON Resources(ApplicationId, Name);

-- Actions tablosu
CREATE TABLE Actions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL
);

-- Permissions tablosu
CREATE TABLE Permissions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ResourceId UNIQUEIDENTIFIER NOT NULL,
    ActionId UNIQUEIDENTIFIER NOT NULL,
    Code VARCHAR(200) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    CONSTRAINT FK_Permissions_Resources FOREIGN KEY (ResourceId) REFERENCES Resources(Id),
    CONSTRAINT FK_Permissions_Actions FOREIGN KEY (ActionId) REFERENCES Actions(Id)
);

-- Roles tablosu
CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL
);

-- Users tablosu
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    Department VARCHAR(100) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL
);

-- UserRoles tablosu
CREATE TABLE UserRoles (
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);

-- Workgroups tablosu
CREATE TABLE Workgroups (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    ParentId UNIQUEIDENTIFIER NULL,
    Description NVARCHAR(MAX) NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    CONSTRAINT FK_Workgroups_Parent FOREIGN KEY (ParentId) REFERENCES Workgroups(Id)
);

-- UserWorkgroups tablosu
CREATE TABLE UserWorkgroups (
    UserId UNIQUEIDENTIFIER NOT NULL,
    WorkgroupId UNIQUEIDENTIFIER NOT NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    CONSTRAINT PK_UserWorkgroups PRIMARY KEY (UserId, WorkgroupId),
    CONSTRAINT FK_UserWorkgroups_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserWorkgroups_Workgroups FOREIGN KEY (WorkgroupId) REFERENCES Workgroups(Id)
);

-- SubjectPermissions tablosu
CREATE TABLE SubjectPermissions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    SubjectType VARCHAR(50) NOT NULL, -- 'User', 'Role', 'Workgroup', 'RoleWorkgroup'
    UserId UNIQUEIDENTIFIER NULL,
    RoleId UNIQUEIDENTIFIER NULL,
    WorkgroupId UNIQUEIDENTIFIER NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    CONSTRAINT FK_SubjectPermissions_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_SubjectPermissions_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    CONSTRAINT FK_SubjectPermissions_Workgroups FOREIGN KEY (WorkgroupId) REFERENCES Workgroups(Id),
    CONSTRAINT FK_SubjectPermissions_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id)
);

-- PermissionAssignments tablosu
CREATE TABLE PermissionAssignments (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    SubjectType VARCHAR(50) NOT NULL, -- 'User', 'Role', 'Workgroup', 'RoleWorkgroup'
    UserId UNIQUEIDENTIFIER NULL,
    RoleId UNIQUEIDENTIFIER NULL,
    WorkgroupId UNIQUEIDENTIFIER NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    SystemDate DATETIME NOT NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    CONSTRAINT FK_PermissionAssignments_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_PermissionAssignments_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    CONSTRAINT FK_PermissionAssignments_Workgroups FOREIGN KEY (WorkgroupId) REFERENCES Workgroups(Id),
    CONSTRAINT FK_PermissionAssignments_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id)
);