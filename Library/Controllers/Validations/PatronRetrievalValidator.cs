using LibraryNet2020.Models;

namespace LibraryNet2020.Controllers.Validations
{
    public class PatronRetrievalValidator: Validator
    {
        private int Id { get; }
        private Patron Patron { get; set; }

        public PatronRetrievalValidator(LibraryContext context, int id)
            : base(context) =>
            Id = id;

        public override void Validate()
        {
            Patron = context.GetById(context.Patrons, Id);
        }

        public override bool IsValid => Patron != null;

        public override string ErrorMessage => $"Patron with ID ${Id} not found";
    }
}