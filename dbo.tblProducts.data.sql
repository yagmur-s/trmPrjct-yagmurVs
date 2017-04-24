SET IDENTITY_INSERT [dbo].[tblProducts] ON
INSERT INTO [dbo].[tblProducts] ([Id], [Name], [Description], [Price], [CategoryName], [CategoryId], [ImageName], [Date], [Place], [TimePeriodId], [TimePeriodName]) VALUES (1, 'Boyoz Festivali', 'Boyoz Festivali', '20.50', 'Electronics', 1, NULL, '24.04.2017', 'İstanbul', 1, 'Today')
SET IDENTITY_INSERT [dbo].[tblProducts] OFF
