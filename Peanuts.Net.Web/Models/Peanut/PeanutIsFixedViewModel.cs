namespace Com.QueoFlow.Peanuts.Net.Web.Models.Peanut {
    public class PeanutIsFixedViewModel {
        public PeanutIsFixedViewModel(Core.Domain.Peanuts.Peanut peanut) {
            Peanut = peanut;
        }

        public Core.Domain.Peanuts.Peanut Peanut {
            get; set;
        }
    }
}