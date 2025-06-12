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
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Resources tablosu
CREATE TABLE Resources (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ApplicationId UNIQUEIDENTIFIER NOT NULL,
    Name VARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Resources_Applications FOREIGN KEY (ApplicationId) REFERENCES Applications(Id)
);
CREATE UNIQUE INDEX IX_Resources_ApplicationId_Name ON Resources(ApplicationId, Name);
CREATE INDEX IX_Resources_ApplicationId ON Resources(ApplicationId);

-- Actions tablosu
CREATE TABLE Actions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Permissions tablosu
CREATE TABLE Permissions (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    ResourceId UNIQUEIDENTIFIER NOT NULL,
    ActionId UNIQUEIDENTIFIER NOT NULL,
    Code VARCHAR(200) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Permissions_Resources FOREIGN KEY (ResourceId) REFERENCES Resources(Id),
    CONSTRAINT FK_Permissions_Actions FOREIGN KEY (ActionId) REFERENCES Actions(Id)
);
CREATE INDEX IX_Permissions_ResourceId_ActionId ON Permissions(ResourceId, ActionId);

-- Roles tablosu
CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0
);

-- Users tablosu
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    UserType VARCHAR(50) NOT NULL,
    Department VARCHAR(100) NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    ExternalId VARCHAR(100) NULL,
    BranchCode VARCHAR(20) NULL,
    CustomerNo VARCHAR(20) NULL
);
CREATE INDEX IX_Users_UserType ON Users(UserType);
CREATE INDEX IX_Users_Department ON Users(Department);
CREATE INDEX IX_Users_BranchCode ON Users(BranchCode);
CREATE INDEX IX_Users_CustomerNo ON Users(CustomerNo);

-- Workgroups tablosu
CREATE TABLE Workgroups (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    ParentId UNIQUEIDENTIFIER NULL,
    Description NVARCHAR(MAX) NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    RowVersion VARBINARY(MAX) NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Workgroups_Parent FOREIGN KEY (ParentId) REFERENCES Workgroups(Id)
);
CREATE INDEX IX_Workgroups_ParentId ON Workgroups(ParentId);

-- UserRoles tablosu
CREATE TABLE UserRoles (
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT PK_UserRoles PRIMARY KEY (UserId, RoleId),
    CONSTRAINT FK_UserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
CREATE INDEX IX_UserRoles_RoleId ON UserRoles(RoleId);

-- UserWorkgroups tablosu
CREATE TABLE UserWorkgroups (
    UserId UNIQUEIDENTIFIER NOT NULL,
    WorkgroupId UNIQUEIDENTIFIER NOT NULL,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT PK_UserWorkgroups PRIMARY KEY (UserId, WorkgroupId),
    CONSTRAINT FK_UserWorkgroups_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_UserWorkgroups_Workgroups FOREIGN KEY (WorkgroupId) REFERENCES Workgroups(Id)
);
CREATE INDEX IX_UserWorkgroups_WorkgroupId ON UserWorkgroups(WorkgroupId);

-- PermissionAssignments tablosu
CREATE TABLE PermissionAssignments (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    AssignmentType VARCHAR(50) NOT NULL, -- 'User', 'Role', 'Workgroup', 'RoleWorkgroup'
    UserId UNIQUEIDENTIFIER NULL,
    RoleId UNIQUEIDENTIFIER NULL,
    WorkgroupId UNIQUEIDENTIFIER NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    ValidFrom DATETIME NULL,
    ValidTo DATETIME NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME NOT NULL,
    CreatedBy VARCHAR(100) NOT NULL,
    UpdatedDate DATETIME NULL,
    ModifiedBy VARCHAR(100) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CONSTRAINT FK_PermissionAssignments_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_PermissionAssignments_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    CONSTRAINT FK_PermissionAssignments_Workgroups FOREIGN KEY (WorkgroupId) REFERENCES Workgroups(Id),
    CONSTRAINT FK_PermissionAssignments_Permissions FOREIGN KEY (PermissionId) REFERENCES Permissions(Id)
);
CREATE INDEX IX_PermissionAssignments_UserId ON PermissionAssignments(UserId);
CREATE INDEX IX_PermissionAssignments_RoleId ON PermissionAssignments(RoleId);
CREATE INDEX IX_PermissionAssignments_WorkgroupId ON PermissionAssignments(WorkgroupId);
CREATE INDEX IX_PermissionAssignments_PermissionId ON PermissionAssignments(PermissionId);
CREATE INDEX IX_PermissionAssignments_UserId_IsActive_IsDeleted ON PermissionAssignments(UserId, IsActive, IsDeleted);
CREATE INDEX IX_PermissionAssignments_RoleId_IsActive_IsDeleted ON PermissionAssignments(RoleId, IsActive, IsDeleted);
CREATE INDEX IX_PermissionAssignments_RoleId_WorkgroupId ON PermissionAssignments(RoleId, WorkgroupId);
CREATE INDEX IX_PermissionAssignments_RoleId_WorkgroupId_IsActive_IsDeleted ON PermissionAssignments(RoleId, WorkgroupId, IsActive, IsDeleted);