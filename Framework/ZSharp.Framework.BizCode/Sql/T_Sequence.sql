CREATE TABLE [dbo].[T_Sequence]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [SequenceName] VARCHAR(128) NOT NULL, 
    [SequenceDelimiter] VARCHAR(50) NULL, 
    [SequenceReset] VARCHAR(50) NULL, 
    [Step] INT NOT NULL, 
    [CurrentNo] INT NOT NULL, 
    [CurrentCode] VARCHAR(128) NULL, 
    [CurrentReset] VARCHAR(50) NULL, 
    [AddTime] DATETIME NOT NULL, 
    [AddUser] VARCHAR(50) NOT NULL, 
    [ModifyUser] VARCHAR(50) NULL, 
    [ModifyTime] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'编号规则主表Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'SequenceName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'分割符号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'SequenceDelimiter'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'序号重置规则',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'SequenceReset'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'步长',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'Step'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'当前序号',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'CurrentNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'当前编码',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'CurrentCode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'当前重置依赖',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_Sequence',
    @level2type = N'COLUMN',
    @level2name = N'CurrentReset'