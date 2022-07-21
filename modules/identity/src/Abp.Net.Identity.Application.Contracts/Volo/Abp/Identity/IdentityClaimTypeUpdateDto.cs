using Volo.Abp.ObjectExtending;

namespace Volo.Abp.Identity
{
    public class IdentityClaimTypeUpdateDto : ExtensibleObject
    {
        public string Name { get; set; }

        public bool Required { get; set; }

        public string Regex { get; set; }

        public string RegexDescription { get; set; }

        public string Description { get; set; }

        public IdentityClaimValueType ValueType { get; set; }

        public IdentityClaimTypeUpdateDto()
            : base(false)
        {
        }
    }
}
