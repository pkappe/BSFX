CREATE TABLE [dbo].[UserAuth]
(
	[UserName] VARCHAR(20) NOT NULL, 
    [Password] VARCHAR(20) NOT NULL, 
    [Allow] VARCHAR(1) NOT NULL, 
    CONSTRAINT [User_Auth] PRIMARY KEY ([UserName]) 
)
