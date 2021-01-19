USE [Yocal.Invoice]
GO
SET IDENTITY_INSERT [Invoice].[Invoice] ON 

GO
INSERT [Invoice].[Invoice] ([Key], [TotalAmount], [PaidAmount], [Status],  [Id]) VALUES (3, CAST(1000.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), 0, N'0838778c-94e6-4d9a-a898-eb78b5163eee')
GO
SET IDENTITY_INSERT [Invoice].[Invoice] OFF
GO
SET IDENTITY_INSERT [Invoice].[InvoiceItemLine] ON 

GO
INSERT [Invoice].[InvoiceItemLine] ([Key], [Type], [Name], [ItemId], [Quantity], [InvoiceId], [Id]) VALUES (5, N'Product', N'Product 1', N'3bf543c8-8776-4a23-87fd-fcffb3f7ce55', 3, 3, N'20190040-e40e-411a-8b43-ba20a57bb2f1')
GO
INSERT [Invoice].[InvoiceItemLine] ([Key], [Type], [Name], [ItemId], [Quantity], [InvoiceId], [Id]) VALUES (6, N'Product', N'Product 2', N'dbf543c8-8776-4a23-87fd-fcffb3f7ce69', 5, 3, N'b0fbbfc1-f3a2-432e-a0e9-55bd336e2ce9')
GO
SET IDENTITY_INSERT [Invoice].[InvoiceItemLine] OFF
GO
/****************************************************************************************************/

USE [Yocal.Inventory]
GO
SET IDENTITY_INSERT [Inventory].[Product] ON 

GO
INSERT [Inventory].[Product] ([Key], [Name], [Quantity], [Id]) VALUES (1, N'Product1', 100, N'3bf543c8-8776-4a23-87fd-fcffb3f7ce55')
GO
INSERT [Inventory].[Product] ([Key], [Name], [Quantity], [Id]) VALUES (2, N'Product2', 200, N'dbf543c8-8776-4a23-87fd-fcffb3f7ce69')
GO
SET IDENTITY_INSERT [Inventory].[Product] OFF
GO
/****************************************************************************************************/