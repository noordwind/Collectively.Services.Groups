using System.Collections.Generic;
using System.Linq;
using Collectively.Common.Domain;

namespace Collectively.Services.Groups.Domain
{
    public static class Criteria
    {
        private static readonly ISet<string> RemarkCriteria = new HashSet<string>
        {
            "public", "member", "moderator", "administrator", "owner" 
        };

        private static readonly ISet<string> MembershipCriteria = new HashSet<string>
        {
            "public", "user_request", "invitation"
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

        private static readonly IDictionary<string,ISet<string>> _defaultGroup = 
            _defaultGroupCriteria.ToDictionary(x => x.Key, x => (ISet<string>)(new HashSet<string>{x.Value.First()}));

        private static readonly IDictionary<string,ISet<string>> _defaultOrganization= 
            _defaultOrganizationCriteria.ToDictionary(x => x.Key, x => (ISet<string>)(new HashSet<string>{x.Value.First()}));

        public static IDictionary<string,ISet<string>> DefaultGroup => _defaultGroup;
        public static IDictionary<string,ISet<string>> DefaultOrganization => _defaultOrganization;

        public static IDictionary<string,ISet<string>> MergeForOrganizationOrFail(IDictionary<string,ISet<string>> criteria)
        => MergeOrFail(criteria, DefaultGroup);

        public static IDictionary<string,ISet<string>> MergeForGroupOrFail(IDictionary<string,ISet<string>> criteria)
        => MergeOrFail(criteria, DefaultOrganization);

        private static IDictionary<string,ISet<string>> MergeOrFail(IDictionary<string,ISet<string>> criteria, 
            IDictionary<string,ISet<string>> mergedCriteria)
        {
            ISet<string> existingCriteria;
            foreach(var criterion in criteria)
            {
                if(!mergedCriteria.TryGetValue(criterion.Key, out existingCriteria))
                {
                    throw new DomainException(OperationCodes.InvalidCriterion, 
                        $"Invalid criterion: '{criterion.Key}'.");
                }
                var invalidCriteria = criterion.Value.Except(existingCriteria);
                if(existingCriteria.Any() && invalidCriteria.Any())
                {
                    throw new DomainException(OperationCodes.InvalidCriterionValues, 
                        $"Invalid criterion values: '{string.Join(", ", invalidCriteria)}'.");                    
                }
                var invalidCriterionValue = criterion.Value.FirstOrDefault(x => x.Length > 100);
                if(invalidCriterionValue != null)
                {
                    throw new DomainException(OperationCodes.InvalidCriterionValue, 
                        $"Invalid criterion value: '{invalidCriterionValue}', for: '{criterion.Key}'.");                       
                }
                if(criterion.Value.Count > 100)
                {
                    throw new DomainException(OperationCodes.TooManyCriterionValues, 
                        $"Too many criterion values: {criterion.Value.Count} (max: 100).");                    
                }
                mergedCriteria[criterion.Key] = criterion.Value; 
            }

            return mergedCriteria;         
        }      
    }
}