-- Use dbo schema for all tables
SET NOCOUNT ON;

USE [AuthorizationDB];
GO

-- Applications
INSERT INTO dbo.Applications (Id, Code, Name, Description, SystemDate, ModifiedBy, RowVersion)
VALUES 
  ('11111111-1111-1111-1111-111111111111', 'MoneyTransferService', 'Money Transfer Service', 'Microservice for money transfer operations', GETUTCDATE(), 'seed', 0x),
  ('11111111-1111-1111-1111-111111111112', 'DocumentService', 'Document Service', 'Document Service Operations operations', GETUTCDATE(), 'seed', 0x);

-- Resources
INSERT INTO dbo.Resources (Id, ApplicationId, Name, Description, SystemDate, ModifiedBy, RowVersion)
VALUES
  ('22222222-2222-2222-2222-222222222222', '11111111-1111-1111-1111-111111111111', 'EFT', 'Electronic Funds Transfer', GETUTCDATE(), 'seed', 0x),
  ('22222222-2222-2222-2222-222222222223', '11111111-1111-1111-1111-111111111111', 'Havale', 'Wire Transfer', GETUTCDATE(), 'seed', 0x),
  ('22222222-2222-2222-2222-222222222224', '11111111-1111-1111-1111-111111111111', 'FAST', 'Instant and Continuous Funds Transfer', GETUTCDATE(), 'seed', 0x),
  ('22222222-2222-2222-2222-222222222225', '11111111-1111-1111-1111-111111111111', 'SWIFT', 'International Wire Transfer', GETUTCDATE(), 'seed', 0x),
  ('22222222-2222-2222-2222-222222222227', '11111111-1111-1111-1111-111111111112', 'BulkTransfer', 'Bulk Transfer Service', GETUTCDATE(), 'seed', 0x);

-- Actions
INSERT INTO dbo.Actions (Id, Name, Description, SystemDate, ModifiedBy, RowVersion)
VALUES
  ('33333333-3333-3333-3333-333333333331', 'Create', 'Create operation', GETUTCDATE(), 'seed', 0x),
  ('33333333-3333-3333-3333-333333333332', 'Read', 'Read operation', GETUTCDATE(), 'seed', 0x),
  ('33333333-3333-3333-3333-333333333333', 'Update', 'Update operation', GETUTCDATE(), 'seed', 0x),
  ('33333333-3333-3333-3333-333333333334', 'Delete', 'Delete operation', GETUTCDATE(), 'seed', 0x),
  ('33333333-3333-3333-3333-3333333333AA', 'All', 'All actions', GETUTCDATE(), 'seed', 0x);

-- Roles
INSERT INTO dbo.Roles (Id, Name, Description, SystemDate, ModifiedBy, RowVersion)
VALUES
  ('44444444-4444-4444-4444-444444444444', 'Admin', 'Administrator', GETUTCDATE(), 'seed', 0x),
  ('44444444-4444-4444-4444-444444444445', 'Operator', 'Operator', GETUTCDATE(), 'seed', 0x),
  ('44444444-4444-4444-4444-444444444446', 'Auditor', 'Auditor', GETUTCDATE(), 'seed', 0x),
  ('44444444-4444-4444-4444-444444444447', 'Finance', 'Finance Department', GETUTCDATE(), 'seed', 0x);

-- Users
INSERT INTO dbo.Users (Id, Username, Department, SystemDate, ModifiedBy, RowVersion)
VALUES
  ('55555555-5555-5555-5555-555555555555', 'admin', 'IT', GETUTCDATE(), 'seed', 0x),
  ('55555555-5555-5555-5555-555555555556', 'operator', 'Operations', GETUTCDATE(), 'seed', 0x),
  ('55555555-5555-5555-5555-555555555557', 'auditor', 'Audit', GETUTCDATE(), 'seed', 0x),
  ('55555555-5555-5555-5555-555555555558', 'finance1', 'Finance', GETUTCDATE(), 'seed', 0x);

-- UserRoles
INSERT INTO dbo.UserRoles (UserId, RoleId, SystemDate, ModifiedBy)
VALUES
  ('55555555-5555-5555-5555-555555555555', '44444444-4444-4444-4444-444444444444', GETUTCDATE(), 'seed'),
  ('55555555-5555-5555-5555-555555555556', '44444444-4444-4444-4444-444444444445', GETUTCDATE(), 'seed'),
  ('55555555-5555-5555-5555-555555555557', '44444444-4444-4444-4444-444444444446', GETUTCDATE(), 'seed'),
  ('55555555-5555-5555-5555-555555555558', '44444444-4444-4444-4444-444444444447', GETUTCDATE(), 'seed');

