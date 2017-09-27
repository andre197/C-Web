namespace Social_Network.Models.Validations
{
    using System.ComponentModel.DataAnnotations;

    public class TagAttribute : ValidationAttribute
    {
        public TagAttribute()
        {
            this.ErrorMessage = "Tag is invalid!";
        }

        public override bool IsValid(object value)
        {
            string tag = value as string;

            if (tag == null)
            {
                return true;
            }
            if (!tag.StartsWith('#')
                || tag.Length > 20
                || tag.Contains(" "))
            {
                return false;
            }

            return true;
        }
    }
}
