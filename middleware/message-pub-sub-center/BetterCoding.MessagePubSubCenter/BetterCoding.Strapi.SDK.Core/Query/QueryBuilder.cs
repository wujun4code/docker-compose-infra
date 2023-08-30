﻿namespace BetterCoding.Strapi.SDK.Core.Query
{
    public class QueryBuilder
    {
        protected IDictionary<string, object> _filters;
        public IDictionary<string, object> Filters => _filters;
        protected QueryBuilder Instance => this;
        public QueryBuilder()
        {
            _filters = new Dictionary<string, object>();
        }

        public virtual QueryBuilder MergerFilter(IDictionary<string, object> filter) 
        {
            _filters = new Dictionary<string, object>(MergeFilterClauses(filter));
            return Instance;
        }

        IDictionary<string, object> MergeFilterClauses(IDictionary<string, object> filter)
        {
            if (Filters is null)
            {
                return filter;
            }

            Dictionary<string, object> newWhere = new Dictionary<string, object>(Filters);
            foreach (KeyValuePair<string, object> pair in filter)
            {
                if (newWhere.ContainsKey(pair.Key))
                {
                    if (!(newWhere[pair.Key] is IDictionary<string, object> oldCondition) || !(pair.Value is IDictionary<string, object> condition))
                    {
                        throw new ArgumentException("More than one filter clause for the given key provided.");
                    }

                    Dictionary<string, object> newCondition = new Dictionary<string, object>(oldCondition);
                    foreach (KeyValuePair<string, object> conditionPair in condition)
                    {
                        if (newCondition.ContainsKey(conditionPair.Key))
                        {
                            throw new ArgumentException("More than one condition for the given key provided.");
                        }

                        newCondition[conditionPair.Key] = conditionPair.Value;
                    }

                    newWhere[pair.Key] = newCondition;
                }
                else
                {
                    newWhere[pair.Key] = pair.Value;
                }
            }
            return newWhere;
        }

        public virtual QueryBuilder EqualsTo(string field, object value)
        {
            var filter = new Dictionary<string, object> { { field, new Dictionary<string, object> { { "$eq", value } } } };
            return MergerFilter(filter);
        }

        public virtual QueryBuilder ContainedIn<TIn>(string field, IEnumerable<TIn> values)
        {
            var filter = new Dictionary<string, object> { { field, new Dictionary<string, object> { { "$in", values.ToList() } } } };
            return MergerFilter(filter);
        }

        public virtual QueryBuilder LaterThan(string field, DateTime utc)
        {
            var filter = new Dictionary<string, object> { { field, new Dictionary<string, object> { { "$lt", utc } } } };
            return MergerFilter(filter);
        }

        public virtual QueryBuilder EarlierThan(string field, DateTime utc)
        {
            var filter = new Dictionary<string, object> { { field, new Dictionary<string, object> { { "$gt", utc } } } };
            return MergerFilter(filter);
        }
    }
}
