using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Domain;

namespace Collectively.Services.Groups.Domain
{
    public static class Criteria
    {
        private static readonly ISet<string> RemarkCriteria = new HashSet<string>
        {
            "moderator", "administrator", "owner", "participant", "public" 
        };

        private static readonly ISet<string> MembershipCriteria = new HashSet<string>
        {
            "user_request", "invitation", "public"
        };

        private static readonly IDictionary<string,ISet<string>> _defaultGroupCriteria = new Dictionary<string,ISet<string>>
        {
            ["membership"] = MembershipCriteria,
            ["remark_create"] = RemarkCriteria,
            ["remark_comment_delete"] = RemarkCriteria,
            ["remark_delete"] = RemarkCriteria,
            ["remark_location"] = new HashSet<string>(),
            ["remark_resolve"] = RemarkCriteria
        };

        private static readonly IDictionary<string,ISet<string>> _defaultOrganizationCriteria = new Dictionary<string,ISet<string>>
        {
            ["membership"] = MembershipCriteria
        };

        private static readonly IDictionary<string,ISet<string>> _defaultGroup = ToDefault(_defaultGroupCriteria);
        private static readonly IDictionary<string,ISet<string>> _defaultOrganization = ToDefault(_defaultOrganizationCriteria);
        private static IDictionary<string,ISet<string>> ToDefault(IDictionary<string,ISet<string>> criteria)
            => criteria.ToDictionary(x => x.Key, x => x.Value.Any() ? 
                (ISet<string>)(new HashSet<string>{x.Value.First()}) : 
                (ISet<string>)(new HashSet<string>()));
        public static IDictionary<string,ISet<string>> DefaultGroup => _defaultGroup;
        public static IDictionary<string,ISet<string>> DefaultOrganization => _defaultOrganization;

        public static IDictionary<string,ISet<string>> MergeForOrganizationOrFail(IDictionary<string,ISet<string>> criteria)
        => MergeOrFail(criteria, DefaultOrganization, _defaultOrganizationCriteria);

        public static IDictionary<string,ISet<string>> MergeForGroupOrFail(IDictionary<string,ISet<string>> criteria)
        => MergeOrFail(criteria, DefaultGroup, _defaultGroupCriteria);

        private static IDictionary<string,ISet<string>> MergeOrFail(IDictionary<string,ISet<string>> criteria, 
            IDictionary<string,ISet<string>> mergedCriteria, IDictionary<string,ISet<string>> defaultCriteria)
        {
            foreach(var criterion in criteria)
            {
                if(!defaultCriteria.ContainsKey(criterion.Key))
                {
                    throw new DomainException(OperationCodes.InvalidCriterion, 
                        $"Invalid '{criterion.Key}' criterion.");                    
                }
                var existingCriteria = defaultCriteria[criterion.Key];
                var invalidCriteria = criterion.Value.Except(existingCriteria);
                if(existingCriteria.Any() && invalidCriteria.Any())
                {
                    throw new DomainException(OperationCodes.InvalidCriterionValues, 
                        $"Invalid '{criterion.Key}' criterion values: '{string.Join(", ", invalidCriteria)}'.");                    
                }
                var invalidCriterionValue = criterion.Value.FirstOrDefault(x => x.Length > 100);
                if(invalidCriterionValue != null)
                {
                    throw new DomainException(OperationCodes.InvalidCriterionValue, 
                        $"Invalid '{criterion.Key}' criterion value: '{invalidCriterionValue}'.");                       
                }
                if(criterion.Value.Count > 100)
                {
                    throw new DomainException(OperationCodes.TooManyCriterionValues, 
                        $"Too many '{criterion.Key}' criterion values: {criterion.Value.Count} (max: 100).");                    
                }
                mergedCriteria[criterion.Key] = criterion.Value; 
            }

            return mergedCriteria;         
        }      
    }
}