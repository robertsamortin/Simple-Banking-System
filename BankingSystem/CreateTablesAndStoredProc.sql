USE [Banking]
GO
/****** Object:  Table [dbo].[Transactions]    Script Date: 06/19/2018 2:04:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transactions](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[AccountNumber] [nvarchar](15) NULL,
	[Amount] [decimal](18, 2) NULL,
	[TransType] [nchar](15) NULL,
	[TransDate] [datetime] NULL,
	[TransBy] [nchar](20) NULL,
 CONSTRAINT [PK_transactions] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [nchar](20) NULL,
	[AccountNumber] [nvarchar](15) NULL,
	[Password] [nvarchar](20) NULL,
	[Balance] [decimal](18, 2) NULL,
	[CreatedDate] [datetime] NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_UniqueLoginName] UNIQUE NONCLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  StoredProcedure [dbo].[spUserCheckBalanceIfEqual]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserCheckBalanceIfEqual]
(      
   @AccountNumber nchar(20)
)     
as    
Begin    
    select Balance    
    from users  where   AccountNumber = @AccountNumber 
End
GO
/****** Object:  StoredProcedure [dbo].[spUserCheckLoginName]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[spUserCheckLoginName]
(      
   @LoginName nchar(20)
)     
as    
Begin    
    select *    
    from users  where   LoginName = @LoginName 
End
GO
/****** Object:  StoredProcedure [dbo].[spUserGetByAccountNumber]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserGetByAccountNumber]  
(      
   @AccountNumber nvarchar(16)      
)     
as    
Begin    
    select *    
    from Users  where AccountNumber = @AccountNumber
End   
GO
/****** Object:  StoredProcedure [dbo].[spUserGetByLoginName]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserGetByLoginName]  
(      
   @LoginName nvarchar(15)      
)     
as    
Begin    
    select *    
    from users  where   LoginName = @LoginName
End   
GO
/****** Object:  StoredProcedure [dbo].[spUserInsert]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[spUserInsert]     
(    
    @LoginName nchar(20),     
    @AccountNumber nvarchar(15),    
    @Password nvarchar(20),    
    @Balance decimal(18,2),
	@CreatedDate datetime    
)    
as     
Begin     
    Insert into users (LoginName,AccountNumber,[Password], Balance, CreatedDate)     
    Values (@LoginName,@AccountNumber,@Password, @Balance, @CreatedDate)     
End 
GO
/****** Object:  StoredProcedure [dbo].[spUserLogin]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserLogin]
(      
   @LoginName nchar(20),
   @Password nvarchar(20)     
)     
as    
Begin    
    select *    
    from users  where   LoginName = @LoginName COLLATE SQL_Latin1_General_CP1_CS_AS and [Password] = @Password
	COLLATE SQL_Latin1_General_CP1_CS_AS
End   
GO
/****** Object:  StoredProcedure [dbo].[spUserTransactionsFundTransfer]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[spUserTransactionsFundTransfer]     
(     
    @AccountNumber nvarchar(15),    
    @Amount decimal(18,2),    
    @TransType nchar(15),
	@TransDate datetime,
	@TransBy nchar(20)    
)    
as     
Begin     
    Insert into Transactions(AccountNumber,Amount, TransType, TransDate, TransBy)     
    Values (@AccountNumber,@Amount,@TransType, @TransDate,@TransBy)
	
	Update Users set Balance = Balance + @Amount where AccountNumber = @AccountNumber
	
	Update Users set Balance = Balance + @Amount where AccountNumber = @TransBy
End 
GO
/****** Object:  StoredProcedure [dbo].[spUserTransactionsGetAll]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserTransactionsGetAll] 
(
	@AccountNumber nvarchar(15)
)
AS
Begin     
    Select ISnull(a.ID,0) ID, b.AccountNumber, 
	isnull(a.Amount,0) Amount, a.TransType, isnull(a.TransDate, GETDATE()) TransDate, a.TransBy, b.Balance,
	isnull(SUM(Amount) OVER ( PARTITION BY a.AccountNumber ORDER BY TransDate ),0) AS RunningBalance
	from Transactions a
	right join Users b
	on a.AccountNumber = b.AccountNumber
	 where b.AccountNumber = @AccountNumber 
	order by id desc     
End 
GO
/****** Object:  StoredProcedure [dbo].[spUserTransactionsInsert]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[spUserTransactionsInsert]     
(     
    @AccountNumber nvarchar(15),    
    @Amount decimal(18,2),    
    @TransType nchar(15),
	@TransDate datetime,
	@TransBy nchar(20)    
)    
as     
Begin     
    
	
	if @TransType = 'Fund Transfer'
		begin
			Insert into Transactions(AccountNumber,Amount, TransType, TransDate, TransBy)     
			Values (@AccountNumber,@Amount * -1,@TransType, @TransDate,@TransBy)

			Insert into Transactions(AccountNumber,Amount, TransType, TransDate, TransBy)     
			Values (@TransBy,@Amount,@TransType, @TransDate,@TransBy)

			Update Users set Balance = Balance + @Amount  where AccountNumber = @TransBy
			
			Update Users set Balance = Balance + (@Amount * -1)  where AccountNumber = @AccountNumber	
		end
	else	
		begin
			Insert into Transactions(AccountNumber,Amount, TransType, TransDate, TransBy)     
			Values (@AccountNumber,@Amount,@TransType, @TransDate,@TransBy)

			Update Users set Balance = Balance + @Amount where AccountNumber = @AccountNumber	
		end
End 
GO
/****** Object:  StoredProcedure [dbo].[spUserUpdate]    Script Date: 06/19/2018 2:04:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[spUserUpdate]
(    
	@ID int,
    @LoginName nchar(20),     
    @AccountNumber nvarchar(15),    
    @Password nvarchar(20),    
    @Balance decimal(18,2),
	@CreatedDate datetime    
)    
as     
Begin     
    Update users set
		LoginName = @LoginName,
		AccountNumber = @AccountNumber,
		[Password] = @Password, 
		Balance = @Balance, 
		CreatedDate = @CreatedDate
	Where ID = @ID
End 
GO
