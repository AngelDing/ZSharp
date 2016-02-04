using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

/// <summary>
/// 通用的业务编码规则设计实现,具体使用时需要根据项目进行重构，此仅为设计思路
/// http://www.cnblogs.com/xqin/p/3708367.html
/// </summary>
namespace ZSharp.Framework.BizCode
{
    public class Sequence : ISequence
    {
        private SequenceContext _context;
        public Sequence(string name)
        {
            _context = new SequenceContext();
            _context.TenantID = "";
            _context.SequenceName = name;
        }

        public ISequence SetDbContext(ISequenceRepository repo)
        {
            if (repo != null)
                _context.Repo = repo;
            return this;
        }

        public ISequence SetTenantID(string tenantId)
        {
            _context.TenantID = tenantId;
            return this;
        }

        public ISequence SetValues(Dictionary<string, object> row)
        {
            _context.row = row;
            return this;
        }

        public ISequence SetDelimiter(string delimiter)
        {
            if (!string.IsNullOrWhiteSpace(delimiter))
                _context.SequenceDelimiter = delimiter;
            return this;

        }
        public ISequence SetCurrentNo(int no)
        {
            if (no >= 0)
                _context.CurrentNo = no;
            return this;
        }
        public ISequence SetCurrentCode(string currentCode)
        {
            if (!string.IsNullOrWhiteSpace(currentCode))
                _context.CurrentCode = currentCode;
            return this;
        }
        public ISequence SetStep(int setp)
        {
            if (setp > 0)
                _context.Step = setp;
            return this;
        }
        public ISequence SetCurrentReset(string reset)
        {
            if (!string.IsNullOrWhiteSpace(reset))
                _context.CurrentReset = reset;
            return this;
        }
        public ISequence SetValues(JToken row)
        {
            if (row != null)
                foreach (var jToken in row.Children())
                {
                    var item = (JProperty)jToken;
                    if (item != null) _context.row[item.Name] = ((JValue)item.Value).Value;
                }

            return this;
        }

        public ISequence SetValue(string name, object value)
        {
            if (!string.IsNullOrEmpty(name))
                _context.row[name] = value;
            return this;
        }

        public ISequence SetRules(IList<SequenceSettingEntity> rules)
        {
            if (rules.Any())
            {
                foreach (var v in rules)
                {
                    var rule = SequenceRuleFactory.CreateRule(v.RuleName);
                    if (v.PaddingChar != null) rule.PaddingChar = (char)v.PaddingChar;
                    rule.RuleValue = v.RuleValue;
                    if (!string.IsNullOrWhiteSpace(v.PaddingSide))
                    {
                        rule.PaddingSide = (PaddingSide)Enum.Parse(typeof(PaddingSide), v.PaddingSide, true);

                    }
                    rule.PaddingWidth = v.PaddingWidth;
                    _context.Rules.Add(rule);

                }
            }
            return this;
        }

        public ISequence SetReset(string resetName)
        {
            if (!string.IsNullOrWhiteSpace(resetName))
                _context.SequenceReset = SequenceResetFactory.CreateReset(resetName);
            return this;
        }

        public string Next()
        {
            return Next(1);
        }

        public string Next(int qty)
        {
            var result = string.Empty;

            try
            {
                if (_context.Repo == null)
                {
                    //_context.Repo = new DbContext();
                }

                //加载Sequence重置依赖
                var reset = _context.Repo.GetBy(v => v.SequenceName == _context.SequenceName).FirstOrDefault();
                if (reset != null)
                {
                    SetReset(reset.SequenceReset);
                    //初始化Sequence数据
                    SetDelimiter(reset.SequenceDelimiter);
                    SetStep(reset.Step);
                    SetCurrentCode(reset.CurrentCode);
                    SetCurrentNo(reset.CurrentNo);
                    SetCurrentReset(reset.CurrentReset);
                }
                //加载Sequence规则
                var listRule = _context.Repo.GetSettingList(_context.SequenceName);;
                SetRules(listRule);

                //重置规则
                if (_context.SequenceReset != null)
                    _context.SequenceReset.Dependency(_context);
                //生成Sequence处理
                for (var i = 0; i < qty; i++)
                {
                    _context.CurrentCode = string.Empty;
                    foreach (var rule in _context.Rules)
                        _context.CurrentCode += (_context.CurrentCode.Length > 0 ? _context.SequenceDelimiter : string.Empty)
                        + rule.Series(_context);

                    result += (result.Length > 0 ? "," : string.Empty) + _context.CurrentCode;
                }

                //更新 CurrentNo
                reset.SetUpdate(() => reset.CurrentNo, _context.CurrentNo);
                reset.SetUpdate(() => reset.CurrentCode, _context.CurrentCode);
                reset.SetUpdate(() => reset.CurrentReset, _context.CurrentReset);
                //repoContext.Commit();

            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
    }
}