-- Workgroups
INSERT INTO dbo.Workgroups (Id, Name, ParentId, Description, SystemDate, ModifiedBy, RowVersion)
VALUES
  ('77777777-7777-7777-7777-777777777777', 'FinanceGroup', NULL, 'Finance Workgroup', GETUTCDATE(), 'seed', 0x),
  ('77777777-7777-7777-7777-777777777778', 'ITGroup', NULL, 'IT Workgroup', GETUTCDATE(), 'seed', 0x),
  ('77777777-7777-7777-7777-777777777779', 'AuditGroup', NULL, 'Audit Workgroup', GETUTCDATE(), 'seed', 0x);

-- UserWorkgroups
INSERT INTO dbo.UserWorkgroups (UserId, WorkgroupId, SystemDate, ModifiedBy)
VALUES
  ('55555555-5555-5555-5555-555555555555', '77777777-7777-7777-7777-777777777778', GETUTCDATE(), 'seed'),
  ('55555555-5555-5555-5555-555555555558', '77777777-7777-7777-7777-777777777777', GETUTCDATE(), 'seed'),
  ('55555555-5555-5555-5555-555555555557', '77777777-7777-7777-7777-777777777779', GETUTCDATE(), 'seed');

-- Permissions (her resource için 4 action, toplam 5x4=20 permission)
INSERT INTO dbo.Permissions (Id, ResourceId, ActionId, Code, Description, SystemDate, ModifiedBy, RowVersion)
VALUES
  -- EFT
  ('66666666-6666-6666-6666-666666666661', '22222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-333333333331', 'EFT.Create', 'Permission to create EFT', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666662', '22222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-333333333332', 'EFT.Read', 'Permission to read EFT', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666663', '22222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-333333333333', 'EFT.Update', 'Permission to update EFT', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666664', '22222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-333333333334', 'EFT.Delete', 'Permission to delete EFT', GETUTCDATE(), 'seed', 0x),
  -- All actions for EFT
  ('66666666-6666-6666-6666-6666666666AA', '22222222-2222-2222-2222-222222222222', '33333333-3333-3333-3333-3333333333AA', 'EFT.All', 'All actions for EFT', GETUTCDATE(), 'seed', 0x),

  -- Havale
  ('66666666-6666-6666-6666-666666666665', '22222222-2222-2222-2222-222222222223', '33333333-3333-3333-3333-333333333331', 'Havale.Create', 'Permission to create Havale', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666666', '22222222-2222-2222-2222-222222222223', '33333333-3333-3333-3333-333333333332', 'Havale.Read', 'Permission to read Havale', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666667', '22222222-2222-2222-2222-222222222223', '33333333-3333-3333-3333-333333333333', 'Havale.Update', 'Permission to update Havale', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666668', '22222222-2222-2222-2222-222222222223', '33333333-3333-3333-3333-333333333334', 'Havale.Delete', 'Permission to delete Havale', GETUTCDATE(), 'seed', 0x),
  -- FAST
  ('66666666-6666-6666-6666-666666666669', '22222222-2222-2222-2222-222222222224', '33333333-3333-3333-3333-333333333331', 'FAST.Create', 'Permission to create FAST', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666670', '22222222-2222-2222-2222-222222222224', '33333333-3333-3333-3333-333333333332', 'FAST.Read', 'Permission to read FAST', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666671', '22222222-2222-2222-2222-222222222224', '33333333-3333-3333-3333-333333333333', 'FAST.Update', 'Permission to update FAST', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666672', '22222222-2222-2222-2222-222222222224', '33333333-3333-3333-3333-333333333334', 'FAST.Delete', 'Permission to delete FAST', GETUTCDATE(), 'seed', 0x),
  -- SWIFT
  ('66666666-6666-6666-6666-666666666673', '22222222-2222-2222-2222-222222222225', '33333333-3333-3333-3333-333333333331', 'SWIFT.Create', 'Permission to create SWIFT', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666674', '22222222-2222-2222-2222-222222222225', '33333333-3333-3333-3333-333333333332', 'SWIFT.Read', 'Permission to read SWIFT', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666675', '22222222-2222-2222-2222-222222222225', '33333333-3333-3333-3333-333333333333', 'SWIFT.Update', 'Permission to update SWIFT', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666676', '22222222-2222-2222-2222-222222222225', '33333333-3333-3333-3333-333333333334', 'SWIFT.Delete', 'Permission to delete SWIFT', GETUTCDATE(), 'seed', 0x),
  -- BulkTransfer
  ('66666666-6666-6666-6666-666666666681', '22222222-2222-2222-2222-222222222227', '33333333-3333-3333-3333-333333333331', 'BulkTransfer.Create', 'Permission to create BulkTransfer', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666682', '22222222-2222-2222-2222-222222222227', '33333333-3333-3333-3333-333333333332', 'BulkTransfer.Read', 'Permission to read BulkTransfer', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666683', '22222222-2222-2222-2222-222222222227', '33333333-3333-3333-3333-333333333333', 'BulkTransfer.Update', 'Permission to update BulkTransfer', GETUTCDATE(), 'seed', 0x),
  ('66666666-6666-6666-6666-666666666684', '22222222-2222-2222-2222-222222222227', '33333333-3333-3333-3333-333333333334', 'BulkTransfer.Delete', 'Permission to delete BulkTransfer', GETUTCDATE(), 'seed', 0x);
  
-- PermissionAssignments (örnek: Admin tüm permissionlara sahip, Operator sadece EFT ve Havale işlemlerine sahip, Auditor sadece Read, Finance sadece Create/Update)
-- Admin: all permissions
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
SELECT NEWID(), 'Role', NULL, '44444444-4444-4444-4444-444444444444', NULL, Id, GETUTCDATE(), 'seed'
FROM dbo.Permissions;

-- Operator: only EFT and Havale permissions (all actions)
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
SELECT NEWID(), 'Role', NULL, '44444444-4444-4444-4444-444444444445', NULL, Id, GETUTCDATE(), 'seed'
FROM dbo.Permissions
WHERE Code LIKE 'EFT.%' OR Code LIKE 'Havale.%';

-- Auditor: only Read permissions
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
SELECT NEWID(), 'Role', NULL, '44444444-4444-4444-4444-444444444446', NULL, Id, GETUTCDATE(), 'seed'
FROM dbo.Permissions
WHERE Code LIKE '%.Read';

-- Finance: only Create and Update permissions
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
SELECT NEWID(), 'Role', NULL, '44444444-4444-4444-4444-444444444447', NULL, Id, GETUTCDATE(), 'seed'
FROM dbo.Permissions
WHERE Code LIKE '%.Create' OR Code LIKE '%.Update';

-- User tipinde: üç farklı kullanıcıya üç farklı permission
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
VALUES 
  (NEWID(), 'User', '55555555-5555-5555-5555-555555555555', NULL, NULL, '66666666-6666-6666-6666-666666666670', GETUTCDATE(), 'seed'), -- admin -> FAST.Read
  (NEWID(), 'User', '55555555-5555-5555-5555-555555555556', NULL, NULL, '66666666-6666-6666-6666-666666666662', GETUTCDATE(), 'seed'), -- operator -> EFT.Read
  (NEWID(), 'User', '55555555-5555-5555-5555-555555555558', NULL, NULL, '66666666-6666-6666-6666-666666666666', GETUTCDATE(), 'seed'); -- finance1 -> Havale.Read

-- Workgroup tipinde: üç farklı workgroup'a üç farklı permission
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
VALUES 
  (NEWID(), 'Workgroup', NULL, NULL, '77777777-7777-7777-7777-777777777777', '66666666-6666-6666-6666-666666666674', GETUTCDATE(), 'seed'), -- FinanceGroup -> SWIFT.Read
  (NEWID(), 'Workgroup', NULL, NULL, '77777777-7777-7777-7777-777777777778', '66666666-6666-6666-6666-666666666682', GETUTCDATE(), 'seed'), -- ITGroup -> BulkTransfer.Read
  (NEWID(), 'Workgroup', NULL, NULL, '77777777-7777-7777-7777-777777777779', '66666666-6666-6666-6666-666666666666', GETUTCDATE(), 'seed'); -- AuditGroup -> Havale.Read

-- RoleWorkgroup tipinde: üç farklı kombinasyona üç farklı permission
INSERT INTO dbo.PermissionAssignments (Id, SubjectType, UserId, RoleId, WorkgroupId, PermissionId, SystemDate, ModifiedBy)
VALUES 
  (NEWID(), 'RoleWorkgroup', NULL, '44444444-4444-4444-4444-444444444445', '77777777-7777-7777-7777-777777777777', '66666666-6666-6666-6666-666666666671', GETUTCDATE(), 'seed'), -- Operator + FinanceGroup -> FAST.Update
  (NEWID(), 'RoleWorkgroup', NULL, '44444444-4444-4444-4444-444444444447', '77777777-7777-7777-7777-777777777778', '66666666-6666-6666-6666-666666666683', GETUTCDATE(), 'seed'), -- Finance + ITGroup -> BulkTransfer.Update
  (NEWID(), 'RoleWorkgroup', NULL, '44444444-4444-4444-4444-444444444444', '77777777-7777-7777-7777-777777777779', '66666666-6666-6666-6666-666666666673', GETUTCDATE(), 'seed'); -- Admin + AuditGroup -> SWIFT.Create

