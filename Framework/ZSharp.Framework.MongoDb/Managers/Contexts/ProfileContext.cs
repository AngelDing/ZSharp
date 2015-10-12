//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;

//namespace ZSharp.Framework.MongoDb.Managers
//{
//    public class ProfileContext : BaseContext
//    {
//        private string sName;
//        private string dbName;
//        public DatabaseModel Database { get; private set; }

//        public ProfileContext(int id)
//        {
//            Id = id;
//            var dbNode = CacheHelper.GetTreeNode(id);
//            Database = dbNode.ModelInfo as DatabaseModel;
//            sName = Database.Server.Name;
//            dbName = Database.Name;
//        }

//        public ProfilingLevel GetProfileStatus()
//        {
//            var manager = new MongoManager(sName, dbName);
            
//            var rst = manager.GetProfilingLevel();
//            if (string.IsNullOrEmpty(rst.ErrorMessage))
//            {
//                if (rst.Ok)
//                {
//                    return rst.Level;
//                }
//                return ProfilingLevel.None;
//            }
//            else
//            {
//                throw new MongoException(rst.ErrorMessage);
//            }
//        }

//        public List<ProfileModel> GetProfileData(int limit)
//        {
//            var manager = new MongoManager(sName, dbName);
//            var cursors = manager.GetProfilingInfo(Query.Null, limit);

//            var list = new List<ProfileModel>();
//            foreach (var item in cursors)
//            {
//                list.Add(new ProfileModel
//                {
//                    Client = item.Client,
//                    Op = item.Op,
//                    Namespace = item.Namespace,
//                    Command = GetCommand(item),
//                    Timestamp = item.Timestamp,
//                    Duration = item.Duration.TotalMilliseconds,
//                    NumberToReturn = item.NumberToReturn,
//                    NumberScanned = item.NumberScanned,
//                    NumberReturned = item.NumberReturned,
//                    ResponseLength = item.ResponseLength
//                });
//            }
//            return list;
//        }

//        private string GetCommand(SystemProfileInfo info)
//        {
//            if (info.Command != null)
//            {
//                return info.Command.ToString();
//            }
//            if (info.Query != null)
//            {
//                return info.Query.ToString();
//            }
//            if (info.UpdateObject != null)
//            {
//                return info.UpdateObject.ToString();
//            }
//            return string.Empty;
//        }

//        public bool SetProfile(int level, int slowms = 100)
//        {
//            var manager = new MongoManager(sName, dbName);

//            CommandResult rst = null;
//            //if ((ProfilingLevel)level == ProfilingLevel.Slow)
//            //{
//            //    rst = manager.SetProfilingLevel((ProfilingLevel)level, new TimeSpan(0, 0, 0, 0, slowms));
//            //}
//            //else
//            //{
//            //    rst = manager.SetProfilingLevel((ProfilingLevel)level);
//            //}

//            if (string.IsNullOrEmpty(rst.ErrorMessage))
//            {
//                return true;
//            }
//            else
//            {
//                throw new MongoException(rst.ErrorMessage);
//            }
//        }
//    }
//}
