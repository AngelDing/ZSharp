CREATE TABLE [dbo].[T_SequenceSetting]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[SequenceId] VARCHAR(50) NOT NULL , 
    [SequenceName] VARCHAR(128) NULL, 
    [RuleOrder] INT NOT NULL, 
    [RuleName] VARCHAR(50) NOT NULL, 
    [RuleValue] VARCHAR(50) NULL, 
    [PaddingSide] VARCHAR(50) NULL, 
    [PaddingWidth] INT NOT NULL, 
    [PaddingChar] CHAR NULL,
	[AddTime] DATETIME NOT NULL, 
    [AddUser] VARCHAR(50) NOT NULL, 
    [ModifyUser] VARCHAR(50) NULL, 
    [ModifyTime] DATETIME NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'编号规则',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'Id'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'编号名称',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'SequenceName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'规则排序',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'RuleOrder'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'规则类别',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'RuleName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'规则参数',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'RuleValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'补齐方向',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'PaddingSide'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'不起宽度',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'PaddingWidth'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'填充字符',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'PaddingChar'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'规则Id',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'T_SequenceSetting',
    @level2type = N'COLUMN',
    @level2name = N'SequenceId'