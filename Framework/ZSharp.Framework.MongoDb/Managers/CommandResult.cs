using MongoDB.Bson;
using System;

namespace ZSharp.Framework.MongoDb.Managers
{
    [Serializable]
    public class CommandResult
    {
        private BsonDocument response;

        public CommandResult(BsonDocument response)
        {
            this.response = response;
        }

        public int? Code
        {
            get { return response.GetValue("code", BsonNull.Value).AsNullableInt32; }
        }

        public BsonDocument Response
        {
            get { return response; }
        }

        public string ErrorMessage
        {
            get
            {
                BsonValue ok;
                if (response.TryGetValue("ok", out ok) && ok.ToBoolean())
                {
                    return null;
                }
                else
                {
                    BsonValue errmsg;
                    if (response.TryGetValue("errmsg", out errmsg) && !errmsg.IsBsonNull)
                    {
                        return errmsg.ToString();
                    }
                    else
                    {
                        return "Unknown error";
                    }
                }
            }
        }

        public bool Ok
        {
            get
            {
                BsonValue ok;
                if (response.TryGetValue("ok", out ok))
                {
                    return ok.ToBoolean();
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
