using System.Threading.Tasks;
using MongoDB.Bson;
using ZSharp.Framework.Sequence;

namespace ZSharp.Framework.MongoDb
{
    public class MongoStateProvider : MongoRepository<Sequences, string>, IStateProvider
    {
        public MongoStateProvider(string connectionString)
            : base(connectionString)
        {
        }

        public async Task<SequenceKey> AddAsync(ISequence sequence)
        {
            var rowKey = ObjectId.GenerateNewId();
            var sequenceEntity = sequence as Sequences;
            sequenceEntity.Id = rowKey.ToString();

            await InsertAsync(sequenceEntity);

            return new SequenceKey { Value = rowKey.ToString() };
        }

        public async Task<ISequence> GetAsync(SequenceKey sequenceKey)
        {
            var sequenceEntity = await GetByKeyAsync(sequenceKey.Value);
            return sequenceEntity;
        }

        public async Task<bool> UpdateAsync(SequenceKey sequenceKey, ISequence sequence)
        {
            var sequenceEntity = sequence as Sequences; 
            sequenceEntity.SetUpdate(() => sequenceEntity.CurrentValue, sequenceEntity.CurrentValue);
            var result = await FindOneAndUpdateAsync(p => p.Id == sequenceKey.Value, sequenceEntity.NeedUpdateList);
            return result != null;
        }

        public async Task<ISequence> NewAsync(SequenceOptions options)
        {
            var sequence = new Sequences(options);

            return await Task.FromResult(sequence);
        }
    }
}
